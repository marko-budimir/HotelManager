using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Repository;
using HotelManager.Repository.Common;
using HotelManager.Service.Common;
using HotelManager.WebApi.Models;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Web.UI;
using System.Linq;

namespace HotelManager.Service
{
    public class ReceiptService : IReceiptService
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IServiceInvoiceRepository _invoiceServiceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IReservationRepository _reservationRepository;

        public ReceiptService(IReceiptRepository receiptRepository, IServiceInvoiceRepository invoiceServiceRepository, IUserRepository userRepository, IReservationRepository reservationRepository)
        {
            _receiptRepository = receiptRepository;
            _invoiceServiceRepository = invoiceServiceRepository;
            _userRepository = userRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task<List<IReceipt>> GetAllAsync([FromUri]ReceiptFilter filter, Sorting sorting, Paging paging)
        {
            try
            {
                return await _receiptRepository.GetAllAsync(filter, sorting, paging);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<InvoiceReceipt> GetByIdAsync(Guid id)
        {
            try
            {
                return await _receiptRepository.GetByIdAsync(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<IServiceInvoiceHistory>> GetServiceInvoiceByInvoiceIdAsync(Guid id)
        {
            try
            {
                return await _invoiceServiceRepository.GetServiceInvoiceByInvoiceIdAsync(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            return await _invoiceServiceRepository.UpdateAsync(id);
        }

        public async Task<List<IServiceInvoice>> GetAllInvoiceServiceAsync([FromUri]Sorting sorting, Paging paging)
        {
            try
            {
                return await _invoiceServiceRepository.GetAllInvoiceServiceAsync(sorting, paging);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> CreateInvoiceServiceAsync(IServiceInvoice serviceInvoice)
        {
            try
            {
                return await _invoiceServiceRepository.CreateInvoiceServiceAsync(serviceInvoice);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
        {
            try { 
                return await    _receiptRepository.CreateInvoiceAsync(invoice);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Invoice> PutTotalPriceAsync(Guid invoiceId,InvoiceUpdate invoiceUpdate)
        {
            try
            {
                return await _receiptRepository.PutTotalPriceAsync(invoiceId,invoiceUpdate);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

       

        public async Task<bool> SendReceiptAsync(Guid id)
        {
            InvoiceReceipt receipt = await _receiptRepository.GetByIdAsync(id);
            if (receipt == null)
            {
                throw new Exception("Receipt not found");
            }

            PdfDocument pdfDocument = await CreateReceiptPdf(receipt);
            MemoryStream memoryStream = SavePdfToMemoryStream(pdfDocument);

            await SendEmailWithAttachment(receipt, memoryStream);

            return true;
        }

        private async Task<PdfDocument> CreateReceiptPdf(InvoiceReceipt receipt)
        {
            PdfDocument pdfDocument = new PdfDocument();
            PdfPage page = pdfDocument.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            var services = await _invoiceServiceRepository.GetServiceInvoiceByInvoiceIdAsync(receipt.Id);
            AddReceiptContent(gfx, receipt, services);

            return pdfDocument;
        }

        private void AddReceiptContent(XGraphics gfx, InvoiceReceipt receipt, List<IServiceInvoiceHistory> services)
        {
            XFont titleFont = new XFont("Arial", 18, XFontStyle.Bold);
            XFont regularFont = new XFont("Arial", 12);
            XFont boldFont = new XFont("Arial", 12, XFontStyle.Bold);

            gfx.DrawString("Receipt", titleFont, XBrushes.Black, new XRect(0, 20, gfx.PageSize.Width, 0), XStringFormats.TopCenter);

            gfx.DrawString($"Receipt Number: {receipt.InvoiceNumber}", regularFont, XBrushes.Black, 50, 70);
            gfx.DrawString($"Issue Date: {DateTime.Now:dd.MM.yyyy}", regularFont, XBrushes.Black, 50, 90);

            gfx.DrawString($"Check-In Date: {receipt.CheckInDate:dd.MM.yyyy}", regularFont, XBrushes.Black, 50, 110);
            gfx.DrawString($"Check-Out Date: {receipt.CheckOutDate:dd.MM.yyyy}", regularFont, XBrushes.Black, 50, 130);
            gfx.DrawString($"Room Number: {receipt.RoomNumber}", regularFont, XBrushes.Black, 50, 150);

            gfx.DrawString("\nGuest Information:", regularFont, XBrushes.Black, 50, 180);
            gfx.DrawString($"Name: {receipt.FirstName} {receipt.LastName}", regularFont, XBrushes.Black, 50, 200);
            gfx.DrawString($"Email: {receipt.Email}", regularFont, XBrushes.Black, 50, 220);

            gfx.DrawString("\nReservation Details:", regularFont, XBrushes.Black, 50, 270);
            gfx.DrawString($"Number of Nights: {receipt.CheckOutDate.Subtract(receipt.CheckInDate).Days}", regularFont, XBrushes.Black, 50, 290);
            gfx.DrawString($"Price per Night: {receipt.PricePerNight:0.00}€", regularFont, XBrushes.Black, 50, 310);
            gfx.DrawString($"Subtotal: {receipt.TotalPrice:0.00}€", boldFont, XBrushes.Black, 50, 330);
            if (receipt.DiscountCode != null)
            {
                gfx.DrawString($"Discount: {receipt.DiscountCode} ({receipt.DiscountPercent}%)", regularFont, XBrushes.Black, 50, 350);
                receipt.TotalPrice -= receipt.TotalPrice * receipt.DiscountPercent / 100;
                gfx.DrawString($"Subtotal with discount: {receipt.TotalPrice:0.00}€", boldFont, XBrushes.Black, 50, 370);

            }

            gfx.DrawString("\nServices (price):", regularFont, XBrushes.Black, 50, 410);
            int yOffset = 430;
            var servicesMap = new Dictionary<string, int>();
            decimal subTotal = 0;

            foreach (var service in services)
            {
                if (servicesMap.ContainsKey(service.ServiceName))
                {
                    servicesMap[service.ServiceName] += service.Quantity;
                }
                else
                {
                    servicesMap.Add(service.ServiceName, service.Quantity);
                }

                subTotal += service.Quantity * service.Price;
            }

            foreach (var entry in servicesMap)
            {
                var service = services.First(s => s.ServiceName == entry.Key);
                gfx.DrawString($"{entry.Key} ({service.Price}€) x {entry.Value}", regularFont, XBrushes.Black, 50, yOffset);
                yOffset += 20;
            }

            gfx.DrawString($"Subtotal: {subTotal}€", boldFont, XBrushes.Black, 50, yOffset);

            gfx.DrawString("\nTotal Price:", boldFont, XBrushes.Black, 50, yOffset + 30);
            gfx.DrawString($"{receipt.TotalPrice + subTotal}€", boldFont, XBrushes.Black, 50, yOffset + 50);
        }

        private MemoryStream SavePdfToMemoryStream(PdfDocument pdfDocument)
        {
            MemoryStream memoryStream = new MemoryStream();
            pdfDocument.Save(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

        private async Task SendEmailWithAttachment(InvoiceReceipt receipt, MemoryStream memoryStream)
        {
            using (MailMessage mailMessage = new MailMessage("hotel@example.com", "recipient.email@example.com"))
            {
                mailMessage.Subject = "Receipt  " + receipt.InvoiceNumber;
                mailMessage.Body = $"Dear {receipt.FirstName} {receipt.LastName}, \nThank you for choosing our hotel for your recent stay. " +
                    $"It was a pleasure to have you as our guest.\nYou can find a copy of your receipt in attachment.";

                memoryStream.Seek(0, SeekOrigin.Begin);
                mailMessage.Attachments.Add(new Attachment(memoryStream, "receipt.pdf", "application/pdf"));

                using (SmtpClient smtpClient = new SmtpClient("sandbox.smtp.mailtrap.io"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential()
                    {
                        UserName = "a7ffb4c2f03522",
                        Password = "bc683a0eda87c2"
                    };
                    smtpClient.EnableSsl = true;

                    // Send email
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }
    }
}
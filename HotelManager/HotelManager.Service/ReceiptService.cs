﻿using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Repository.Common;
using HotelManager.Service.Common;
using HotelManager.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace HotelManager.Service
{
    public class ReceiptService : IReceiptService
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IServiceInvoiceRepository _invoiceServiceRepository;

        public ReceiptService(IReceiptRepository receiptRepository, IServiceInvoiceRepository invoiceServiceRepository)
        {
            _receiptRepository = receiptRepository;
            _invoiceServiceRepository = invoiceServiceRepository;
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

        public async Task<IReceipt> GetByIdAsync(Guid id)
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

        public async Task<ServiceHistoryView> GetServiceInvoiceByInvoiceIdAsync(Guid id)
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
    }
}
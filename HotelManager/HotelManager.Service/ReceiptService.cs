using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Repository.Common;
using HotelManager.Service.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManager.Service
{
    public class ReceiptService : IReceiptService
    {
        private readonly IReceiptRepository _receiptRepository;

        public ReceiptService(IReceiptRepository receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }

        public async Task<List<IReceipt>> GetAllAsync(ReceiptFilter filter, Sorting sorting, Paging paging)
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

        public async Task<int> DeleteAsync(Guid id, IReceipt receipt)
        {
            return await _receiptRepository.UpdateAsync(id, receipt);
        }
    }
}
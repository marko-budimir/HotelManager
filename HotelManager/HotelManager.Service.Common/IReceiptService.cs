﻿using HotelManager.Common;
using HotelManager.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Service.Common
{
    public interface IReceiptService
    {

        Task<List<IReceipt>> GetAllAsync(ReceiptFilter filter, Sorting sorting, Paging paging);
        Task<IReceipt> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id, IReceipt newReceipt);
    }
}
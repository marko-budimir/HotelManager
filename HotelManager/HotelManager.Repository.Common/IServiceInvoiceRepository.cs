﻿using HotelManager.Common;
using HotelManager.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository.Common
{
    public interface IServiceInvoiceRepository
    {
        Task<List<IServiceInvoice>> GetAllInvoiceServiceAsync(Sorting sorting, Paging paging);
        Task<int> UpdateAsync(Guid id);
        Task<string> CreateInvoiceAsync(IServiceInvoice serviceInvoice);
    }
}
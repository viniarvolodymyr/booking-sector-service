﻿using SoftServe.BookingSectors.WebAPI.BLL.DTO;
using SoftServe.BookingSectors.WebAPI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftServe.BookingSectors.WebAPI.BLL.Interfaces
{
    public interface IBookingSectorService
    {
        Task<IEnumerable<BookingSectorDTO>> GetBookingSectorsAsync();
        Task<BookingSectorDTO> GetBookingByIdAsync(int id);
        Task<IEnumerable<SectorDTO>> GetFreeSectorsAsync(DateTime fromDate, DateTime toDate);
        void UpdateBookingApproved(int id, bool isApproved);
        Task DeleteBookingByIdAsync(int id);
        void Dispose();
    }
}
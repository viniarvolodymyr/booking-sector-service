﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoftServe.BookingSectors.WebAPI.BLL.DTO;
using SoftServe.BookingSectors.WebAPI.BLL.Services.Interfaces;
using SoftServe.BookingSectors.WebAPI.DAL.Models;
using SoftServe.BookingSectors.WebAPI.DAL.UnitOfWork;

namespace SoftServe.BookingSectors.WebAPI.BLL.Services
{
    public class BookingSectorService : IBookingSectorService
    {
        private readonly IUnitOfWork database;
        private readonly IMapper mapper;

        public BookingSectorService(IUnitOfWork database, IMapper mapper)
        {
            this.database = database;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BookingSectorDTO>> GetBookingSectorsAsync()
        {
            var bookings = await database.BookingSectorsRepository.GetAllEntitiesAsync();
            var dtos = mapper.Map<IEnumerable<BookingSector>, IEnumerable<BookingSectorDTO>>(bookings);
            return dtos;
        }
        public async Task<BookingSectorDTO> GetBookingByIdAsync(int id)
        {
            var booking = await database.BookingSectorsRepository.GetEntityByIdAsync(id);
            var dtos = mapper.Map<BookingSector, BookingSectorDTO>(booking);
            return dtos;
        }

        public async Task<IEnumerable<SectorDTO>> GetFreeSectorsAsync(DateTime fromDate, DateTime toDate)
        {
            var bookings = await database.BookingSectorsRepository.GetAllEntitiesAsync();
            var group = bookings.GroupBy(x => x.SectorId).OrderBy(x => x.Key);
            var sectors = group.Select(temp =>
                new
                {
                    Sector = database.SectorsRepository.GetEntityByIdAsync(temp.Key).Result,
                    IsFree = temp.All(b => (!(b.BookingStart >= fromDate && b.BookingStart <= toDate)
                                                    && !(b.BookingEnd >= fromDate && b.BookingEnd <= toDate)))
                });
            var freeSectors = sectors.Where(s => s.IsFree).Select(s => s.Sector);
            var dtos = mapper.Map<IEnumerable<Sector>, IEnumerable<SectorDTO>>(freeSectors);
            return dtos;
        }

        public async Task UpdateBookingApprovedAsync(int id, bool isApproved)
        {
            var booking = await database.BookingSectorsRepository.GetEntityByIdAsync(id);
            if (booking == null)
            {
                throw new NullReferenceException();
            }
            booking.IsApproved = isApproved;
            database.BookingSectorsRepository.UpdateEntity(booking);
            await database.SaveAsync();
        }
        public async Task DeleteBookingByIdAsync(int id)
        {
            await database.BookingSectorsRepository.DeleteEntityByIdAsync(id);
            await database.SaveAsync();
        }    
        public async ValueTask<EntityEntry<BookingSector>> BookSector(int sectorId, DateTime fromDate, DateTime toDate, int userId)
        {
            BookingSector booking = new BookingSector()
            {
                SectorId = sectorId,
                BookingStart = fromDate,
                BookingEnd = toDate,
                UserId = userId,
                CreateUserId = new Random().Next(1, 100)
            };
            EntityEntry<BookingSector> bookingsSector = await database.BookingSectorsRepository.InsertEntityAsync(booking);
            await database.SaveAsync();

            return bookingsSector;
        }
        public void Dispose()
        {
            database.Dispose();
        }

    }
}

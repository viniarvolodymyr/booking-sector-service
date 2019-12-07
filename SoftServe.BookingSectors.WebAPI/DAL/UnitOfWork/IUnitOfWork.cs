﻿using System;
using System.Threading.Tasks;
using SoftServe.BookingSectors.WebAPI.DAL.Models;
using SoftServe.BookingSectors.WebAPI.DAL.Repositories;

namespace SoftServe.BookingSectors.WebAPI.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Sector> Sectors { get; }

        IBaseRepository<User> Users { get; }

        IBaseRepository<TournamentSector> TournamentSectors { get; }
   
        Task<bool>  SaveAsync();

    }
}

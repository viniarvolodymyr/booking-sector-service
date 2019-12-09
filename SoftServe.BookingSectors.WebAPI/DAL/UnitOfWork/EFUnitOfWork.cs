﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoftServe.BookingSectors.WebAPI.DAL.EF;
using SoftServe.BookingSectors.WebAPI.DAL.Models;
using SoftServe.BookingSectors.WebAPI.DAL.Repositories;
using SoftServe.BookingSectors.WebAPI.DAL.Repositories.ImplementationRepositories;


namespace SoftServe.BookingSectors.WebAPI.DAL.UnitOfWork
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly BookingSectorContext context;
        private TournamentSectorRepository tournamentSectorsRepository;
        private TournamentRepository tournamentRepository;
        private bool disposed = false;
        public EFUnitOfWork(BookingSectorContext context)
        {
            this.context = context;
        }
      
        public IBaseRepository<TournamentSector> TournamentSectorsRepository
        {
            get { return tournamentSectorsRepository ??= new TournamentSectorRepository(context); }
        }


        public IBaseRepository<Tournament> TournamentRepository
        {
            get { return tournamentRepository ??= new TournamentRepository(context); }
        }   
        public async Task<bool> SaveAsync()
        {
            try
            {
                var changes = context.ChangeTracker.Entries().Count(
                    p => p.State == EntityState.Modified || p.State == EntityState.Deleted
                                                         || p.State == EntityState.Added);
                if (changes == 0) return true;

                return await context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }

                disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}

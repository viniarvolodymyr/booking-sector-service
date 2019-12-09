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

        private SectorRepository sectorsRepository;
        private UserRepository usersRepository;
        private TournamentSectorRepository tournamentSectorsRepository;
        private TournamentRepository tournamentRepository;
        private BookingSectorRepository bookingRepository;
        private bool disposed = false;
        public EFUnitOfWork(BookingSectorContext context)
        {
            this.context = context;
        }

        public IBaseRepository<TournamentSector> TournamentSectorsRepository
        {
            get { return tournamentSectorsRepository ??= new TournamentSectorRepository(context); }
        }

        public IBaseRepository<BookingSector> BookingSectorsRepository
        {
            get { return bookingRepository ??= new BookingSectorRepository(context); }
        }
        public IBaseRepository<User> UsersRepository
        {
            get { return usersRepository ??= new UserRepository(context); }
        }


        public IBaseRepository<Tournament> TournamentRepository
        {
            get { return tournamentRepository ??= new TournamentRepository(context); }
        }
        public IBaseRepository<Sector> SectorsRepository
        {
            get { return sectorsRepository ??= new SectorRepository(context); }
        }


        public IBaseRepository<BookingSector> BookingSectorRepository
        {
            get { return bookingRepository ??= new BookingSectorRepository(context); }
        }

        public async Task<bool> SaveAsync()

        {
            try
            {
                var changes = context.ChangeTracker.Entries().Count(
                    p => p.State == EntityState.Modified || p.State == EntityState.Deleted
                                                         || p.State == EntityState.Added);
                if (changes == 0) return true;
                await context.SaveChangesAsync();

                return true;
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

﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoftServe.BookingSectors.WebAPI.DAL.EF;
using SoftServe.BookingSectors.WebAPI.DAL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;

namespace SoftServe.BookingSectors.WebAPI.DAL.Repositories.ImplementationRepositories
{
    public class SectorRepository : IBaseRepository<Sector>
    {
        private readonly BookingSectorContext context;
        private readonly DbSet<Sector> sectorSet;

        public SectorRepository(BookingSectorContext context)
        {
            this.context = context;
            sectorSet = context.Set<Sector>();
        }

        public Task<List<Sector>> GetAllEntitiesAsync()
        {
            return sectorSet.AsNoTracking().ToListAsync();
        }

        public Task<Sector> GetEntityByIdAsync(int id)
        {
            return sectorSet.AsNoTracking().Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public IQueryable<Sector> GetByCondition(Expression<Func<Sector, bool>> expression)
        {
            return sectorSet.Where(expression).AsNoTracking().AsQueryable();
        }

        public ValueTask<EntityEntry<Sector>> InsertEntityAsync(Sector entityToInsert)
        {
            return sectorSet.AddAsync(entityToInsert);
        }

        public void UpdateEntity(Sector entityToUpdate)
        {
            sectorSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public async Task<EntityEntry<Sector>> DeleteEntityByIdAsync(int id)
        {
            Sector entityToDelete = await sectorSet.FindAsync(id);
            return sectorSet.Remove(entityToDelete);
        }
    }
}
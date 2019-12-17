using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoftServe.BookingSectors.WebAPI.BLL.ErrorHandling;
using SoftServe.BookingSectors.WebAPI.DAL.EF;
using SoftServe.BookingSectors.WebAPI.DAL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SoftServe.BookingSectors.WebAPI.BLL.Helpers.LoggerManager;

namespace SoftServe.BookingSectors.WebAPI.DAL.Repositories.ImplementationRepositories
{
    public class UserRepository : IBaseRepository<User>
    {

        private readonly BookingSectorContext context;
        private readonly DbSet<User> userSet;

        public UserRepository(BookingSectorContext context)
        {
            this.context = context;
            userSet = context.Set<User>();
        }
        public Task<List<User>> GetAllEntitiesAsync()
        {
            return userSet.Include(x => x.Role).AsNoTracking().ToListAsync();
        }
        public Task<User> GetEntityByIdAsync(int id)
        {
            var result = userSet.Include(x => x.Role).AsNoTracking().Where(e => e.Id == id).FirstOrDefaultAsync();
            if (result.Result == null)
            {
                
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"User with id: {id} not found when trying to get entity.");
            }
            return result;
        }

        public ValueTask<EntityEntry<User>> InsertEntityAsync(User entityToInsert)
        {
            return userSet.AddAsync(entityToInsert);
        }

        public void UpdateEntity(User entityToUpdate)
        {
            userSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
        public async Task<EntityEntry<User>> DeleteEntityByIdAsync(int id)
        {
            var entityToDelete = await userSet.FindAsync(id);
            if (entityToDelete == null)
            {
                
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"User with id: {id} not found when trying to update entity. Entity was no Deleted.");
            }
            return userSet.Remove(entityToDelete);
        }
    }
}

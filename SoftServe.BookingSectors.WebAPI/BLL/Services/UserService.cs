using AutoMapper;
using SoftServe.BookingSectors.WebAPI.BLL.DTO;
using SoftServe.BookingSectors.WebAPI.BLL.Helpers;
using SoftServe.BookingSectors.WebAPI.BLL.Services.Interfaces;
using SoftServe.BookingSectors.WebAPI.DAL.Models;
using SoftServe.BookingSectors.WebAPI.DAL.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoftServe.BookingSectors.WebAPI.BLL.Helpers.LoggerManager;
using SoftServe.BookingSectors.WebAPI.BLL.ErrorHandling;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SoftServe.BookingSectors.WebAPI.BLL.Filters;


namespace SoftServe.BookingSectors.WebAPI.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork database;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;

        public UserService(IUnitOfWork database, IMapper mapper, ILoggerManager logger)
        {
            this.database = database;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await database.UserRepository.GetAllEntitiesAsync();
            var dtos = mapper.Map<IEnumerable<User>, List<UserDTO>>(users);
            return dtos;
        }
        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var entity = await database.UserRepository.GetEntityByIdAsync(id);
            var dto = mapper.Map<User, UserDTO>(entity);
            return dto;
        }

        public async Task<UserDTO> GetUserByPhoneAsync(string phone)
        {
            var user = await database.UserRepository
                .GetByCondition(x => x.Phone == phone)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"User with phone number: {phone} not found when trying to get entity.");
            }

            var dto = mapper.Map<User, UserDTO>(user);
            return dto;
        }

        public async Task<User> UpdateUserById(int id, UserDTO userDTO)
        {
            var existedUser = await database.UserRepository.GetEntityByIdAsync(id);
            if (existedUser == null)
            {
                return null;
            }

            var user = mapper.Map<UserDTO, User>(userDTO);
            user.Firstname = userDTO.Firstname;
            user.Lastname = userDTO.Lastname;
            user.Phone = userDTO.Phone;
            user.Password = SHA256Hash.Compute(userDTO.Password);
            user.ModDate = System.DateTime.Now;

            database.UserRepository.UpdateEntity(user);

            bool isSaved = await database.SaveAsync();

            return (isSaved == true) ? user : null;
        }

        public async Task<UserDTO> InsertUserAsync(UserDTO userDTO)
        {
            var insertedUser = mapper.Map<UserDTO, User>(userDTO);
            string randomPassword = RandomNumbers.Generate();
            insertedUser.Password = SHA256Hash.Compute(randomPassword);
            insertedUser.ModUserId = null;

            await database.UserRepository.InsertEntityAsync(insertedUser);
            bool isSaved = await database.SaveAsync();

            if (isSaved == false)
            {
                return null;
            }
            else
            {
                return mapper.Map<User, UserDTO>(insertedUser);
            }
        }

        public async Task<User> DeleteUserByIdAsync(int id)
        {
            var user = await database.UserRepository.DeleteEntityByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            bool isSaved = await database.SaveAsync();

            return (isSaved == true) ? user.Entity : null;
        }
    }
}

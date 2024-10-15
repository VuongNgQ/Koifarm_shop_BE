﻿using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepo
{
    public interface IUserRepo:IBaseRepo<User>
    {
        Task<User> CreateUser(User user);
        Task<User> CreateStaff(User user);
        Task<User> UpdateUser(int id, User newUser);
        Task<User> UpdateProfile(int id, User user);
        Task<bool> DeleteUser(int id);
        Task<User?> GetById(int id);
        Task<User> GetManager();
        Task<bool> isManager(int managerId);
        Task<User?> GetByName(string name);
        Task<User?> GetByEmail(string email);
        Task<IEnumerable<User>> GetAllUser();
        Task<User> Login(string email, string password);
        Task<User?> GetByPhone(string phone);
        Task<bool> SavePasswordResetToken(PasswordResetToken token);
        Task<PasswordResetToken> GetPasswordResetToken(string token);
        Task<bool> DeletePasswordResetToken(string token);

    }
}

using DataAccess.Entity;
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
        Task<User> UpdateUser(int id, User newUser);
        Task<bool> UpdateProfile(User user);
        Task<bool> DeleteUser(int id);
        Task<User?> GetById(int id);
        Task<User> GetManager();
        Task<bool> isManager(int managerId);
        Task<User?> GetByName(string name);
        Task<User?> GetByEmail(string email);
        Task<IEnumerable<User>> GetAllUser();
        Task<User> Login(string email, string password);
        Task<User?> GetByPhone(string phone);
        Task AddToken(PasswordResetToken token);
        Task<PasswordResetToken> GetToken(string token);
        Task RemoveToken(int tokenId);

    }
}

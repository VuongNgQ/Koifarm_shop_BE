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
        Task<bool> DeleteUser(int id);
        Task<User> GetById(int id);
        Task<bool> isManager(int managerId);
        Task<User> GetByName(string name);
        Task<User> GetByEmail(string email);
        Task<IEnumerable<User>> GetAllUser();
        Task<bool> Login(string email, string password);
        Task<User> GetByPhone(string phone);
        Task<bool> LoginCustomer(string email, string password);
        Task<bool> LoginAdmin(string email, string password);
        Task<bool> LoginStaff(string email, string password);
    }
}

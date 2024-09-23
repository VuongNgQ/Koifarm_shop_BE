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
        Task<User> UpdateUser(User user);
        Task<User> DeleteUser(int id);
        Task<User> GetById(int id);
        Task<User> GetByName(string name);
        Task<User> GetByEmail(string email);
        Task<IEnumerable<User>> GetAllUser();
    }
}

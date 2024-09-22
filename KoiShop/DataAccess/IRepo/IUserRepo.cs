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
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<User> DeleteAsync(int id);
        Task<User> GetByIdAsync(int id);
        Task<User> GetByNameAsync(string name);
        Task<User> GetByEmailAsync(string email);

    }
}

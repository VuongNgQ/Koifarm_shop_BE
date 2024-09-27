using DataAccess.Entity;
using DataAccess.IRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class UserRepo : BaseRepo<User>, IUserRepo
    {
        private readonly KoiShopContext _context;
        public UserRepo(KoiShopContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> CreateUser(User user)
        {
            await _context.Set<User>().AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var exist = await _context.Set<User>().FirstOrDefaultAsync(e => e.UserId == id);
            if (exist != null)
            {
                _context.Remove(exist);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<User>> GetAllUser()
        {
            return await _context.Set<User>().ToListAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(e=>e.Email.Equals(email));
        }
        public async Task<User> GetByName(string name)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(e => e.Name.Equals(name));
        }

        public async Task<User> GetByPhone(string phone)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(e => e.Phone == phone);
        }

        public async Task<bool> Login(string email, string password)
        {
            return await _context.Set<User>().AnyAsync(e => e.Email == email && e.Password == password);
        }

        public async Task<User> UpdateUser(int id, User newUser)
        {
            var exist = await _context.Set<User>().FirstOrDefaultAsync(e => e.UserId == id);
            if (exist != null)
            {
                exist.Status = newUser.Status;
                exist.Email = newUser.Email;
                exist.Name = newUser.Name;
                exist.RoleId = newUser.RoleId;
                exist.Password = newUser.Password;
                exist.Phone = newUser.Phone;
                exist.DateOfBirth=newUser.DateOfBirth; 
                _context.Update(exist);
                await _context.SaveChangesAsync();
                return newUser;
            }
            else
            {
                throw new Exception("User not found");
            }
        }
    }
}

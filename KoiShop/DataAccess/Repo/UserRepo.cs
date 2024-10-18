using DataAccess.Entity;
using DataAccess.IRepo;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repo
{
    public class UserRepo : BaseRepo<User>, IUserRepo
    {
        private readonly new KoiShopContext _context;
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
        public async Task<User> CreateStaff(User user)
        {
            _context.Set<User>().Add(user);
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
            return await _context.Set<User>().Include(u=>u.UserCarts).Include(u=>u.UserAddresses).ThenInclude(u=>u.Address).ToListAsync();
        }
        public async Task<User> GetManager()
        {
            var user = await _context.Set<User>().FirstOrDefaultAsync(u => u.RoleId == 1);
            return user ?? throw new InvalidOperationException("Manager not found.");
        }
        public async Task<User?> GetByEmail(string email)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(e=>e.Email.Equals(email));
        }

        public async Task<User?> GetById(int id)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(e => e.UserId.Equals(id));
        }

        public async Task<User?> GetByName(string name)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(e => e.Name.Equals(name));
        }

        public async Task<User?> GetByPhone(string phone)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(e => e.Phone == phone);
        }

        public async Task<bool> isManager(int managerRoleId)
        {
            return await _context.Set<User>().AnyAsync(e =>managerRoleId==1);
        }
        public async Task<User> Login(string email, string password)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(e => e.Email == email && e.Password == password);
            if (user == null)
            {
                throw new Exception("Invalid email or password");
            }
            return user;
        }
        public async Task<User> UpdateProfile(int id, User user)
        {
            var exist = await _context.Set<User>().FirstOrDefaultAsync(e => e.UserId == id);
            if (exist != null)
            {
                exist.Status = user.Status;
                exist.Email = user.Email;
                exist.Name = user.Name;
                exist.RoleId = user.RoleId;
                exist.Password = user.Password;
                exist.Phone = user.Phone;
                exist.DateOfBirth = user.DateOfBirth;
                _context.Update(exist);
                await _context.SaveChangesAsync();
                return user;
            }
            else
            {
                throw new Exception("User not found");
            }
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
                exist.DateOfBirth = newUser.DateOfBirth;
                _context.Update(exist);
                await _context.SaveChangesAsync();
                return newUser;
            }
            else
            {
                throw new Exception("User not found");
            }
        }
        public async Task<bool> SavePasswordResetToken(PasswordResetToken token)
        {
            _context.PasswordResetTokens.Add(token);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<PasswordResetToken> GetPasswordResetToken(string token)
        {
            return await _context.PasswordResetTokens.FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task<bool> DeletePasswordResetToken(string token)
        {
            var resetToken = await GetPasswordResetToken(token);
            if (resetToken != null)
            {
                _context.PasswordResetTokens.Remove(resetToken);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

    }
}

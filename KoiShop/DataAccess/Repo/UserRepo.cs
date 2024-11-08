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
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == user.UserId);
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
            return await _context.Set<User>().Include(u => u.Role).Include(u=>u.UserCart).Include(u=>u.UserAddresses).ThenInclude(u=>u.Address).ToListAsync();
        }
        public async Task<User> GetManager()
        {
            var user = await _context.Set<User>().FirstOrDefaultAsync(u => u.RoleId == 1);
            return user ?? throw new InvalidOperationException("Manager not found.");
        }
        public async Task<User?> GetByEmail(string email)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(e=>e.Email.Equals(email));
        }

        public async Task<User?> GetById(int id)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(e => e.UserId.Equals(id));
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
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(e => e.Email == email);
            if (user == null)
            {
                throw new Exception("Invalid email or password");
            }
            return user;
        }
        public async Task<bool> UpdateProfile(User user)
        {
            var exist = await _context.Set<User>().FirstOrDefaultAsync(e => e.UserId == user.UserId);
            if (exist != null)
            {
                exist.Email = user.Email;
                exist.Name = user.Name;
                exist.PasswordHash = user.PasswordHash;
                exist.Phone = user.Phone;
                exist.DateOfBirth = user.DateOfBirth;
                _context.Update(exist);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
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
                exist.PasswordHash = newUser.PasswordHash;
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

        public async Task<PasswordResetToken> GetToken(string token)
        {
            return await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.Token == token && t.ExpirationTime > DateTime.UtcNow);
        }

        public async Task AddToken(PasswordResetToken token)
        {
            await _context.PasswordResetTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveToken(int tokenId)
        {
            var token = await _context.PasswordResetTokens.FindAsync(tokenId);
            if (token != null)
            {
                _context.PasswordResetTokens.Remove(token);
                await _context.SaveChangesAsync();
            }
        }
    }
}

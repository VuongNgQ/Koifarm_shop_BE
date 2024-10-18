using DataAccess.Entity;
using DataAccess.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class UserAddressRepo:BaseRepo<UserAddress>, IUserAddressRepo
    {
        private readonly KoiShopContext _context;
        public UserAddressRepo(KoiShopContext context):base(context) 
        {
            _context = context;
        }
    }
}

using DataAccess.Entity;
using DataAccess.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class SubImageRepo:BaseRepo<SubImage>, ISubImageRepo
    {
        private readonly KoiShopContext _context;
        public SubImageRepo(KoiShopContext context):base(context) 
        {
            _context = context;
        }

    }
}

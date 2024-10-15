using DataAccess.Entity;
using DataAccess.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class FishRepo:BaseRepo<FishSingle>, IFishRepo
    {
        private readonly KoiShopContext _context;
        public FishRepo(KoiShopContext context):base(context) 
        {
            _context = context;
        }
    }
}

using DataAccess.Entity;
using DataAccess.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class OrderRepo:BaseRepo<Order>, IOrderRepo
    {
        private readonly KoiShopContext _context;
        public OrderRepo(KoiShopContext context):base(context)
        {
            
        }
    }
}

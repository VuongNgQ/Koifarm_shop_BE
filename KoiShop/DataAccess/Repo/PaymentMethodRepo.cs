using DataAccess.Entity;
using DataAccess.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class PaymentMethodRepo:BaseRepo<PaymentMethod>, IPaymentMethodRepo
    {
        private readonly KoiShopContext _context;
        public PaymentMethodRepo(KoiShopContext context):base(context) 
        {
            
        }
    }
}

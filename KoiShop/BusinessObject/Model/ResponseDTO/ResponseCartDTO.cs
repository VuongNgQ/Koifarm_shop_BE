using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.ResponseDTO
{
    public class ResponseCartDTO
    {
        public int UserCartId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public ICollection<ResponseCartItemDTO>? CartItems { get; set; }
    }
}

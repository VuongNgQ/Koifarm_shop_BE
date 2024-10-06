using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface IOrderStatusService
    {
        Task<ServiceResponseFormat<PaginationModel<OrderStatus>>> GetAllOrderStatus(int page = 1, int pageSize = 10,
            string search = "", string sort = "");

    }
}

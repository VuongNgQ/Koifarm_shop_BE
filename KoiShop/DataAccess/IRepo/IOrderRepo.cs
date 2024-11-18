using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepo
{
    public interface IOrderRepo:IBaseRepo<Order>
    {
        Task<IEnumerable<Order>> GetAllOrder();
        Task<Order> GetByIdWithItemsAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByUserId(int userId);
        Task UpdateOrder(Order order);
    }
}

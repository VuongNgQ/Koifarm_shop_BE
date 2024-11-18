using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.RequestDTO.UpdateReq.Entity;
using BusinessObject.Model.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface IOrderService
    {
        Task<ServiceResponseFormat<ResponseOrderDTO>> CreateOrderWithItems(CreateOrderDTO orderDTO);
        Task<ServiceResponseFormat<PaginationModel<ResponseOrderDTO>>> GetOrders(int page, int pageSize,
            string? search, string sort);
        Task<ServiceResponseFormat<ResponseOrderDTO>> GetOrderById(int id);
        Task<ServiceResponseFormat<IEnumerable<ResponseOrderDTO>>> GetOrdersByUserIdAsync(int userId);
        Task<ServiceResponseFormat<ResponseOrderDTO>> UpdateOrder(int id, UpdateOrderDTO orderDTO);
        Task<ServiceResponseFormat<bool>> DeleteOrder(int id);
        Task<ServiceResponseFormat<bool>> ChangeStatus(int id, string status);
        Task<ServiceResponseFormat<bool>> FinishOrder(int id);
        Task<ServiceResponseFormat<bool>> CancelOrder(int id);
        Task MarkOrderAsCompleted(int orderId);
    }
}

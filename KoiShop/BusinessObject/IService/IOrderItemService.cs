using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface IOrderItemService
    {
        Task<ServiceResponseFormat<ResponseOrderItemDTO>> CreateFishItem(CreateOrderItemDTO itemDTO);
        Task<ServiceResponseFormat<ResponseOrderItemDTO>> CreatePackageItem(CreateOrderItemDTO itemDTO);
        Task<ServiceResponseFormat<PaginationModel<ResponseOrderItemDTO>>> GetAllItem(int page, int pageSize, string sort);
        Task<ServiceResponseFormat<bool>> DeleteOrderItemById(int id);
        Task<ServiceResponseFormat<bool>> UpdateFishQuantity(int id, int quantity);
        Task<ServiceResponseFormat<IEnumerable<ResponseOrderItemDTO>>> GetItemByOrderId(int id);
    }
}

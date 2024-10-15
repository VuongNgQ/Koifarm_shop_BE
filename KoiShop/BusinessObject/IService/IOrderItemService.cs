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
        Task<ServiceResponseFormat<ResponseOrderItemDTO>> CreateFishItem(CreateFishItemDTO itemDTO);
        Task<ServiceResponseFormat<ResponseOrderItemDTO>> CreatePackageItem(CreatePackageItemDTO itemDTO);
        Task<ServiceResponseFormat<PaginationModel<ResponseOrderItemDTO>>> GetAllItem(int page, int pageSize,
            string search, string sort);
        Task<ServiceResponseFormat<bool>> DeleteOrderItemById(int id);
        Task<ServiceResponseFormat<bool>> UpdateFishQuantity(int id, int quantity);
        Task<ServiceResponseFormat<IEnumerable<ResponseOrderItemDTO>>> GetItemByOrderId(int id);
    }
}

using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface ICartItemService
    {
        Task<ServiceResponseFormat<ResponseCartItemDTO>> CreateFishItem(CreateFishItemDTO itemDTO);
        Task<ServiceResponseFormat<ResponseCartItemDTO>> CreatePackageItem(CreatePackageItemDTO itemDTO);
        Task<ServiceResponseFormat<PaginationModel<ResponseCartItemDTO>>>GetAllItem(int page , int pageSize ,
            string search , string sort);
        Task<ServiceResponseFormat<bool>> DeleteCartItemById(int id);
        Task<ServiceResponseFormat<bool>> UpdatePackageQuantity(int id ,int? quantity);
        Task<ServiceResponseFormat<IEnumerable<ResponseCartItemDTO>>> GetItemByCartId(int id);
        Task<ServiceResponseFormat<bool>> ChangeStatus(int id, string status);
    }
}

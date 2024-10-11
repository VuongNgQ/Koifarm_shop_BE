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
    public interface IOrderStatusService
    {
        Task<ServiceResponseFormat<PaginationModel<ResponseOrderStatusDTO>>> GetAllOrderStatus(int page = 1, int pageSize = 10,
            string search = "", string sort = "");
        Task<ServiceResponseFormat<ResponseOrderStatusDTO>> CreateOrderStatus(CreateOrderStatusDTO statusDTO);
        Task<ServiceResponseFormat<bool>> DeleteOrderStatusById(int id);

    }
}

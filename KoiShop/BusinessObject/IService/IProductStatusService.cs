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
    public interface IProductStatusService
    {
        Task<ServiceResponseFormat<ResponseProductStatusDTO>> CreateProductStatus(CreateProductStatusDTO status);
        Task<ServiceResponseFormat<PaginationModel<ResponseProductStatusDTO>>> GetProductStatuses(int page, int pageSize,
            string? search, string sort);
        Task<ServiceResponseFormat<ResponseProductStatusDTO>> GetStatusByName(string name);
        Task<ServiceResponseFormat<ResponseProductStatusDTO>> GetStatusById(int id);
        Task<ServiceResponseFormat<bool>> DeleteProductStatus(int id);
        Task<ServiceResponseFormat<bool>> DeleteStatusByName(string name);
    }
}

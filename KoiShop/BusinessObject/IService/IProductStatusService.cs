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
        Task<ServiceResponseFormat<ResponseFishStatusDTO>> CreateFishStatus(CreateFishStatusDTO status);
        Task<ServiceResponseFormat<PaginationModel<ResponseFishStatusDTO>>> GetFishStatuses(int page, int pageSize,
            string? search, string sort);
        Task<ServiceResponseFormat<ResponseFishStatusDTO>> GetStatusByName(string name);
        Task<ServiceResponseFormat<ResponseFishStatusDTO>> GetStatusById(int id);
        Task<ServiceResponseFormat<bool>> DeleteFishStatus(int id);
        Task<ServiceResponseFormat<bool>> DeleteStatusByName(string name);
    }
}

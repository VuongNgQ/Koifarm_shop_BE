using BusinessObject.Model.RequestDTO.UpdateReq.Entity;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface IFishStatusService
    {
        Task<ServiceResponseFormat<PaginationModel<ResponseFishStatusDTO>>> GetAllFishStatus(int page, int pageSize,
            string? search, string sort);
        Task<ServiceResponseFormat<ResponseFishStatusDTO>> GetFishStatusById(int id);
        Task<ServiceResponseFormat<ResponseFishStatusDTO>> GetFishStatusByName(string name);
        Task<ServiceResponseFormat<ResponseFishStatusDTO>> CreateFishStatus(CreateFishStatusDTO statusDTO);

        Task<ServiceResponseFormat<bool>> DeleteFishStatus(int id);
    }
}

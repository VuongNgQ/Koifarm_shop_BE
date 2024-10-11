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
        Task<ServiceResponseFormat<PaginationModel<ResponseFishStatusDTO>>> GetAllStatus(int page, int pageSize,
            string? search, string sort);
        Task<ServiceResponseFormat<ResponseFishStatusDTO>> GetStatusById(int id);
        Task<ServiceResponseFormat<ResponseFishStatusDTO>> GetStatusByName(string name);
        Task<ServiceResponseFormat<ResponseFishStatusDTO>> CreateStatus(CreateFishStatusDTO statusDTO);

        Task<ServiceResponseFormat<bool>> DeleteStatus(int id);
    }
}

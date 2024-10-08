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
        Task<ServiceResponseFormat<IEnumerable<ResponseFishStatusDTO>>> GetAllStatus();
        Task<ServiceResponseFormat<ResponseFishStatusDTO>> CreateStatus(CreateFishStatusDTO statusDTO);
        Task<ServiceResponseFormat<ResponseFishStatusDTO>> UpdateStatus(UpdateFishStatusDTO statusDTO);
        Task<ServiceResponseFormat<bool>> DeleteStatus(int id);
    }
}

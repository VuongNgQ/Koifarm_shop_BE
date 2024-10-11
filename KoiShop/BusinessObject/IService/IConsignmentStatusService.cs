using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface IConsignmentStatusService
    {
        Task<ServiceResponseFormat<ResponseConsignmentStatusDTO>> CreateStatus(CreateConsignmentStatusDTO statusDTO);
        Task<ServiceResponseFormat<PaginationModel<ResponseConsignmentStatusDTO>>> GetAll(int page, int pageSize,
            string? search, string sort);
        Task<ServiceResponseFormat<ResponseConsignmentStatusDTO>>GetById(int id);
        Task<ServiceResponseFormat<bool>> DeleteById(int id);
        Task<ServiceResponseFormat<bool>> DeleteByName(string name);
    }
}

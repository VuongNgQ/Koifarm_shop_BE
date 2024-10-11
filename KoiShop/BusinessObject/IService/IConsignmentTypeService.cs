using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface IConsignmentTypeService
    {
        Task<ServiceResponseFormat<ResponseConsignmentTypeDTO>> CreateType(CreateConsignmentTypeDTO typeDTO);
        Task<ServiceResponseFormat<PaginationModel<ResponseConsignmentTypeDTO>>> GetTypes(int page = 1, int pageSize = 10,
            string search = "", string sort = "");
        Task<ServiceResponseFormat<bool>> DeleteType(int id);
    }
}

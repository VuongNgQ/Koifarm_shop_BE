using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.RequestDTO.UpdateReq.Entity;
using BusinessObject.Model.ResponseDTO;
using BusinessObject.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface ICategoryService
    {
        Task<ServiceResponseFormat<ResponseCategoryDTO>> CreateCategory(CreateCategoryDTO categoryDTO);
        Task<ServiceResponseFormat<PaginationModel<ResponseCategoryDTO>>> GetAll(int page, int pageSize,
            string? search, string sort);
        Task<ServiceResponseFormat<bool>> DeleteCategory(int id);
        Task<ServiceResponseFormat<bool>> UpdateCategory(int id, UpdateCategoryDTO categoryDTO);
    }
}

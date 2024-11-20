using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.RequestDTO.UpdateReq.Entity;
using BusinessObject.Model.ResponseDTO;
using DataAccess.Entity;
using DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface IFishPackageService
    {
        Task<ServiceResponseFormat<PaginationModel<ResponseFishPackageDTO>>> GetFishPackages(int page, int pageSize,
            string? search, string sort);
        Task<ServiceResponseFormat<IEnumerable<ResponseFishPackageDTO>>> GetDisplayablePackage();
        Task<ServiceResponseFormat<ResponseFishPackageDTO>> GetFishPackage(int id);
        Task<ServiceResponseFormat<ResponseFishPackageDTO>> CreatePackage(CreateFishPackageDTO package);
        Task<ServiceResponseFormat<bool>> AddFishToPackage(CreateCategoryPackageDTO categoryDTO);
        Task<ServiceResponseFormat<bool>> UpdateQuantityInPackage(CreateCategoryPackageDTO categoryDTO);
        Task<ServiceResponseFormat<bool>> DeleteCategoryInPackage(int packageId, int categoryId);
        Task<ServiceResponseFormat<ResponseFishPackageDTO>> UpdatePackage(int id, UpdatePackageDTO package);
        Task<ServiceResponseFormat<bool>> DeletePackage(int id);
        Task<ServiceResponseFormat<bool>> ChangeStatus(int id, string status);
        Task<ServiceResponseFormat<bool>> SoldoutPackage(int id);
        Task<ServiceResponseFormat<bool>> RestorePackage(int id);
        Task<ServiceResponseFormat<PaginationModel<ResponseFishAndPackageDTO>>> SearchFishAndPackages(
    int page, int pageSize, string? search, string sort,
    string? productStatus = null, decimal? minSize = null, decimal? maxSize = null,
    decimal? minPrice = null, decimal? maxPrice = null);
    }
}

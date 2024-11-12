using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.RequestDTO.UpdateReq.Entity;
using BusinessObject.Model.ResponseDTO;
using DataAccess.Entity;
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
        Task<ServiceResponseFormat<ResponseFishPackageDTO>> GetFishPackage(int id);
        Task<ServiceResponseFormat<ResponseFishPackageDTO>> CreatePackage(CreateFishPackageDTO package);
        Task<ServiceResponseFormat<ResponseFishPackageDTO>> UpdatePackage(int id, UpdatePackageDTO package);
        Task<ServiceResponseFormat<bool>> DeletePackage(int id);
        Task<ServiceResponseFormat<bool>> SoldoutPackage(int id);
    }
}

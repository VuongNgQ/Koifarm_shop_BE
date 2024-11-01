using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface ISubImageService
    {
        Task<ServiceResponseFormat<List<ResponseSubImageDTO>>> CreateFishSubImage(CreateFishSubImageDTO subImageDTO);
        Task<ServiceResponseFormat<List<ResponseSubImageDTO>>> CreatePackageSubImage(CreatePackageSubImageDTO subImageDTO);
        Task<ServiceResponseFormat<bool>> DeleteSubImage(int id);
        Task<ServiceResponseFormat<IEnumerable<ResponseSubImageDTO>>> GetSubImageByFishId(int id);
        Task<ServiceResponseFormat<IEnumerable<ResponseSubImageDTO>>> GetSubImageByPackageId(int id);
        Task<ServiceResponseFormat<bool>> ChangeSubImage(int imageId, IFormFile image);
    }
}

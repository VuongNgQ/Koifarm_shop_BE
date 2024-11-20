using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface IFishService
    {
        Task<ServiceResponseFormat<List<ResponseFishDTO>>> GetAllFishes();
        Task<ServiceResponseFormat<IEnumerable<ResponseFishDTO>>> GetDisplayableFish();
        Task<ServiceResponseFormat<List<ResponseFishDTO>>> GetFishByCategoryId(int categoryId);
        Task<ServiceResponseFormat<ResponseFishDTO>> GetFishById(int fishId);
        Task<ServiceResponseFormat<ResponseFishDTO>> CreateFish(CreateFishDTO createFishDto);
        Task<ServiceResponseFormat<ResponseFishDTO>> UpdateFish(int fishId, UpdateFishDTO updateFishDto);
        Task<ServiceResponseFormat<bool>> DeleteFish(int fishId);
        Task<ServiceResponseFormat<bool>> SoldoutFish(int id);
        Task<ServiceResponseFormat<bool>> RestoreFish(int id);
        Task<ServiceResponseFormat<bool>> ChangeStatus(int id, string status);
    }
}

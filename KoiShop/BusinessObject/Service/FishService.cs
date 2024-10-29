using AutoMapper;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using BusinessObject.Utils;
using DataAccess.Entity;
using DataAccess.IRepo;
using DataAccess.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public class FishService : IFishService
    {
        private readonly IFishRepo _fishRepository;
        private readonly IMapper _mapper;

        public FishService(IFishRepo fishRepository, IMapper mapper)
        {
            _fishRepository = fishRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponseFormat<List<ResponseFishDTO>>> GetAllFishes()
        {
            var response = new ServiceResponseFormat<List<ResponseFishDTO>>();
            var fishes = await _fishRepository.GetAllAsync();
            response.Data = _mapper.Map<List<ResponseFishDTO>>(fishes);
            response.Success = true;
            return response;
        }
        public async Task<ServiceResponseFormat<ResponseFishDTO>> GetFishById(int fishId)
        {
            var response = new ServiceResponseFormat<ResponseFishDTO>();
            var fish = await _fishRepository.GetFishByIdAsync(fishId);
            if (fish == null)
            {
                response.Success = false;
                response.Message = "Fish not found";
                return response;
            }
            response.Data = _mapper.Map<ResponseFishDTO>(fish);
            response.Success = true;
            return response;
        }
        public async Task<ServiceResponseFormat<ResponseFishDTO>> CreateFish(CreateFishDTO createFishDto)
        {
            var response = new ServiceResponseFormat<ResponseFishDTO>();

            // Kiểm tra xem category có tồn tại không
            var categoryExists = await _fishRepository.CategoryExists(createFishDto.CategoryId);
            if (!categoryExists)
            {
                response.Success = false;
                response.Message = "Category does not exist.";
                return response;
            }
            
            var imageService = new CloudinaryService();
            string uploadedImageUrl = string.Empty;
            
            if (createFishDto.ImageUrl != null)
            {
                // Image is a local file uploaded via a form
                using (var stream = createFishDto.ImageUrl.OpenReadStream())
                {
                    uploadedImageUrl = await imageService.UploadImageAsync(stream, createFishDto.ImageUrl.FileName);
                }
            }
            // Tạo mới cá
            var newFish = _mapper.Map<Fish>(createFishDto);
            newFish.ImageUrl = uploadedImageUrl;
            await _fishRepository.AddFishAsync(newFish);
            response.Data = _mapper.Map<ResponseFishDTO>(newFish);
            response.Success = true;
            response.Message = "Fish created successfully.";
            return response;
        }
        public async Task<ServiceResponseFormat<ResponseFishDTO>> UpdateFish(int fishId, UpdateFishDTO updateFishDto)
        {
            var response = new ServiceResponseFormat<ResponseFishDTO>();
            var fish = await _fishRepository.GetFishByIdAsync(fishId);
            if (fish == null)
            {
                response.Success = false;
                response.Message = "Fish not found";
                return response;
            }
            var imageService = new CloudinaryService();
            string uploadedImageUrl = string.Empty;

            if (updateFishDto.ImageUrl != null)
            {
                // Image is a local file uploaded via a form
                using (var stream = updateFishDto.ImageUrl.OpenReadStream())
                {
                    uploadedImageUrl = await imageService.UploadImageAsync(stream, updateFishDto.ImageUrl.FileName);
                }
            }
            _mapper.Map(updateFishDto, fish); // Cập nhật thông tin cá

            await _fishRepository.UpdateFishAsync(fish);

            response.Data = _mapper.Map<ResponseFishDTO>(fish);
            response.Success = true;
            response.Message = "Fish updated successfully";
            return response;
        }
        public async Task<ServiceResponseFormat<bool>> DeleteFish(int fishId)
        {
            var response = new ServiceResponseFormat<bool>();
            var fish = await _fishRepository.GetFishByIdAsync(fishId);
            if (fish == null)
            {
                response.Success = false;
                response.Message = "Fish not found";
                return response;
            }

            await _fishRepository.DeleteFishAsync(fish);
            response.Success = true;
            response.Message = "Fish deleted successfully";
            response.Data = true;
            return response;
        }
    }
}

using AutoMapper;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using BusinessObject.Utils;
using DataAccess.Entity;
using DataAccess.Enum;
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
        private readonly ICategoryRepo _categoryRepo;
        private readonly IMapper _mapper;

        public FishService(IFishRepo fishRepository, ICategoryRepo categoryRepository, IMapper mapper)
        {
            _fishRepository = fishRepository;
            _categoryRepo = categoryRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponseFormat<bool>> SoldoutFish(int id)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var fishExist = await _fishRepository.GetFishByIdAsync(id);
                if (fishExist != null)
                {
                    fishExist.ProductStatus = ProductStatusEnum.SOLDOUT;
                    res.Data = true;
                    res.Success = true;
                    res.Message = "THIS FISH NOW HAS BEEN SOLD OUT";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "Fish not found?";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to change status:{ex.Message}";
                return res;
            }
        }
        public async Task<ServiceResponseFormat<List<ResponseFishDTO>>> GetAllFishes()
        {
            var response = new ServiceResponseFormat<List<ResponseFishDTO>>();
            var fishes = await _fishRepository.GetAllAsync();
            response.Data = _mapper.Map<List<ResponseFishDTO>>(fishes);
            response.Success = true;
            return response;
        }
        public async Task<ServiceResponseFormat<List<ResponseFishDTO>>> GetFishByCategoryId(int categoryId)
        {
            var response = new ServiceResponseFormat<List<ResponseFishDTO>>();

            var category = await _categoryRepo.GetByIdAsync(categoryId);
            if (category == null)
            {
                response.Success = false;
                response.Message = "Category không tồn tại.";
                return response;
            }

            var fishes = await _fishRepository.GetByCategoryIdAsync(categoryId);

            if (fishes == null || !fishes.Any())
            {
                response.Success = false;
                response.Message = "Không có cá nào thuộc category này.";
                return response;
            }

            // Map dữ liệu từ Entity sang DTO
            response.Data = _mapper.Map<List<ResponseFishDTO>>(fishes);
            response.Success = true;
            response.Message = $"Có {fishes.Count} cá thuộc category {category.Name}.";

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
            newFish.ProductStatus = ProductStatusEnum.AVAILABLE;
            newFish.Status = FishStatusEnum.GOOD;
            await _fishRepository.AddFishAsync(newFish);
            response.Data = _mapper.Map<ResponseFishDTO>(newFish);
            response.Success = true;
            response.Message = "Fish created successfully.";
            return response;
        }
        /*public async Task<ServiceResponseFormat<ResponseFishDTO>> UpdateFish(int fishId, UpdateFishDTO updateFishDto)
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
        }*/
        public async Task<ServiceResponseFormat<ResponseFishDTO>> UpdateFish(int fishId, UpdateFishDTO updateFishDto)
        {
            var response = new ServiceResponseFormat<ResponseFishDTO>();
            try
            {
                var fish = await _fishRepository.GetFishByIdAsync(fishId);
                if (fish == null)
                {
                    response.Success = false;
                    response.Message = "Fish not found";
                    return response;
                }

                bool isUpdated = false;
                var imageService = new CloudinaryService();
                string uploadedImageUrl = string.Empty;

                // Handle image upload
                if (updateFishDto.ImageUrl != null)
                {
                    using (var stream = updateFishDto.ImageUrl.OpenReadStream())
                    {
                        uploadedImageUrl = await imageService.UploadImageAsync(stream, updateFishDto.ImageUrl.FileName);
                        if (!string.IsNullOrEmpty(uploadedImageUrl))
                        {
                            fish.ImageUrl = uploadedImageUrl;
                            isUpdated = true;
                        }
                    }
                }

                // Update other fields if they have changed
                if (!string.IsNullOrEmpty(updateFishDto.Name) && updateFishDto.Name != fish.Name)
                {
                    fish.Name = updateFishDto.Name;
                    isUpdated = true;
                }

                if (updateFishDto.Age.HasValue && updateFishDto.Age != fish.Age)
                {
                    fish.Age = updateFishDto.Age.Value;
                    isUpdated = true;
                }

                if (Enum.TryParse<FishGenderEnum>(updateFishDto.Gender, true, out var genderEnum) && genderEnum != fish.Gender)
                {
                    fish.Gender = genderEnum;
                    isUpdated = true;
                }

                if (updateFishDto.Size.HasValue && updateFishDto.Size != fish.Size)
                {
                    fish.Size = updateFishDto.Size.Value;
                    isUpdated = true;
                }

                if (!string.IsNullOrEmpty(updateFishDto.Description) && updateFishDto.Description != fish.Description)
                {
                    fish.Description = updateFishDto.Description;
                    isUpdated = true;
                }

                if (updateFishDto.CategoryId.HasValue && updateFishDto.CategoryId.Value != fish.CategoryId)
                {
                    fish.CategoryId = updateFishDto.CategoryId.Value;
                    isUpdated = true;
                }

                if (updateFishDto.Price.HasValue && updateFishDto.Price != fish.Price)
                {
                    fish.Price = updateFishDto.Price.Value;
                    isUpdated = true;
                }

                if (updateFishDto.DailyFood.HasValue && updateFishDto.DailyFood != fish.DailyFood)
                {
                    fish.DailyFood = updateFishDto.DailyFood.Value;
                    isUpdated = true;
                }

                if (Enum.TryParse<ProductStatusEnum>(updateFishDto.ProductStatus, true, out var statusEnum) && statusEnum != fish.ProductStatus)
                {
                    fish.ProductStatus = statusEnum;
                    isUpdated = true;
                }

                // Check if no updates were made
                if (!isUpdated)
                {
                    response.Success = false;
                    response.Message = "No changes detected. Fish was not updated.";
                    return response;
                }

                // Save the updates if changes were made
                await _fishRepository.UpdateFishAsync(fish);

                response.Data = _mapper.Map<ResponseFishDTO>(fish);
                response.Success = true;
                response.Message = "Fish updated successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to update Fish: {ex.Message}";
                return response;
            }
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

using AutoMapper;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using BusinessObject.Utils;
using DataAccess.Entity;
using DataAccess.IRepo;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public class SubImageService:ISubImageService
    {
        private readonly ISubImageRepo _repo;
        private readonly IMapper _mapper;
        private readonly IFishRepo _fishRepo;
        private readonly IFishPackageRepo _fishPackageRepo;
        public SubImageService(ISubImageRepo repo, IMapper mapper, IFishRepo fishRepo
            , IFishPackageRepo fishPackageRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _fishRepo = fishRepo;
            _fishPackageRepo = fishPackageRepo;
        }

        public async Task<ServiceResponseFormat<bool>> ChangeSubImage(int imageId, IFormFile image)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                bool isUpdated=false;
                var exist=await _repo.GetByIdAsync(imageId);
                if (exist != null)
                {
                    var imageService = new CloudinaryService();
                    string uploadedImageUrl = string.Empty;

                    if (image != null)
                    {
                        using (var stream = image.OpenReadStream())
                        {
                            uploadedImageUrl = await imageService.UploadImageAsync(stream, image.FileName.ToString());
                            if (!string.IsNullOrEmpty(uploadedImageUrl))
                            {
                                exist.SubImageURL = uploadedImageUrl;
                                isUpdated = true;
                            }
                        }
                    }
                    if (!isUpdated)
                    {
                        res.Success = false;
                        res.Message = "No changes detected. Image was not updated.";
                        return res;
                    }
                    _repo.Update(exist);
                    res.Success = true;
                    res.Message = "Image updated successfully";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "Id not found";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Failed to update Image: {ex.Message}";
                return res;
            }
        }



        public async Task<ServiceResponseFormat<List<ResponseSubImageDTO>>> CreateFishSubImage(CreateFishSubImageDTO subImageDTO)
        {
            var res = new ServiceResponseFormat<List<ResponseSubImageDTO>>();
            try
            {
                // Check if the fish exists
                var fishExist = await _fishRepo.GetFishByIdAsync(subImageDTO.FishId);
                if (fishExist == null)
                {
                    res.Success = false;
                    res.Message = "No Fish with this Id";
                    return res;
                }

                // Get all existing sub-images for this fish
                var subImages = await _repo.GetAllAsync();
                int existingImageCount = subImages.Count(s => s.FishId == subImageDTO.FishId);

                // Check if adding the new images would exceed the limit of 5
                if (existingImageCount + subImageDTO.SubImageFile.Count() > 5)
                {
                    res.Success = false;
                    res.Message = "You reached Max Images";
                    return res;
                }

                var imageService = new CloudinaryService();
                var uploadedSubImages = new List<ResponseSubImageDTO>();

                // Loop through each image file and upload
                foreach (var imageFile in subImageDTO.SubImageFile)
                {
                    string uploadedImageUrl = string.Empty;

                    // Upload image if it's a valid file
                    if (imageFile != null)
                    {
                        using (var stream = imageFile.OpenReadStream())
                        {
                            uploadedImageUrl = await imageService.UploadImageAsync(stream, imageFile.FileName);
                        }

                        // Map DTO to SubImage entity and save URL
                        var newSubImage = new SubImage
                        {
                            FishId = subImageDTO.FishId,
                            SubImageURL = uploadedImageUrl
                        };

                        await _repo.AddAsync(newSubImage);

                        // Map to response DTO and add to the result list
                        var result = _mapper.Map<ResponseSubImageDTO>(newSubImage);
                        uploadedSubImages.Add(result);
                    }
                }

                res.Success = true;
                res.Message = "Sub Images Created Successfully";
                res.Data = uploadedSubImages;
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create Sub Images: {ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<List<ResponseSubImageDTO>>> CreatePackageSubImage(CreatePackageSubImageDTO subImageDTO)
        {
            var res = new ServiceResponseFormat<List<ResponseSubImageDTO>>();
            try
            {
                // Check if the fish package exists
                var fishPackageExist = await _fishPackageRepo.GetFishPackage(subImageDTO.FishPackageId);
                if (fishPackageExist == null)
                {
                    res.Success = false;
                    res.Message = "No Package with this Id";
                    return res;
                }

                // Get all existing sub-images for this package
                var subImages = await _repo.GetAllAsync();
                int existingImageCount = subImages.Count(s => s.FishPackageId == subImageDTO.FishPackageId);

                // Check if adding the new images would exceed the limit of 5
                if (existingImageCount + subImageDTO.SubImageFile.Count() > 5)
                {
                    res.Success = false;
                    res.Message = "You reached Max Images";
                    return res;
                }

                var imageService = new CloudinaryService();
                var uploadedSubImages = new List<ResponseSubImageDTO>();

                // Loop through each image file and upload
                foreach (var imageFile in subImageDTO.SubImageFile)
                {
                    string uploadedImageUrl = string.Empty;

                    // Upload image if it's a valid file
                    if (imageFile != null)
                    {
                        using (var stream = imageFile.OpenReadStream())
                        {
                            uploadedImageUrl = await imageService.UploadImageAsync(stream, imageFile.FileName);
                        }

                        // Map DTO to SubImage entity and save URL
                        var newSubImage = new SubImage
                        {
                            FishPackageId = subImageDTO.FishPackageId,
                            SubImageURL = uploadedImageUrl
                        };

                        await _repo.AddAsync(newSubImage);

                        // Map to response DTO and add to the result list
                        var result = _mapper.Map<ResponseSubImageDTO>(newSubImage);
                        uploadedSubImages.Add(result);
                    }
                }

                res.Success = true;
                res.Message = "Sub Images Created Successfully";
                res.Data = uploadedSubImages;
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create Sub Images: {ex.Message}";
                return res;
            }
        }


        public async Task<ServiceResponseFormat<bool>> DeleteSubImage(int id)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var exist=await _repo.GetByIdAsync(id);
                if(exist != null)
                {
                    _repo.Remove(exist);
                    res.Success = true;
                    res.Message = "Sub Image Deleted successfully";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Sub Image";
                    return res;
                }
            }
            catch(Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to Delete Image :{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<IEnumerable<ResponseSubImageDTO>>> GetSubImageByFishId(int id)
        {
            var res=new ServiceResponseFormat<IEnumerable<ResponseSubImageDTO>>();
            try
            {
                var list=await _repo.GetAllAsync();
                var fishExist = list.Where(i => i.FishId != null && i.FishId == id).ToList();
                if (fishExist.Count != 0)
                {
                    var result = _mapper.Map<IEnumerable<ResponseSubImageDTO>>(fishExist);
                    res.Success = true;
                    res.Data = result;
                    res.Message = "Get Image Successfully";
                    return res;
                }
                else
                {
                    res.Success = true;
                    res.Message = "No Sub Image for this Fish ID";
                    return res;
                }
            }
            catch(Exception ex)
            {
                res.Success = true;
                res.Message = $"Fail to get Image:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<IEnumerable<ResponseSubImageDTO>>> GetSubImageByPackageId(int id)
        {
            var res = new ServiceResponseFormat<IEnumerable<ResponseSubImageDTO>>();
            try
            {
                var list = await _repo.GetAllAsync();
                var packageExist = list.Where(i => i.FishPackageId != null && i.FishPackageId == id).ToList();
                if (packageExist.Count != 0)
                {
                    var result = _mapper.Map<IEnumerable<ResponseSubImageDTO>>(packageExist);
                    res.Success = true;
                    res.Data = result;
                    res.Message = "Get Image Successfully";
                    return res;
                }
                else
                {
                    res.Success = true;
                    res.Message = "No Sub Image for this Package ID";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = true;
                res.Message = $"Fail to get Image:{ex.Message}";
                return res;
            }
        }
    }
}

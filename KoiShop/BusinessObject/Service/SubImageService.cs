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

        

        public async Task<ServiceResponseFormat<ResponseSubImageDTO>> CreateFishSubImage(CreateFishSubImageDTO subImageDTO)
        {
            var res = new ServiceResponseFormat<ResponseSubImageDTO>();
            try
            {
                var fishExist = await _fishRepo.GetFishByIdAsync(subImageDTO.FishId);
                if (fishExist == null)
                {
                    res.Success = false;
                    res.Message = "No Fish with this Id";
                    return res;
                }
                var subImages=await _repo.GetAllAsync();
                if (subImages.Where(s => s.FishId == subImageDTO.FishId).Count() > 5)
                {
                    res.Success = false;
                    res.Message = "You reached Max Images";
                    return res;
                }
                string uploadedImageUrl = string.Empty;
                var imageService = new CloudinaryService();
                if (subImageDTO.SubImageFile != null)
                {
                    // Image is a local file uploaded via a form
                    using (var stream = subImageDTO.SubImageFile.OpenReadStream())
                    {
                        uploadedImageUrl = await imageService.UploadImageAsync(stream, subImageDTO.SubImageFile.FileName);
                    }
                }
                var mapp = _mapper.Map<SubImage>(subImageDTO);
                mapp.SubImageURL = uploadedImageUrl;
                await _repo.AddAsync(mapp);
                var result = _mapper.Map<ResponseSubImageDTO>(mapp);
                res.Success = true;
                res.Message = "Sub Image Created Successfully";
                res.Data = result;
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create Sub Image:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<ResponseSubImageDTO>> CreatePackageSubImage(CreatePackageSubImageDTO subImageDTO)
        {
            var res = new ServiceResponseFormat<ResponseSubImageDTO>();
            try
            {
                var fishExist = await _fishPackageRepo.GetFishPackage(subImageDTO.FishPackageId);
                if (fishExist == null)
                {
                    res.Success = false;
                    res.Message = "No Package with this Id";
                    return res;
                }
                var subImages = await _repo.GetAllAsync();
                if (subImages.Where(s => s.FishPackageId == subImageDTO.FishPackageId).Count() > 5)
                {
                    res.Success = false;
                    res.Message = "You reached Max Images";
                    return res;
                }
                string uploadedImageUrl = string.Empty;
                var imageService = new CloudinaryService();
                if (subImageDTO.SubImageFile != null)
                {
                    // Image is a local file uploaded via a form
                    using (var stream = subImageDTO.SubImageFile.OpenReadStream())
                    {
                        uploadedImageUrl = await imageService.UploadImageAsync(stream, subImageDTO.SubImageFile.FileName);
                    }
                }
                var mapp = _mapper.Map<SubImage>(subImageDTO);
                mapp.SubImageURL = uploadedImageUrl;
                await _repo.AddAsync(mapp);
                var result = _mapper.Map<ResponseSubImageDTO>(mapp);
                res.Success = true;
                res.Message = "Sub Image Created Successfully";
                res.Data = result;
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create Sub Image:{ex.Message}";
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

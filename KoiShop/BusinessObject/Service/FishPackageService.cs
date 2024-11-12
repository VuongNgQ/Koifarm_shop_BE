using AutoMapper;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.RequestDTO.UpdateReq.Entity;
using BusinessObject.Model.ResponseDTO;
using BusinessObject.Utils;
using DataAccess.Entity;
using DataAccess.Enum;
using DataAccess.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public class FishPackageService:IFishPackageService
    {
        private readonly IFishPackageRepo _repo;
        private readonly IMapper _mapper;
        public FishPackageService(IFishPackageRepo repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<ServiceResponseFormat<ResponseFishPackageDTO>> CreatePackage(CreateFishPackageDTO package)
        {
            var res = new ServiceResponseFormat<ResponseFishPackageDTO>();
            try
            {
                var imageService = new CloudinaryService();
                string uploadedImageUrl = string.Empty;

                if (package.ImageUrl != null)
                {
                    // Image is a local file uploaded via a form
                    using (var stream = package.ImageUrl.OpenReadStream())
                    {
                        uploadedImageUrl = await imageService.UploadImageAsync(stream, package.ImageUrl.FileName);
                    }
                }
                
                var mapp=_mapper.Map<FishPackage>(package);
                
                mapp.ProductStatus = ProductStatusEnum.AVAILABLE;
                mapp.ImageUrl = uploadedImageUrl;
                await _repo.CreatePackage(mapp);
                var result=_mapper.Map<ResponseFishPackageDTO>(mapp);
                res.Success = true;
                res.Message = "Package created successfully";
                res.Data = result;
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create User:{ex.Message}";
                return res;
            }
            
        }
        public async Task<ServiceResponseFormat<bool>> RestorePackage(int id)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var packageExist = await _repo.GetByIdAsync(id);
                if (packageExist != null)
                {
                    packageExist.ProductStatus = ProductStatusEnum.AVAILABLE;
                    res.Data = true;
                    res.Success = true;
                    res.Message = "THIS PACKAGE HAS BEEN SOLD OUT";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "Package not found?";
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
        public async Task<ServiceResponseFormat<bool>> SoldoutPackage(int id)
        {
            var res=new ServiceResponseFormat<bool>();
            try
            {
                var packageExist = await _repo.GetByIdAsync(id);
                if(packageExist != null)
                {
                    packageExist.ProductStatus = ProductStatusEnum.SOLDOUT;
                    res.Data = true;
                    res.Success = true;
                    res.Message = "THIS PACKAGE HAS BEEN SOLD OUT";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "Package not found?";
                    return res;
                }
            }
            catch(Exception ex)
            {
                res.Success=false;
                res.Message=$"Fail to change status:{ex.Message}";
                return res;
            }
        }
        public async Task<ServiceResponseFormat<bool>> DeletePackage(int id)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var result = await _repo.DeletePackage(id);
                if (result)
                {
                    res.Success = true;
                    res.Message = "Package Deleted successfully";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Package";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to Delete Package";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<ResponseFishPackageDTO>> GetFishPackage(int id)
        {
            var res = new ServiceResponseFormat<ResponseFishPackageDTO>();
            try
            {
                var result=await _repo.GetFishPackage(id);
                if (result != null)
                {
                    var mapp = _mapper.Map<ResponseFishPackageDTO>(result);
                    res.Success = true;
                    res.Message = "Get Package successfully";
                    res.Data= mapp; 
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No package";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to Get Package:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<PaginationModel<ResponseFishPackageDTO>>> GetFishPackages(int page, int pageSize,
            string? search, string sort)
        {
            var res = new ServiceResponseFormat<PaginationModel<ResponseFishPackageDTO>>();
            try
            {
                var packages=await _repo.GetFishPackages();
                if (!string.IsNullOrEmpty(search))
                {
                    packages = packages.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase)||
                    p.ProductStatus.ToString().Contains(search, StringComparison.OrdinalIgnoreCase));
                }
                packages = sort.ToLower().Trim() switch
                {
                    "name" => packages.OrderBy(e => e.Name),
                    
                    "fishinpackage" => packages.OrderBy(e => e.NumberOfFish),
                    "price" => packages.OrderBy(e => e.TotalPrice),
                    _=>packages.OrderBy(e=>e.FishPackageId)
                };
                var mapp = _mapper.Map<IEnumerable<ResponseFishPackageDTO>>(packages);
                if (mapp.Any())
                {
                    var paginationModel = await Pagination.GetPaginationEnum(mapp, page, pageSize);
                    res.Success = true;
                    res.Message = "Get Package successfully";
                    res.Data = paginationModel;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Package";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to get Package:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<ResponseFishPackageDTO>> UpdatePackage(int id, UpdatePackageDTO package)
        {
            var res = new ServiceResponseFormat<ResponseFishPackageDTO>();
            try
            {
                // Fetch the existing package from the database
                var existingPackage = await _repo.GetByIdAsync(id);

                if (existingPackage == null)
                {
                    res.Success = false;
                    res.Message = "Package not found";
                    return res;
                }
                
                bool isUpdated = false;

                // Handle image upload (either local or from a link)
                var imageService = new CloudinaryService();
                string uploadedImageUrl = string.Empty;

                if (package.ImageUrl != null)
                {
                    // Image is a local file uploaded via a form
                    using (var stream = package.ImageUrl.OpenReadStream())
                    {
                        uploadedImageUrl = await imageService.UploadImageAsync(stream, package.ImageUrl.FileName.ToString());
                        if (!string.IsNullOrEmpty(uploadedImageUrl))
                        {
                            existingPackage.ImageUrl = uploadedImageUrl;
                            isUpdated = true;
                        }
                    }
                }

                // Check for changes in other fields and update if necessary
                if (!string.IsNullOrEmpty(package.Name) && package.Name != existingPackage.Name)
                {
                    existingPackage.Name = package.Name;
                    isUpdated = true;
                }

                if (!string.IsNullOrEmpty(package.Description) && package.Description != existingPackage.Description)
                {
                    existingPackage.Description = package.Description;
                    isUpdated = true;
                }

                if (package.TotalPrice.HasValue && package.TotalPrice != existingPackage.TotalPrice)
                {
                    existingPackage.TotalPrice = package.TotalPrice.Value;
                    isUpdated = true;
                }

                if (package.DailyFood.HasValue && package.DailyFood != existingPackage.DailyFood)
                {
                    existingPackage.DailyFood = package.DailyFood.Value;
                    isUpdated = true;
                }
                
                if (!string.IsNullOrEmpty(package.ProductStatus))
                {
                    var statusEnum = package.ProductStatus.ToUpper().Trim();
                    if (ProductStatusEnum.AVAILABLE.ToString().Equals(statusEnum) && existingPackage.ProductStatus != ProductStatusEnum.AVAILABLE)
                    {
                        existingPackage.ProductStatus = ProductStatusEnum.AVAILABLE;
                        isUpdated = true;
                    }
                    if (ProductStatusEnum.UNAVAILABLE.ToString().Equals(statusEnum) && existingPackage.ProductStatus != ProductStatusEnum.UNAVAILABLE)
                    {
                        existingPackage.ProductStatus = ProductStatusEnum.UNAVAILABLE;
                        isUpdated = true;
                    }
                }
                // If no changes were made, return a message indicating no update
                if (!isUpdated)
                {
                    res.Success = false;
                    res.Message = "No changes detected. Package was not updated.";
                    return res;
                }
                // Perform the update only if changes were made
                await _repo.UpdatePackage(id, existingPackage);

                var result = _mapper.Map<ResponseFishPackageDTO>(existingPackage);
                res.Success = true;
                res.Data = result;
                res.Message = "Package updated successfully";
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Failed to update Package: {ex.Message}";
                return res;
            }
        }
    }
}

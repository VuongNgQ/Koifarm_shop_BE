using AutoMapper;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.RequestDTO.UpdateReq.Entity;
using BusinessObject.Model.ResponseDTO;
using BusinessObject.Utils;
using DataAccess;
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
    public class CategoryService:ICategoryService
    {   
        private readonly ICategoryRepo _repo;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepo repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<ServiceResponseFormat<ResponseCategoryDTO>> CreateCategory(CreateCategoryDTO categoryDTO)
        {
            var res = new ServiceResponseFormat<ResponseCategoryDTO>();
            try
            {
                var list = await _repo.GetAllAsync();
                if (list.Any(x => x.Name.ToLower().Equals(categoryDTO.Name.ToLower().Trim())))
                {
                    res.Success = false;
                    res.Message = "Name exist";
                    return res;
                }
                var mapp = _mapper.Map<Category>(categoryDTO);
                await _repo.AddAsync(mapp);
                var result = _mapper.Map<ResponseCategoryDTO>(mapp);
                res.Success = true;
                res.Data=result;
                res.Message = "Category Created Successfully";
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create Category:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<bool>> DeleteCategory(int id)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var exist=await _repo.GetByIdAsync(id); 
                if(exist != null)
                {
                    _repo.Remove(exist);
                    res.Success = true;
                    res.Message = "Category Deleted Successfully";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "Category not found";
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

        public async Task<ServiceResponseFormat<PaginationModel<ResponseCategoryDTO>>> GetAll(int page, int pageSize, string? search, string sort)
        {
            var res = new ServiceResponseFormat<PaginationModel<ResponseCategoryDTO>>();
            try
            {
                var categories = await _repo.GetAllAsync();
                if (!string.IsNullOrEmpty(search))
                {
                    categories = categories.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
                }
                categories = sort.ToLower().Trim() switch
                {
                    "name" => categories.OrderBy(e => e.Name),
                    _ => categories.OrderBy(e => e.CategoryId)
                };
                var mapp = _mapper.Map<IEnumerable<ResponseCategoryDTO>>(categories);
                if (mapp.Any())
                {
                    var paginationModel = await Pagination.GetPaginationEnum(mapp, page, pageSize);
                    res.Success = true;
                    res.Message = "Get Category successfully";
                    res.Data = paginationModel;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Category";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to get Category:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<bool>> UpdateCategory(int id, UpdateCategoryDTO categoryDTO)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                // Fetch the existing package from the database
                var existCategory = await _repo.GetByIdAsync(id);

                if (existCategory == null)
                {
                    res.Success = false;
                    res.Message = "Category not found";
                    return res;
                }

                bool isUpdated = false;


                // Check for changes in other fields and update if necessary
                if (!string.IsNullOrEmpty(categoryDTO.Name) && categoryDTO.Name != existCategory.Name)
                {
                    existCategory.Name = categoryDTO.Name;
                    isUpdated = true;
                }
                if (!string.IsNullOrEmpty(categoryDTO.Description) && categoryDTO.Description != existCategory.Description)
                {
                    existCategory.Description = categoryDTO.Description;
                    isUpdated = true;
                }
                if (!string.IsNullOrEmpty(categoryDTO.OriginCountry) && categoryDTO.OriginCountry != existCategory.OriginCountry)
                {
                    existCategory.OriginCountry = categoryDTO.OriginCountry;
                    isUpdated = true;
                }
                // If no changes were made, return a message indicating no update
                if (!isUpdated)
                {
                    res.Success = false;
                    res.Message = "No changes detected. Category was not updated.";
                    return res;
                }

                // Perform the update only if changes were made
                _repo.Update(existCategory);

                var result = _mapper.Map<ResponseCategoryDTO>(existCategory);
                res.Success = true;
                res.Message = "Category updated successfully";
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Failed to update Category: {ex.Message}";
                return res;
            }
        }
    }
}

using AutoMapper;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using BusinessObject.Utils;
using DataAccess.Entity;
using DataAccess.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public class ProductStatusService:IProductStatusService
    {
        private readonly IProductStatusRepo _repo;
        private readonly IMapper _mapper;

        public ProductStatusService(IProductStatusRepo repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<ServiceResponseFormat<ResponseProductStatusDTO>> CreateProductStatus(CreateProductStatusDTO status)
        {
            var res = new ServiceResponseFormat<ResponseProductStatusDTO>();
            try
            {
                var map=_mapper.Map<ProductStatus>(status);
                await _repo.CreateProductStatus(map);
                var result=_mapper.Map<ResponseProductStatusDTO>(map);
                res.Success = true;
                res.Message = "Status created successfully";
                res.Data = result;
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create Status: {ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<bool>> DeleteProductStatus(int id)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var result=await _repo.DeleteProductStatus(id);
                if (result)
                {
                    res.Success = true;
                    res.Message = "Status Deleted Successfully";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Status found";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to delete status:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<bool>> DeleteStatusByName(string name)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var result = await _repo.DeleteStatusByName(name);
                if (result)
                {
                    res.Success = true;
                    res.Message = "Status Deleted Successfully";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Status found";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to Delete Status:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<PaginationModel<ResponseProductStatusDTO>>> GetProductStatuses(int page, int pageSize,
            string? search, string sort)
        {
            var res = new ServiceResponseFormat<PaginationModel<ResponseProductStatusDTO>>();
            try
            {
                var statuses = await _repo.GetProductStatuses();
                if (!string.IsNullOrEmpty(search))
                {
                    statuses = statuses.Where(s => s.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
                }
                statuses = sort.ToLower().Trim() switch
                {
                    "name" => statuses.OrderBy(s => s.Name),
                    _ => statuses.OrderBy(s => s.ProductStatusId)
                };
                var mapp = _mapper.Map<IEnumerable<ResponseProductStatusDTO>>(statuses);
                if (mapp.Any())
                {
                    var paginationModel = await Pagination.GetPaginationEnum(mapp, page, pageSize);
                    res.Success = true;
                    res.Message = "Get Status successfully";
                    res.Data = paginationModel;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Status";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to get Status:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<ResponseProductStatusDTO>> GetStatusById(int id)
        {
            var res = new ServiceResponseFormat<ResponseProductStatusDTO>();
            try
            {
                var result = await _repo.GetStatusById(id);
                if(result!=null)
                {
                    var mapp = _mapper.Map<ResponseProductStatusDTO>(result);
                    res.Success = true;
                    res.Message = "Get Status Successfully";
                    res.Data = mapp;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Status";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to get Status:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<ResponseProductStatusDTO>> GetStatusByName(string name)
        {
            var res = new ServiceResponseFormat<ResponseProductStatusDTO>();
            try
            {
                var result = await _repo.GetStatusByName(name);
                if (result != null)
                {
                    var mapp = _mapper.Map<ResponseProductStatusDTO>(result);
                    res.Success = true;
                    res.Message = "Get Status Successfully";
                    res.Data = mapp;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Status";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to get Status:{ex.Message}";
                return res;
            }
        }
    }
}

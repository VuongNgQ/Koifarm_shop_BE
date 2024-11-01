using AutoMapper;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
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
    public class ConsignmentTypeService : IConsignmentTypeService
    {
        private readonly IConsignmentTypeRepo _repo;
        private readonly IMapper _mapper;
        public ConsignmentTypeService(IConsignmentTypeRepo repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }
        public async Task<ServiceResponseFormat<ResponseConsignmentTypeDTO>> CreateType(CreateConsignmentTypeDTO typeDTO)
        {
            var res = new ServiceResponseFormat<ResponseConsignmentTypeDTO>();
            try
            {
                var statuses = await _repo.GetAllAsync();
                if(statuses.Any(u=>u.Name==typeDTO.Name))
                {
                    res.Success = false;
                    res.Message = "Name existed";
                    return res;
                }
                else
                {
                    var mapp = _mapper.Map<ConsignmentType>(typeDTO);
                    await _repo.AddAsync(mapp);
                    var result = _mapper.Map<ResponseConsignmentTypeDTO>(mapp);
                    res.Success = true;
                    res.Message = "Create Type Successfully";
                    res.Data= result;
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create Type:{ex.Message}";
                return res;
            }
        }
        public async Task<ServiceResponseFormat<bool>> DeleteType(int id)
        {
            var res= new ServiceResponseFormat<bool>();
            try
            {
                var exist = await _repo.GetByIdAsync(id);
                if (exist != null)
                {
                    _repo.Remove(exist);
                    res.Message = "Delete type successfully";
                    res.Success= true;
                    return res;
                }
                else
                {
                    res.Message = "No consignment type";
                    res.Success = false;
                    return res;
                }
            }
            catch(Exception ex)
            {
                res.Message = $"Fail to delete type:{ex.Message}";
                res.Success = false;
                return res;
            }
        }

        public async Task<ServiceResponseFormat<PaginationModel<ResponseConsignmentTypeDTO>>> GetTypes(int page = 1, int pageSize = 10, string search = "", string sort = "")
        {
            var res = new ServiceResponseFormat<PaginationModel<ResponseConsignmentTypeDTO>>();
            try
            {
                var statuses = await _repo.GetAllAsync();
                if (!string.IsNullOrEmpty(search))
                {
                    statuses = statuses.Where(s => s.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
                }
                statuses = sort.ToLower().Trim() switch
                {
                    "name" => statuses.OrderBy(s => s.Name),
                    _ => statuses.OrderBy(s => s.ConsignmentTypeId)
                };
                var mapp = _mapper.Map<IEnumerable<ResponseConsignmentTypeDTO>>(statuses);
                if (mapp.Any())
                {
                    var paginationModel = await Pagination.GetPaginationEnum(mapp, page, pageSize);
                    res.Success = true;
                    res.Message = "Get Types successfully";
                    res.Data = paginationModel;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Consignment Type";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to get Type:{ex.Message}";
                return res;
            }
        }
    }
}

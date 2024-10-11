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
    public class OrderStatusService:IOrderStatusService
    {
        private readonly IOrderStatusRepo _repo;
        private readonly IMapper _mapper;
        public OrderStatusService(IOrderStatusRepo repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<ServiceResponseFormat<ResponseOrderStatusDTO>> CreateOrderStatus(CreateOrderStatusDTO statusDTO)
        {
            var res = new ServiceResponseFormat<ResponseOrderStatusDTO>();
            try
            {
                var statuses = await _repo.GetAllAsync();
                if(statuses.Any(s=>s.Name==statusDTO.Name))
                {
                    res.Success = false;
                    res.Message = "Name existed";
                    return res;
                }
                else
                {
                    var mapp = _mapper.Map<OrderStatus>(statusDTO);
                    await _repo.AddAsync(mapp);
                    var result=_mapper.Map<ResponseOrderStatusDTO>(mapp);
                    res.Success = true;
                    res.Message = "Create Status Successfully";
                    res.Data=result;
                    return res;
                }
            }
            catch(Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create Status:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<bool>> DeleteOrderStatusById(int id)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var exist = await _repo.GetByIdAsync(id);
                if (exist==null)
                {
                    res.Success = false;
                    res.Message = "Name existed";
                    return res;
                }
                else
                {
                    _repo.Remove(exist);
                    res.Success = true;
                    res.Message = "Delete Status Successfully";   
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to delete Status:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<PaginationModel<ResponseOrderStatusDTO>>> GetAllOrderStatus(int page = 1, int pageSize = 10, string search = "", string sort = "")
        {
            var res= new ServiceResponseFormat<PaginationModel<ResponseOrderStatusDTO>>();
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
                    _ => statuses.OrderBy(s => s.OrderStatusId)
                };
                var mapp = _mapper.Map<IEnumerable<ResponseOrderStatusDTO>>(statuses);
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
            catch(Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to get Status:{ex.Message}";
                return res;
            }
        }
    }
}

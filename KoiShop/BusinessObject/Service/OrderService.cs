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
    public class OrderService:IOrderService
    {
        private readonly IOrderRepo _repo;
        private readonly IMapper _mapper;
        public OrderService(IOrderRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ServiceResponseFormat<ResponseOrderDTO>> CreateOrder(CreateOrderDTO orderDTO)
        {
            var res=new ServiceResponseFormat<ResponseOrderDTO>();
            try
            {
                var mapp = _mapper.Map<Order>(orderDTO);
                await _repo.AddAsync(mapp);
                var result = _mapper.Map<ResponseOrderDTO>(mapp);
                res.Success = true;
                res.Message = "Create Order Successfully";
                res.Data = result;
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create Order:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<bool>> DeleteOrder(int id)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var exist = await _repo.GetByIdAsync(id);
                if(exist != null)
                {
                    _repo.Remove(exist);
                    res.Success = true;
                    res.Message = "Delete Order Successfully";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Order";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to delete Order:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<ResponseOrderDTO>> GetOrderById(int id)
        {
            var res = new ServiceResponseFormat<ResponseOrderDTO>();
            try
            {
                var exist = await _repo.GetByIdAsync(id);
                if (exist != null)
                {
                    var result = _mapper.Map<ResponseOrderDTO>(exist);
                    res.Success = true;
                    res.Message = "Get Order Successfully";
                    res.Data =result;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Order";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to get Order:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<PaginationModel<ResponseOrderDTO>>> GetOrders(int page, int pageSize, string? search, string sort)
        {
            var res = new ServiceResponseFormat<PaginationModel<ResponseOrderDTO>>();
            try
            {
                var users = await _repo.GetAllAsync();
                /*if (!string.IsNullOrEmpty(search))
                {
                    users = users.Where(e => e.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    e.Email.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    e.Phone.Contains(search, StringComparison.OrdinalIgnoreCase));
                }*/
                users = sort.ToLower().Trim() switch
                {
                    "date" => users.OrderBy(e => e.OrderDate),
                    "totalprice" => users.OrderBy(e => e.TotalPrice),
                    _ => users.OrderBy(e => e.OrderId)
                };
                var mapp = _mapper.Map<IEnumerable<ResponseOrderDTO>>(users);
                if (mapp.Any())
                {
                    var paginationModel = await Pagination.GetPaginationEnum(mapp, page, pageSize);
                    res.Success = true;
                    res.Message = "Get Orders successfully";
                    res.Data = paginationModel;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Order";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to get User:{ex.Message}";
            }
            return res;
        }

        public Task<ServiceResponseFormat<ResponseOrderDTO>> UpdateOrder(ResponseOrderDTO orderDTO)
        {
            throw new NotImplementedException();
        }
    }
}

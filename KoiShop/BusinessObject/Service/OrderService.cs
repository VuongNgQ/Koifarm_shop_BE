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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _repo;
        private readonly IAddressRepo _addressRepo;
        private readonly IMapper _mapper;
        private readonly IUserAddressRepo _uaRepo;
        private readonly IUserRepo _userRepo;
        private readonly IOrderItemRepo _itemRepo;
        public OrderService(IOrderRepo repo, 
            IMapper mapper, IAddressRepo addressRepo, IUserAddressRepo uaRepo
            , IUserRepo userRepo, IOrderItemRepo itemRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _addressRepo = addressRepo;
            _uaRepo = uaRepo;
            _userRepo = userRepo;
            _itemRepo = itemRepo;
        }

        public async Task<ServiceResponseFormat<bool>> ChangeStatus(int id, string status)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var exist =await _repo.GetByIdAsync(id);
                if (exist == null)
                {
                    res.Success = false;
                    res.Message = "No Order found";
                    return res;
                }
                if (OrderStatusEnum.PENDING.Equals(status.ToUpper().Trim()))
                {
                    exist.Status = OrderStatusEnum.PENDING;
                }
                if (OrderStatusEnum.COMPLETED.Equals(status.ToUpper().Trim()))
                {
                    exist.Status = OrderStatusEnum.COMPLETED;
                }
                _repo.Update(exist);
                res.Success = true;
                res.Message = "Order Updated Successfully";
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to change Status:{ex.Message}";
                return res;
            }
        }
        public async Task<ServiceResponseFormat<ResponseOrderDTO>> CreateOrder(CreateOrderDTO orderDTO)
        {
            var res = new ServiceResponseFormat<ResponseOrderDTO>();
            try
            {
                var mapp = _mapper.Map<Order>(orderDTO);
                
                mapp.Status = OrderStatusEnum.PENDING;
                mapp.OrderDate = DateTime.Now;
                
                var addressMap=_mapper.Map<Address>(orderDTO.CreateAddressDTO);
                await _addressRepo.AddAsync(addressMap);
                var userExist = await _userRepo.GetByIdAsync(orderDTO.UserId);
                if(userExist == null)
                {
                    res.Success = false;
                    res.Message = "No User found";
                    return res;
                }
                UserAddress userAddress = new()
                {
                    UserId=orderDTO.UserId,
                    AddressId=addressMap.AddressId,
                };
                await _uaRepo.AddAsync(userAddress);
                await _repo.AddAsync(mapp);
                var userRes = _mapper.Map<ResponseUserDTO>(userExist);
                var addressResult = _mapper.Map<ResponseAddressDTO>(addressMap);
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
                if (exist != null)
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
                    res.Data = result;
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
                var users = await _repo.GetAllOrder();
                if (!string.IsNullOrEmpty(search))
                {
                    users = users.Where(e => e.Status.ToString().Contains(search, StringComparison.OrdinalIgnoreCase));
                }
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

        public async Task<ServiceResponseFormat<ResponseOrderDTO>> UpdateOrder(int id, UpdateOrderDTO orderDTO)
        {
            var res = new ServiceResponseFormat<ResponseOrderDTO>();
            try
            {
                var orders = await _repo.GetAllOrder();
                var exist = orders.FirstOrDefault(x=>x.OrderId==id&&x.Status == OrderStatusEnum.PENDING);
                if (exist != null)
                {
                    var mapp = _mapper.Map<Order>(orderDTO);
                    var addressMap = _mapper.Map<Address>(orderDTO.Address);
                    if (OrderStatusEnum.PENDING.Equals(orderDTO.Status.ToUpper().Trim()))
                    {
                        mapp.Status = OrderStatusEnum.PENDING;
                    }
                    if (OrderStatusEnum.COMPLETED.Equals(orderDTO.Status.ToUpper().Trim()))
                    {
                        mapp.Status = OrderStatusEnum.COMPLETED;
                    }
                    mapp.PaymentMethodId=orderDTO.PaymentMethodId;
                    mapp.IsSent=orderDTO.IsSent;
                    _addressRepo.Update(addressMap);
                    _repo.Update(exist);
                    var addRes=_mapper.Map<ResponseAddressDTO>(addressMap);
                    var result = _mapper.Map<ResponseOrderDTO>(exist);
                    res.Success = true;
                    res.Message = "Order Updated Successfully";
                    res.Data = result;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Order Found ";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to update Order:{ex.Message}";
                return res;
            }
        }
    }
}

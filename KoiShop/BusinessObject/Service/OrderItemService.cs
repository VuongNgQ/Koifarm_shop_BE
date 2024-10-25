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
    public class OrderItemService:IOrderItemService
    {
        private readonly IOrderItemRepo _repo;
        private readonly IMapper _mapper;
        private readonly IFishRepo _fishRepo;
        private readonly IFishPackageRepo _fishPackageRepo;
        private readonly IOrderRepo _orderRepo;
        private readonly ICartRepo _cartRepo;
        private readonly ICartItemRepo _cartItemRepo;
        private readonly ICartItemService _cartItemService;
        public OrderItemService(IOrderItemRepo repo, IMapper mapper, IFishRepo fishRepo, 
            IFishPackageRepo packageRepo, IOrderRepo orderRepo, 
            ICartRepo cartRepo, ICartItemRepo cartItemRepo,
            ICartItemService cartItemService)
        {
            _repo = repo;
            _mapper = mapper;
            _fishRepo = fishRepo;
            _fishPackageRepo = packageRepo;
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
            _cartItemRepo = cartItemRepo;
            _cartItemService = cartItemService;
        }

        public async Task<ServiceResponseFormat<ResponseOrderItemDTO>> CreateFishItem(CreateOrderItemDTO itemDTO)
        {
            var res = new ServiceResponseFormat<ResponseOrderItemDTO>();
            try
            {
                var orderExist = await _orderRepo.GetByIdAsync(itemDTO.OrderId);
                if (orderExist == null)
                {
                    res.Success = false;
                    res.Message = "No Order with this Id";
                    return res;
                }
                var cartExist = await _cartRepo.GetByIdAsync(itemDTO.UserCartId);
                if (cartExist == null)
                {
                    res.Success = false;
                    res.Message = "No Cart with this Id";
                    return res;
                }
                else
                {
                    var items = await _cartItemRepo.GetAllAsync();
                    var fishInCart = items.Where(i => i.UserCartId == itemDTO.UserCartId && i.FishId != null).ToList();
                    if (fishInCart.Count == 0)
                    {
                        res.Success = false;
                        res.Message = "No Fish in cart";
                        return res;
                    }
                    foreach (var item in fishInCart)
                    {
                        OrderItem newItem = new OrderItem()
                        {
                            OrderId = itemDTO.OrderId,
                            FishId = item.FishId,
                            Quantity = item.Quantity,
                            Price = item.Fish?.Price*item.Quantity
                        };
                        await _repo.AddAsync(newItem);
                        await _cartItemService.DeleteCartItemById(item.CartItemId);
                    }
                }
                res.Success = true;
                res.Message = "Create Item Successfully";
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create Item:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<ResponseOrderItemDTO>> CreatePackageItem(CreateOrderItemDTO itemDTO)
        {
            var res = new ServiceResponseFormat<ResponseOrderItemDTO>();
            try
            {
                var orderExist = await _orderRepo.GetByIdAsync(itemDTO.OrderId);
                if (orderExist == null)
                {
                    res.Success = false;
                    res.Message = "No Order with this Id";
                    return res;
                }
                var cartExist = await _cartRepo.GetByIdAsync(itemDTO.UserCartId);
                if(cartExist == null)
                {
                    res.Success = false;
                    res.Message = "No Cart with this Id";
                    return res;
                }
                else
                {
                    var items=await _cartItemRepo.GetAllAsync();
                    var packageInCart = items.Where(i => i.UserCartId == itemDTO.UserCartId&&i.PackageId!=null).ToList();
                    if (packageInCart.Count == 0)
                    {
                        res.Success = false;
                        res.Message = "No Package in cart";
                        return res;
                    }
                    foreach (var item in packageInCart)
                    {

                        OrderItem newItem = new OrderItem()
                        {
                            OrderId = itemDTO.OrderId,
                            PackageId = item.PackageId,
                            Quantity = item.Quantity,
                            Price = item.Package?.TotalPrice
                        };
                        await _repo.AddAsync(newItem);
                        await _cartItemService.DeleteCartItemById(item.CartItemId);
                    }
                }
                res.Success = true;
                res.Message = "Create Item Successfully";
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create Item:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<bool>> DeleteOrderItemById(int id)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var exist = await _repo.GetByIdAsync(id);
                if (exist == null)
                {
                    res.Success = false;
                    res.Message = "No Item";
                    return res;
                }
                else
                {
                    _repo.Remove(exist);
                    res.Success = true;
                    res.Message = "Delete Item Successfully";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to Delete Item:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<PaginationModel<ResponseOrderItemDTO>>> GetAllItem(int page, int pageSize, string sort)
        {
            var res = new ServiceResponseFormat<PaginationModel<ResponseOrderItemDTO>>();
            try
            {
                var items = await _repo.GetAllAsync();
                var mapp = _mapper.Map<IEnumerable<ResponseOrderItemDTO>>(items);
                if (mapp.Any())
                {
                    var paginationModel = await Pagination.GetPaginationEnum(mapp, page, pageSize);
                    res.Success = true;
                    res.Message = "Get Items successfully";
                    res.Data = paginationModel;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Item";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to Get Item:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<IEnumerable<ResponseOrderItemDTO>>> GetItemByOrderId(int id)
        {
            var res = new ServiceResponseFormat<IEnumerable<ResponseOrderItemDTO>>();
            try
            {
                var item = await _repo.GetAllAsync();
                var itemCart = item.Where(i => i.OrderId == id).ToList();
                var orderExist = await _orderRepo.GetByIdAsync(id);
                if (orderExist == null)
                {
                    res.Success = false;
                    res.Message = "No Order with this Id";
                    return res;
                }
                if (itemCart.Any())
                {
                    var mapp = _mapper.Map<IEnumerable<ResponseOrderItemDTO>>(itemCart);
                    res.Success = true;
                    res.Message = "Get Items successfully";
                    res.Data = mapp;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Item in this cart";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to Get Item:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<bool>> UpdateFishQuantity(int id, int quantity)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var exist = await _repo.GetByIdAsync(id);
                if (exist != null&&exist.FishId!=null&&exist.PackageId==null)
                {
                    exist.Quantity = quantity;
                    _repo.Update(exist);
                    res.Success = true;
                    res.Message = "Quantity Updated Successfully";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Fish Item Found";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to update quantity:{ex.Message}";
                return res;
            }
        }
    }
}

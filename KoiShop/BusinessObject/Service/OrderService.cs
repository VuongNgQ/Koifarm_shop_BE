﻿using AutoMapper;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.RequestDTO.UpdateReq.Entity;
using BusinessObject.Model.ResponseDTO;
using BusinessObject.Utils;
using DataAccess;
using DataAccess.Entity;
using DataAccess.Enum;
using DataAccess.IRepo;
using DataAccess.Repo;
using System;
using System.Collections;
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
        private readonly ICartRepo _cartRepo;
        private readonly ICartItemRepo _cartItemRepo;
        private readonly IOrderItemRepo _orderItemRepo;
        private readonly IFishPackageRepo _packageRepo;

        private readonly ICartItemService _cartItemService;
        private readonly IOrderItemService _orderItemService;
        private readonly IFishPackageService _packageService;
        private readonly IFishService _fishService;
        public OrderService(IOrderRepo repo, 
            IMapper mapper, IAddressRepo addressRepo, IUserAddressRepo uaRepo
            , IUserRepo userRepo, IOrderItemRepo itemRepo, ICartRepo cartRepo
            , ICartItemRepo cartItemRepo, IOrderItemRepo orderItemRepo, ICartItemService cartItemService
            , IOrderItemService orderItemService, IFishPackageService packageService, IFishService fishService
            , IFishPackageRepo packageRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _addressRepo = addressRepo;
            _uaRepo = uaRepo;
            _userRepo = userRepo;
            _itemRepo = itemRepo;
            _cartRepo = cartRepo;
            _cartItemRepo = cartItemRepo;
            _orderItemRepo = orderItemRepo;
            _cartItemService = cartItemService;
            _orderItemService = orderItemService;
            _packageService = packageService;
            _fishService = fishService;
            _packageRepo = packageRepo;
        }
        public async Task<ServiceResponseFormat<bool>> FinishOrder(int id)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var exist=await _repo.GetByIdAsync(id);
                if (exist == null)
                {
                    res.Success = false;
                    res.Message = "No Order found";
                    return res;
                }
                else if(exist.Status==OrderStatusEnum.READY|| exist.Status == OrderStatusEnum.PENDING)
                {
                    exist.Status = OrderStatusEnum.COMPLETED;
                    exist.CompleteDate=DateTime.Now;
                    _repo.Update(exist);
                    res.Success = true;
                    res.Message = "Order Updated Successfully";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "Order may have COMPLETED/CANCELED";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to Update Order:{ex.Message}";
                return res;
            }
        }
        public async Task<ServiceResponseFormat<bool>> CancelOrder(int id)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var orders = await _repo.GetAllOrder();
                var exist = orders.FirstOrDefault(o => o.OrderId == id);
                if (exist == null)
                {
                    res.Success = false;
                    res.Message = "No Order found";
                    return res;
                }
                else if (exist.Status == OrderStatusEnum.PENDING)
                {
                    exist.Status = OrderStatusEnum.CANCELLED;
                    exist.CompleteDate = DateTime.Now;
                    _repo.Update(exist);
                    if(exist.OrderItems.Count > 0)
                    {
                        foreach (var item in exist.OrderItems)
                        {
                            if (item.FishId != null)
                            {
                                await _fishService.RestoreFish((int)item.FishId);
                            }
                            if (item.PackageId != null)
                            {
                                await _packageService.RestorePackage((int)item.PackageId);
                            }
                        }
                    }
                    else
                    {
                        res.Success = false;
                        res.Message = "No Item In Order";
                        return res;
                    }
                    res.Success = true;
                    res.Message = "Order Updated Successfully";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "Order may have COMPLETED/CANCELED/ONPORT";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to Update Order:{ex.Message}";
                return res;
            }
        }
        public async Task<ServiceResponseFormat<bool>> ChangeStatus(int id, string status)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var orders = await _repo.GetAllOrder();
                var exist = orders.FirstOrDefault(o => o.OrderId == id);
                if (exist == null)
                {
                    res.Success = false;
                    res.Message = "No Order found";
                    return res;
                }
                if(exist.Status==OrderStatusEnum.COMPLETED||
                    exist.Status == OrderStatusEnum.CANCELLED)
                {
                    res.Success = false;
                    res.Message = "This Order is Done so you can't change it anymore";
                    return res;
                }
                if (OrderStatusEnum.CANCELLED.ToString().Equals(status.ToUpper().Trim()))
                {
                    //Update for Fish and Package 
                    if (exist.OrderItems.Count > 0)
                    {
                        foreach (var item in exist.OrderItems)
                        {
                            if (item.FishId != null)
                            {
                                await _fishService.RestoreFish((int)item.FishId);
                            }
                            var packageForOrder = await _packageRepo.GetFishPackage((int)item.PackageId);
                            int curQuantity = (int)packageForOrder.QuantityInStock;
                            int newQuantity = 0;
                            if (curQuantity == 0)
                            {
                                newQuantity = (int)(curQuantity + item.Quantity);
                                await _packageService.ChangeStatus((int)item.PackageId, ProductStatusEnum.AVAILABLE.ToString());
                            }
                            else
                            {
                                newQuantity = (int)(curQuantity + item.Quantity);
                            }
                            packageForOrder.QuantityInStock = newQuantity;
                            _packageRepo.Update(packageForOrder);
                        }
                    }
                    else
                    {
                        res.Success = false;
                        res.Message = "No Item In Order";
                        return res;
                    }
                    //Change status of items in cart
                    var cartList = await _cartRepo.GetAll();
                    var cart = cartList.FirstOrDefault(c => c.UserId == exist.UserId);
                    if (cart == null)
                    {
                        res.Success = false;
                        res.Message = "No Cart found for the user";
                        return res;
                    }
                    var listItem = await _cartItemRepo.GetAll();
                    var cartItems = listItem.Where(c => c.UserCartId == cart.UserCartId).ToList();
                    foreach (var cartItem in cartItems)
                    {
                        await _cartItemService.ChangeStatus(cartItem.CartItemId, CartItemStatus.CANCEL_AT_ORDER.ToString());
                    }
                    //Change Order status
                    exist.Status = OrderStatusEnum.CANCELLED;
                }
                else if (OrderStatusEnum.COMPLETED.ToString().Equals(status.ToUpper().Trim()))
                {
                    //Update Cart Items Status 
                    var cartList = await _cartRepo.GetAll();
                    var cart = cartList.FirstOrDefault(c => c.UserId == exist.UserId);
                    if (cart == null)
                    {
                        res.Success = false;
                        res.Message = "No Cart found for the user";
                        return res;
                    }
                    var listItem = await _cartItemRepo.GetAll();
                    var cartItems = listItem.Where(c => c.UserCartId == cart.UserCartId).ToList();
                    foreach (var cartItem in cartItems)
                    {
                        await _cartItemService.ChangeStatus(cartItem.CartItemId, CartItemStatus.COMPLETE_AT_ORDER.ToString());
                        if (cartItem.FishId != null|| cartItem.PackageId != null)
                        {
                            if(cartItem.FishId != null)
                            {
                                await _fishService.SoldoutFish((int)cartItem.FishId);
                            }
                            var packageOfCart = await _packageRepo.GetFishPackage((int)cartItem.PackageId);
                            if( packageOfCart!=null&&packageOfCart.QuantityInStock == 0)
                            {
                                await _packageService.SoldoutPackage((int)cartItem.PackageId);
                            }
                        }
                    }
                    //Update Order Status 
                    exist.Status = OrderStatusEnum.COMPLETED;
                }
                else if (OrderStatusEnum.READY.ToString().Equals(status.ToUpper().Trim()))
                {
                    var cartList = await _cartRepo.GetAll();
                    var cart = cartList.FirstOrDefault(c => c.UserId == exist.UserId);
                    if (cart == null)
                    {
                        res.Success = false;
                        res.Message = "No Cart found for the user";
                        return res;
                    }
                    var listItem = await _cartItemRepo.GetAll();
                    var cartItems = listItem.Where(c => c.UserCartId == cart.UserCartId).ToList();
                    foreach (var cartItem in cartItems)
                    {
                        await _cartItemService.ChangeStatus(cartItem.CartItemId, CartItemStatus.READY_FOR_ORDER.ToString());
                    }
                    exist.Status = OrderStatusEnum.READY;
                }
                else
                {
                    res.Success = false;
                    res.Message = "Invalid Status/Order is Pending";
                    return res;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderDTO"></param>
        /// <returns></returns>
        public async Task<ServiceResponseFormat<ResponseOrderDTO>> CreateOrderWithItems(CreateOrderDTO orderDTO)
        {
            var res = new ServiceResponseFormat<ResponseOrderDTO>();
            try
            {
                // 1. Create Order
                var mappedOrder = _mapper.Map<Order>(orderDTO);
                mappedOrder.Status = OrderStatusEnum.PENDING;
                mappedOrder.OrderDate = DateTime.Now;
                if (PaymentMethod.CASH.ToString().Equals(orderDTO.PaymentMethod.ToUpper().Trim()))
                {
                    mappedOrder.PaymentMethod = PaymentMethod.CASH;
                }
                else if (PaymentMethod.ZALOPAY.ToString().Equals(orderDTO.PaymentMethod.ToUpper().Trim()))
                {
                    mappedOrder.PaymentMethod = PaymentMethod.ZALOPAY;
                }
                var addressMap = _mapper.Map<Address>(orderDTO.CreateAddressDTO);
                await _addressRepo.AddAsync(addressMap);

                var user = await _userRepo.GetByIdAsync(orderDTO.UserId);
                if (user == null)
                {
                    res.Success = false;
                    res.Message = "No User found";
                    return res;
                }

                UserAddress userAddress = new UserAddress
                {
                    UserId = orderDTO.UserId,
                    AddressId = addressMap.AddressId,
                };
                await _uaRepo.AddAsync(userAddress);


                // 2. Retrieve User's Cart and Items
                var cartList = await _cartRepo.GetAll();
                var cart = cartList.FirstOrDefault(c=>c.UserId==orderDTO.UserId);
                if (cart == null)
                {
                    res.Success = false;
                    res.Message = "No Cart found for the user";
                    return res;
                }
                var listItem=await _cartItemRepo.GetAll();
                var cartItems = listItem.Where(c => c.UserCartId == cart.UserCartId).ToList();
                if (cartItems.Count == 0)
                {
                    res.Success = false;
                    res.Message = "No items in cart";
                    return res;
                }
                else
                {
                    await _repo.AddAsync(mappedOrder);
                    foreach (var cartItem in cartItems)
                    {
                        if (CartItemStatus.PENDING_FOR_ORDER.Equals(cartItem.CartItemStatus))
                        {
                            OrderItem newItem = new OrderItem
                            {
                                OrderId = mappedOrder.OrderId,
                                FishId = cartItem.FishId,
                                PackageId = cartItem.PackageId,
                                Quantity = cartItem.Quantity,
                                Price = cartItem.TotalPricePerItem
                            };
                            /*await _repo.AddAsync(mappedOrder);*/
                            await _orderItemRepo.AddAsync(newItem);
                            await _cartItemService.ChangeStatus(cartItem.CartItemId, CartItemStatus.ADDED_IN_ORDER.ToString());
                        }
                    }
                    // 4. Update Order Total Price
                    await _orderItemService.UpdateOrderTotalPrice(mappedOrder.OrderId);
                    var result = _mapper.Map<ResponseOrderDTO>(mappedOrder);
                    res.Success = true;
                    res.Message = "Order created with items successfully";
                    res.Data = result;
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Failed to create order with items: {ex.Message}";
                return res;
            }
        }
        public async Task<ServiceResponseFormat<bool>> DeleteOrder(int id)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                
                var exist = await _repo.GetByIdWithItemsAsync(id); 
                if (exist != null)
                {
                    if(exist.Status != OrderStatusEnum.CANCELLED)
                    {
                        res.Success = false;
                        res.Message = "You can't delete the COMPLETE/PENDING/READY Orders";
                        res.Data = false;
                    }
                    var addressOfOrder = await _addressRepo.GetByIdAsync((int)exist.AddressId);
                    if (exist.OrderItems != null && exist.OrderItems.Any())
                    {
                        _addressRepo.Remove(addressOfOrder);
                        _itemRepo.RemoveRange(exist.OrderItems);
                    }
                    _repo.Remove(exist);
                    
                    res.Success = true;
                    res.Message = "Order and associated items deleted successfully";
                    res.Data = true;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No order found with the given ID";
                    res.Data = false;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Failed to delete order: {ex.Message}";
                res.Data = false;
            }

            return res;
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
                res.Message = $"Fail to get Order:{ex.Message}";
            }
            return res;
        }

        public async Task<ServiceResponseFormat<ResponseOrderDTO>> UpdateOrder(int id, UpdateOrderDTO orderDTO)
        {
            var res = new ServiceResponseFormat<ResponseOrderDTO>();
            try
            {
                var orders = await _repo.GetAllOrder();
                var exist = orders.FirstOrDefault(x => x.OrderId == id && x.Status == OrderStatusEnum.PENDING);

                if (exist != null)
                {
                    bool isUpdated = false; // Flag to track if any updates are made

                    // Check and update status
                    if (!string.IsNullOrEmpty(orderDTO.Status) && orderDTO.Status != exist.Status.ToString())
                    {
                        if (Enum.TryParse(orderDTO.Status, true, out OrderStatusEnum newStatus))
                        {
                            exist.Status = newStatus;
                            isUpdated = true;
                        }
                    }

                    

                    // Check and update IsSent
                    if (orderDTO.IsSent != exist.IsSent)
                    {
                        exist.IsSent = orderDTO.IsSent;
                        isUpdated = true;
                    }

                    // Check and update address fields if Address is provided
                    if (orderDTO.Address != null)
                    {
                        if (!string.IsNullOrEmpty(orderDTO.Address.Street) && orderDTO.Address.Street != exist.Address.Street)
                        {
                            exist.Address.Street = orderDTO.Address.Street;
                            isUpdated = true;
                        }
                        if (!string.IsNullOrEmpty(orderDTO.Address.City) && orderDTO.Address.City != exist.Address.City)
                        {
                            exist.Address.City = orderDTO.Address.City;
                            isUpdated = true;
                        }
                        if (!string.IsNullOrEmpty(orderDTO.Address.District) && orderDTO.Address.District != exist.Address.District)
                        {
                            exist.Address.District = orderDTO.Address.District;
                            isUpdated = true;
                        }
                    }

                    // If no fields were updated, return a message
                    if (!isUpdated)
                    {
                        res.Success = false;
                        res.Message = "No fields were updated.";
                        return res;
                    }

                    // Save the changes to the database
                    _repo.Update(exist);
                    res.Success = true;
                    res.Message = "Order updated successfully.";
                    res.Data = _mapper.Map<ResponseOrderDTO>(exist);
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Order Found/Order Completed or Cancelled";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to update Order: {ex.Message}";
                return res;
            }
        }
    }
}

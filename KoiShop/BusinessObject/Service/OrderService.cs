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
        private readonly IFishRepo _fishRepo;

        private readonly ICartItemService _cartItemService;
        private readonly IOrderItemService _orderItemService;
        private readonly IFishPackageService _packageService;
        private readonly IFishService _fishService;
        private readonly IUserFishOwnerShipRepo _ownerShipRepo;
        private readonly IFishConsignmentRepo _consignmentRepo;
        public OrderService(IOrderRepo repo, 
            IMapper mapper, IAddressRepo addressRepo, IUserAddressRepo uaRepo
            , IUserRepo userRepo, IOrderItemRepo itemRepo, ICartRepo cartRepo
            , ICartItemRepo cartItemRepo, IOrderItemRepo orderItemRepo, ICartItemService cartItemService
            , IOrderItemService orderItemService, IFishPackageService packageService, IFishService fishService
            , IFishPackageRepo packageRepo, IUserFishOwnerShipRepo ownerShipRepo, IFishRepo fishRepo, IFishConsignmentRepo consignmentRepo)
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
            _packageRepo = packageRepo;
            _ownerShipRepo = ownerShipRepo;
            _fishRepo = fishRepo;

            _cartItemService = cartItemService;
            _orderItemService = orderItemService;
            _packageService = packageService;
            _fishService = fishService;
            _consignmentRepo = consignmentRepo;
        }
        #region Change status(da hell?)
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
        #endregion

        #region Change Status of the Order and update the thing inside (Fish, Package)
        public async Task<ServiceResponseFormat<bool>> ChangeStatus(int id, string status)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                // Fetch the order
                var orders = await _repo.GetAllOrder();
                var exist = orders.FirstOrDefault(o => o.OrderId == id);
                if (exist == null)
                {
                    res.Success = false;
                    res.Message = "No Order found";
                    return res;
                }

                // Validate the current status
                if (exist.Status == OrderStatusEnum.COMPLETED ||
                    exist.Status == OrderStatusEnum.CANCELLED )
                {
                    res.Success = false;
                    res.Message = "This Order is finalized and cannot be changed.";
                    return res;
                }

                // Fetch items in the order
                var items = await _orderItemRepo.GetAllAsync();
                var itemsInOrder=items.Where(o => o.OrderId == id).ToList();
                /*var itemsInOrder = await _orderItemService.GetItemByOrderId(id);*/
                if (itemsInOrder == null || !itemsInOrder.Any())
                {
                    res.Success = false;
                    res.Message = "No items in the order to process.";
                    return res;
                }

                // Handle cancellation or failed delivery
                if (OrderStatusEnum.CANCELLED.ToString().Equals(status.ToUpper().Trim()))
                {
                    foreach (var item in itemsInOrder)
                    {
                        // Restore Fish stock
                        if (item.FishId != null)
                        {
                            await _fishService.RestoreFish((int)item.FishId);
                        }

                        // Restore Package stock
                        if (item.PackageId != null)
                        {
                            var package = await _packageRepo.GetFishPackage((int)item.PackageId);
                            if (package != null)
                            {
                                package.QuantityInStock = (package.QuantityInStock ?? 0) + item.Quantity;
                                await _packageService.ChangeStatus(package.FishPackageId, ProductStatusEnum.AVAILABLE.ToString());
                                _packageRepo.Update(package);
                            }
                        }
                    }

                    // Update cart item statuses
                    await UpdateCartItemsStatus((int)exist.UserId, id, CartItemStatus.CANCEL_AT_ORDER);

                    // Update order status
                    exist.Status = OrderStatusEnum.CANCELLED;
                    exist.CompleteDate = DateTime.Now;
                }
                // Handle ready or on-port statuses
                else if (OrderStatusEnum.READY.ToString().Equals(status.ToUpper().Trim()))
                {
                    await UpdateCartItemsStatus((int)exist.UserId, id, CartItemStatus.READY_FOR_ORDER);

                    if (OrderStatusEnum.READY.ToString().Equals(status.ToUpper().Trim()))
                    {
                        if (exist.Status == OrderStatusEnum.PENDING)
                        {
                            exist.Status = OrderStatusEnum.READY;
                        }
                        else
                        {
                            res.Success = false;
                            res.Message = "Order must be in Pending status to set to Ready.";
                            return res;
                        }
                    }
                }
                // Handle completed or delivered statuses
                else if (OrderStatusEnum.COMPLETED.ToString().Equals(status.ToUpper().Trim()))
                {
                    foreach (var item in itemsInOrder)
                    {
                        // Restore Fish stock
                        if (item.FishId != null)
                        {
                            await _fishService.SoldoutFish((int)item.FishId);
                        }

                        // Restore Package stock
                        if (item.PackageId != null)
                        {
                            var package = await _packageRepo.GetFishPackage((int)item.PackageId);
                            if (package != null)
                            {
                                if (package.QuantityInStock == 0)
                                {
                                    await _packageService.SoldoutPackage(package.FishPackageId);
                                    _packageRepo.Update(package);
                                }
                            }
                        }
                    }
                    await UpdateCartItemsStatus((int)exist.UserId, id, CartItemStatus.COMPLETE_AT_ORDER);

                    if (OrderStatusEnum.COMPLETED.ToString().Equals(status.ToUpper().Trim()))
                    {
                            exist.Status = OrderStatusEnum.COMPLETED;
                            exist.CompleteDate = DateTime.Now;
                    }
                }
                else
                {
                    res.Success = false;
                    res.Message = "Invalid status.";
                    return res;
                }

                // Update the order
                _repo.Update(exist);
                res.Success = true;
                res.Message = "Order status updated successfully.";
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Failed to change status: {ex.Message}";
                return res;
            }
        }

        private async Task UpdateCartItemsStatus(int userId, int orderId, CartItemStatus status)
        {
            var cartList = await _cartRepo.GetAll();
            var cart = cartList.FirstOrDefault(c => c.UserId == userId);
            if (cart == null) return;

            var listItem = await _cartItemRepo.GetAll();
            var cartItems = listItem.Where(c => c.UserCartId == cart.UserCartId && c.OrderId == orderId).ToList();

            foreach (var cartItem in cartItems)
            {
                await _cartItemService.ChangeStatus(cartItem.CartItemId, status.ToString());
            }
        }

        #endregion

        #region Create Order and Items (with item at specific Status from the Cart)
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

                            //check if someone has taken the item 
                            if (cartItem.FishId != null)
                            {
                                var fishTaken = await _fishRepo.GetFishByIdAsync((int)cartItem.FishId);
                                if (fishTaken.ProductStatus != ProductStatusEnum.AVAILABLE)
                                {
                                    await _cartItemService.ChangeStatus(cartItem.CartItemId, CartItemStatus.TAKEN_BY_OTHERS.ToString());
                                    res.Success = false;
                                    res.Message = "That fish has been taken by other:)))))";
                                    return res;
                                }
                            }
                            else if (cartItem.PackageId != null)
                            {
                                var packageTaken = await _packageRepo.GetFishPackage((int)cartItem.PackageId);
                                var quantityPackageTaken = packageTaken.QuantityInStock;
                                if (packageTaken.ProductStatus != ProductStatusEnum.AVAILABLE)
                                {
                                    await _cartItemService.ChangeStatus(cartItem.CartItemId, CartItemStatus.TAKEN_BY_OTHERS.ToString());
                                    res.Success = false;
                                    res.Message = "That Package has been taken by other:)))))";
                                    return res;
                                }
                                if (quantityPackageTaken < cartItem.Quantity)
                                {
                                    res.Success = false;
                                    res.Message = $"Now it's only {quantityPackageTaken} package(s) left";
                                    return res;
                                }
                            }

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
                            cartItem.OrderId= mappedOrder.OrderId;
                            await _cartItemService.UpdateOrderIdForItem(cartItem.CartItemId, mappedOrder.OrderId);
                            await _cartItemService.ChangeStatus(cartItem.CartItemId, CartItemStatus.ADDED_IN_ORDER.ToString());
                            if (newItem.FishId != null)
                            {
                                await _fishService.ChangeStatus((int)newItem.FishId, ProductStatusEnum.PENDINGPAID.ToString());
                            }
                            if (newItem.PackageId!=null)
                            {
                                var packageForCart = await _packageRepo.GetFishPackage((int)newItem.PackageId);
                                int curQuantity = (int)packageForCart.QuantityInStock;
                                int newQuantity = curQuantity - (int)newItem.Quantity;
                                if (newQuantity < 0)
                                {
                                    res.Success = false;
                                    res.Message = "Exceed the package quantity??";
                                    return res;
                                }
                                else if (newQuantity == 0)
                                {
                                    packageForCart.QuantityInStock = newQuantity;
                                    _packageRepo.Update(packageForCart);
                                    await _packageService.ChangeStatus((int)newItem.PackageId, ProductStatusEnum.PENDINGPAID.ToString());
                                }
                                else
                                {
                                    packageForCart.QuantityInStock = newQuantity;
                                    _packageRepo.Update(packageForCart);
                                }
                            }
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
        #endregion

        #region Delete the order(only for Specific Order Status )
        public async Task<ServiceResponseFormat<bool>> DeleteOrder(int id)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                
                var exist = await _repo.GetByIdWithItemsAsync(id); 
                if (exist != null)
                {
                    if((exist.Status == OrderStatusEnum.PENDING||
                        exist.Status == OrderStatusEnum.READY)&&exist.OrderItems.Count>0)
                    {
                        res.Success = false;
                        res.Message = "You can't delete the PENDING/ONPORT/READY Orders";
                        res.Data = false;
                        return res;
                    }
                    var addressOfOrder = await _addressRepo.GetByIdAsync((int)exist.AddressId);
                    if (exist.OrderItems != null && exist.OrderItems.Any())
                    {
                        _addressRepo.Remove(addressOfOrder);
                        _itemRepo.RemoveRange(exist.OrderItems);
                    }
                    _repo.Remove(exist);
                    
                    res.Success = true;
                    res.Message = "Order deleted successfully";
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
        #endregion

        #region Get Order By Order ID
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
        #endregion

        #region Get all orders and also search, sort
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
        #endregion

        #region Update the order (not the items in order)
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
        #endregion

        public async Task MarkOrderAsCompleted(int orderId)//Minh
        {
            var order = await _repo.GetByIdWithItemsAsync(orderId);
            if (order == null || order.Status != OrderStatusEnum.READY)
            {
                throw new Exception("Order not found or already processed.");
            }
            foreach (var item in order.OrderItems)
            {
                if (item.FishId.HasValue)
                {
                    var consignment = await _consignmentRepo.GetConsignmentByFishIdAsync(item.FishId.Value);
                    if (consignment != null)
                    {
                        consignment.ConsignmentStatus = ConsignmentStatusEnum.Sold;
                        consignment.Earned = consignment.FinalPrice - (consignment.CommissionFee ?? 0) - (consignment.ServiceFee ?? 0);
                        await _consignmentRepo.UpdateFishConsignmentAsync(consignment);
                    }
                    var fish = await _fishRepo.GetFishByIdAsync(item.FishId.Value);
                    if (fish != null)
                    {
                        fish.ProductStatus = ProductStatusEnum.SOLDOUT;
                        await _fishRepo.UpdateFishAsync(fish);
                    }
                    var ownership = new UserFishOwnership
                    {
                        UserId = order.UserId.Value,
                        FishId = item.FishId.Value,
                        PurchaseDate = DateTime.Now
                    };
                    await _ownerShipRepo.AddAsync(ownership);
                }
            }
            await ChangeStatus(orderId, OrderStatusEnum.COMPLETED.ToString());
        }
        #region Get Order base on the User ID 
        public async Task<ServiceResponseFormat<IEnumerable<ResponseOrderDTO>>> GetOrdersByUserIdAsync(int userId)
        {
            var res = new ServiceResponseFormat<IEnumerable<ResponseOrderDTO>>();
            try
            {
                var orders = await _repo.GetAllOrder();
                var userOrders=orders.Where(o=>o.UserId==userId);
                if (!userOrders.Any())
                {
                    res.Success=false;
                    res.Message = $"No Order for this User Id: {userId}";
                    return res;
                }
                else
                {
                    var mapp=_mapper.Map<IEnumerable<ResponseOrderDTO>>(userOrders);
                    res.Success = true;
                    res.Message = $"Get Order successfully with this User ID: {userId}";
                    res.Data=mapp;
                    return res;
                }
            }
            catch(Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to get Order: {ex.Message}";
                return res;
            }
            
        }
        #endregion
    }
}

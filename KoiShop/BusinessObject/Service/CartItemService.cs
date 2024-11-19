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
    public class CartItemService:ICartItemService
    {
        private readonly ICartItemRepo _repo;
        private readonly IMapper _mapper;
        private readonly IFishRepo _fishRepo;
        private readonly IFishPackageRepo _fishPackageRepo;
        private readonly ICartRepo _cartRepo;

        private readonly IFishService _fishService;
        private readonly IFishPackageService _fishPackageService;
        public CartItemService(ICartItemRepo repo, IMapper mapper, IFishRepo fishRepo, IFishPackageRepo fishPackageRepo, ICartRepo cartRepo
            , IFishService fishService, IFishPackageService fishPackageService)
        {
            _mapper = mapper;
            _repo = repo;
            _fishRepo = fishRepo;
            _fishPackageRepo = fishPackageRepo;
            _cartRepo = cartRepo;
            _fishService = fishService;
            _fishPackageService = fishPackageService;
        }
        public async Task<ServiceResponseFormat<bool>> ChangeStatus(int id, string status)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var items=await _repo.GetAll();
                var exist = items.FirstOrDefault(i => i.CartItemId == id);
                if (exist == null)
                {
                    res.Success = false;
                    res.Message = "No Item found";
                    return res;
                }
                if (exist.CartItemStatus == CartItemStatus.CANCEL_AT_ORDER ||
                    exist.CartItemStatus == CartItemStatus.COMPLETE_AT_ORDER)
                {
                    res.Success = false;
                    res.Message = "This Item is Done at Order so you can't change it anymore";
                    return res;
                }
                if (CartItemStatus.CANCEL_AT_ORDER.ToString().Equals(status.ToUpper().Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    exist.CartItemStatus=CartItemStatus.CANCEL_AT_ORDER;
                }
                else if (CartItemStatus.COMPLETE_AT_ORDER.ToString().Equals(status.ToUpper().Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    exist.CartItemStatus = CartItemStatus.COMPLETE_AT_ORDER;
                }
                else if (CartItemStatus.READY_FOR_ORDER.ToString().Equals(status.ToUpper().Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    exist.CartItemStatus = CartItemStatus.READY_FOR_ORDER;
                }
                else if (CartItemStatus.ADDED_IN_ORDER.ToString().Equals(status.ToUpper().Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    exist.CartItemStatus = CartItemStatus.ADDED_IN_ORDER;
                }
                else if (CartItemStatus.TAKEN_BY_OTHERS.ToString().Equals(status.ToUpper().Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    exist.CartItemStatus = CartItemStatus.TAKEN_BY_OTHERS;
                }
                else
                {
                    res.Success = false;
                    res.Message = "Invalid Status/Order is Pending";
                    return res;
                }
                _repo.Update(exist);
                res.Success = true;
                res.Message = "Item Updated Successfully";
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to change Status:{ex.Message}";
                return res;
            }
        }
        public async Task<ServiceResponseFormat<ResponseCartItemDTO>> CreateFishItem(CreateFishItemDTO itemDTO)
        {
            var res = new ServiceResponseFormat<ResponseCartItemDTO>();
            try
            {
                var items=await _repo.GetAllAsync();
                if(items.Any(i=>i.FishId==itemDTO.FishId && i.UserCartId == itemDTO.UserCartId))
                {
                    res.Success = false;
                    res.Message = "You haved added this Fish before, now you can only update Quantity inside";
                    return res;
                }
                var exist = await _fishRepo.GetFishByIdAsync(itemDTO.FishId);
                if (exist==null)
                {
                    res.Success = false;
                    res.Message = "No fish with this Id";
                    return res;
                }
                if (!ProductStatusEnum.AVAILABLE.Equals(exist.ProductStatus))
                {
                    res.Success = false;
                    res.Message = "This fish is SOLD OUT/UNAVAILABLE/PENDINGPAID";
                    return res;
                }
                var cartExist = await _cartRepo.GetByIdAsync(itemDTO.UserCartId);
                if (cartExist == null)
                {
                    res.Success = false;
                    res.Message = "No Cart with this Id";
                    return res;
                }
                var mapp = _mapper.Map<CartItem>(itemDTO);
                mapp.Quantity = 1;
                mapp.TotalPricePerItem = exist.Price;
                mapp.CartItemStatus = CartItemStatus.PENDING_FOR_ORDER;
                await _repo.AddAsync(mapp);
                var result = _mapper.Map<ResponseCartItemDTO>(mapp);
                result.TotalPricePerItem=exist.Price;
                res.Success = true;
                res.Message = "Create Item Successfully";
                res.Data=result;
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create Item:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<ResponseCartItemDTO>> CreatePackageItem(CreatePackageItemDTO itemDTO)
        {
            var res = new ServiceResponseFormat<ResponseCartItemDTO>();
            try
            {
                var items = await _repo.GetAllAsync();
                if (items.Any(i => i.PackageId == itemDTO.PackageId&&i.UserCartId==itemDTO.UserCartId&&
                (i.CartItemStatus==CartItemStatus.PENDING_FOR_ORDER||i.CartItemStatus==CartItemStatus.ADDED_IN_ORDER)))
                {
                    res.Success = false;
                    res.Message = "You haved added this Package before";
                    return res;
                }
                var exist = await _fishPackageRepo.GetFishPackage(itemDTO.PackageId);
                if (exist == null)
                {
                    res.Success = false;
                    res.Message = "No Package with this Id";
                    return res;
                }
                if(exist.CategoryPackages.Count<=0)
                {
                    res.Success = false;
                    res.Message = "Package Has no fish?";
                    return res;
                }
                if (!ProductStatusEnum.AVAILABLE.Equals(exist.ProductStatus))
                {
                    res.Success = false;
                    res.Message = "This Package is Not AVAILABLE";
                    return res;
                }
                if (exist.QuantityInStock < itemDTO.Quantity)
                {
                    res.Success = false;
                    res.Message = "DON'T ADD WITH QUANTITY MORE THAN STOCK!!!";
                    return res;
                }
                var cartExist = await _cartRepo.GetByIdAsync(itemDTO.UserCartId);
                if (cartExist == null)
                {
                    res.Success = false;
                    res.Message = "No Cart with this Id";
                    return res;
                }
                var mapp = _mapper.Map<CartItem>(itemDTO);

                mapp.TotalPricePerItem = exist.TotalPrice*itemDTO.Quantity;

                var packageForCart = await _fishPackageRepo.GetFishPackage(itemDTO.PackageId);
                int curQuantity = (int)packageForCart.QuantityInStock;
                int newQuantity = curQuantity - itemDTO.Quantity;
                /*if (newQuantity < 0)
                {
                    res.Success = false;
                    res.Message = "Exceed the package quantity??";
                    return res;
                }
                else if (newQuantity == 0)
                {
                    packageForCart.QuantityInStock = newQuantity;
                    _fishPackageRepo.Update(packageForCart);
                    await _fishPackageService.ChangeStatus(itemDTO.PackageId, ProductStatusEnum.INCART.ToString());
                }
                else
                {
                    packageForCart.QuantityInStock = newQuantity;
                    _fishPackageRepo.Update(packageForCart);
                }*/
                if (newQuantity < 0)
                {
                    res.Success = false;
                    res.Message = "Exceed the package quantity??";
                    return res;
                }
                mapp.CartItemStatus=CartItemStatus.PENDING_FOR_ORDER;
                await _repo.AddAsync(mapp);
                var result = _mapper.Map<ResponseCartItemDTO>(mapp);
                res.Success = true;
                res.Message = "Create Item Successfully";
                res.Data = result;
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create Item:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<bool>> DeleteCartItemById(int id)
        {
            var res= new ServiceResponseFormat<bool>();
            try
            {
                var exist = await _repo.GetByIdAsync(id);
                if(exist == null)
                {
                    res.Success = false;
                    res.Message = "No Item";
                    return res;
                }
                else if (exist.CartItemStatus!=CartItemStatus.PENDING_FOR_ORDER)
                {
                    if (exist.CartItemStatus == CartItemStatus.COMPLETE_AT_ORDER ||
                    exist.CartItemStatus == CartItemStatus.CANCEL_AT_ORDER)
                    {
                        _repo.Remove(exist);
                        res.Success = true;
                        res.Message = "Delete Item Successfully";
                        return res;
                    }
                    else
                    {
                        res.Success = false;
                        res.Message = "This item is not PENDING/COMPLETE/CANCEL so you can't delete it";
                        return res;
                    }
                }
                else
                {
                    if (exist.FishId != null)
                    {
                        await _fishService.RestoreFish((int)exist.FishId);
                        _repo.Remove(exist);
                    }
                    else
                    {
                        var packageForCart = await _fishPackageRepo.GetFishPackage((int)exist.PackageId);
                        int curQuantity = (int)packageForCart.QuantityInStock;
                        int newQuantity = 0;
                        if (curQuantity == 0)
                        {
                            newQuantity = (int)(curQuantity + exist.Quantity);
                            await _fishPackageService.ChangeStatus((int)exist.PackageId, ProductStatusEnum.AVAILABLE.ToString());
                        }
                        else
                        {
                            newQuantity = (int)(curQuantity + exist.Quantity);
                        }
                        packageForCart.QuantityInStock = newQuantity;
                        _fishPackageRepo.Update(packageForCart);
                        _repo.Remove(exist);
                    }
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

        public async Task<ServiceResponseFormat<PaginationModel<ResponseCartItemDTO>>> GetAllItem(int page, int pageSize, string search, string sort)
        {
            var res = new ServiceResponseFormat<PaginationModel<ResponseCartItemDTO>>();
            try
            {
                var items =await _repo.GetAllAsync();
                /*if (!string.IsNullOrEmpty(search))
                {
                    items = items.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
                }*/
                /*items = sort.ToLower().Trim() switch
                {
                    "name" => packages.OrderBy(e => e.Name),

                    "fishinpackage" => packages.OrderBy(e => e.NumberOfFish),
                    "age" => packages.OrderBy(e => e.Age),
                    "price" => packages.OrderBy(e => e.TotalPrice),
                    _ => packages.OrderBy(e => e.FishPackageId)
                };*/
                var mapp=_mapper.Map<IEnumerable<ResponseCartItemDTO> >(items);
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

        public async Task<ServiceResponseFormat<IEnumerable<ResponseCartItemDTO>>> GetItemByCartId(int id)
        {
            var res=new ServiceResponseFormat<IEnumerable<ResponseCartItemDTO>>();
            try
            {
                var item=await _repo.GetAll();
                var itemCart = item.Where(i=>i.UserCartId==id).ToList();
                var cartExist = await _cartRepo.GetByIdAsync(id);
                if (cartExist == null)
                {
                    res.Success = false;
                    res.Message = "No Cart with this Id";
                    return res;
                }
                if (itemCart.Count != 0)
                {
                    var mapp = _mapper.Map<IEnumerable<ResponseCartItemDTO>>(itemCart);
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
            catch(Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to Get Item:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<bool>> UpdatePackageQuantity(int id, int? quantity)
        {
            var res = new ServiceResponseFormat<bool>();
            bool isUpdated = false;

            try
            {
                var exist = await _repo.GetByIdAsync(id);
                int quantityInCart = exist.Quantity ?? 0;
                int quantityDTO = quantity ?? 0;
                if (exist != null)
                {
                    if (exist.PackageId == null)
                    {
                        res.Success = false;
                        res.Message = "You can only update the quantity for Package items.";
                        return res;
                    }

                    // Check and update the quantity if provided
                    if (quantity.HasValue && quantity.Value != exist.Quantity)
                    {
                        var package=await _fishPackageRepo.GetFishPackage((int)exist.PackageId);
                        exist.Quantity = quantity.Value;
                        exist.TotalPricePerItem = quantity.Value * (package.TotalPrice ?? 0);
                        var packageForCart = await _fishPackageRepo.GetFishPackage((int)exist.PackageId);
                        int curQuantity = (int)packageForCart.QuantityInStock;
                        int newQuantity =0;
                        
                        if (quantityInCart > quantityDTO)
                        {
                            newQuantity = curQuantity + (quantityInCart-quantityDTO);
                            
                            if (curQuantity == 0)
                            {
                                packageForCart.QuantityInStock = newQuantity;
                                _fishPackageRepo.Update(packageForCart);
                                await _fishPackageService.ChangeStatus((int)exist.PackageId, ProductStatusEnum.AVAILABLE.ToString());
                            }
                            else
                            {
                                packageForCart.QuantityInStock = newQuantity;
                                _fishPackageRepo.Update(packageForCart);
                            }
                            isUpdated = true;
                        }
                        if(quantityInCart < quantityDTO)
                        {
                            newQuantity = curQuantity - (quantityDTO-quantityInCart);
                            if (newQuantity < 0)
                            {
                                res.Success = false;
                                res.Message = "Exceed the package quantity??";
                                return res;
                            }
                            /*else if (newQuantity == 0)
                            {
                                packageForCart.QuantityInStock = newQuantity;
                                _fishPackageRepo.Update(packageForCart);
                            }*/
                            else
                            {
                                packageForCart.QuantityInStock = newQuantity;
                                _fishPackageRepo.Update(packageForCart);
                            }
                            isUpdated = true;
                        }
                    }

                    // If nothing was updated, return a message
                    if (!isUpdated)
                    {
                        res.Success = false;
                        res.Message = "No fields were updated.";
                        return res;
                    }

                    // Save the changes if any updates were applied
                    _repo.Update(exist);
                    res.Success = true;
                    res.Message = "Quantity updated successfully.";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No item found with the provided ID.";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Failed to update quantity: {ex.Message}";
                return res;
            }
        }


    }
}

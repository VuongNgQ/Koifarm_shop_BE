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
    public class CartItemService:ICartItemService
    {
        private readonly ICartItemRepo _repo;
        private readonly IMapper _mapper;
        private readonly IFishRepo _fishRepo;
        private readonly IFishPackageRepo _fishPackageRepo;
        private readonly ICartRepo _cartRepo;
        public CartItemService(ICartItemRepo repo, IMapper mapper, IFishRepo fishRepo, IFishPackageRepo fishPackageRepo, ICartRepo cartRepo)
        {
            _mapper = mapper;
            _repo = repo;
            _fishRepo = fishRepo;
            _fishPackageRepo = fishPackageRepo;
            _cartRepo = cartRepo;
        }

        public async Task<ServiceResponseFormat<ResponseCartItemDTO>> CreateFishItem(CreateFishItemDTO itemDTO)
        {
            var res = new ServiceResponseFormat<ResponseCartItemDTO>();
            try
            {
                var items=await _repo.GetAllAsync();
                if(items.Any(i=>i.FishId==itemDTO.FishId))
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
                var cartExist = await _cartRepo.GetByIdAsync(itemDTO.UserCartId);
                if (cartExist == null)
                {
                    res.Success = false;
                    res.Message = "No Cart with this Id";
                    return res;
                }
                var mapp = _mapper.Map<CartItem>(itemDTO);
                mapp.TotalPricePerItem = exist.Price*itemDTO.Quantity;
                await _repo.AddAsync(mapp);
                var result = _mapper.Map<ResponseCartItemDTO>(mapp);
                result.TotalPricePerItem=exist.Price*itemDTO.Quantity;
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
                if (items.Any(i => i.PackageId == itemDTO.PackageId))
                {
                    res.Success = false;
                    res.Message = "You haved added this Package before";
                    return res;
                }
                var exist = await _fishPackageRepo.GetByIdAsync(itemDTO.PackageId);
                if (exist == null)
                {
                    res.Success = false;
                    res.Message = "No Package with this Id";
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
                mapp.TotalPricePerItem = exist.TotalPrice;
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

        public async Task<ServiceResponseFormat<bool>> UpdateFishQuantity(int id,int quantity)
        {
            var res=new ServiceResponseFormat<bool>();
            try
            {
                var exist = await _repo.GetByIdAsync(id);
                if (exist != null)
                {
                    if (exist.FishId == null)
                    {
                        res.Success = false;
                        res.Message = "You can get only 1 package";
                        return res;
                    }
                    exist.Quantity = quantity;
                    exist.TotalPricePerItem = quantity * exist.Fish?.Price;
                    _repo.Update(exist);
                    res.Success = true;
                    res.Message = "Quantity Updated Successfully";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Item Found";
                    return res;
                }
            }
            catch(Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to update quantity:{ex.Message}";
                return res;
            }
        }
    }
}

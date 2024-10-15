﻿using AutoMapper;
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
        public OrderItemService(IOrderItemRepo repo, IMapper mapper, IFishRepo fishRepo, IFishPackageRepo packageRepo, IOrderRepo orderRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _fishRepo = fishRepo;
            _fishPackageRepo = packageRepo;
            _orderRepo = orderRepo;
        }

        public async Task<ServiceResponseFormat<ResponseOrderItemDTO>> CreateFishItem(CreateFishItemDTO itemDTO)
        {
            var res = new ServiceResponseFormat<ResponseOrderItemDTO>();
            try
            {
                var items = await _repo.GetAllAsync();
                if (items.Any(i => i.FishId == itemDTO.FishId))
                {
                    res.Success = false;
                    res.Message = "You haved added this Fish before, now you can only update Quantity inside";
                    return res;
                }
                var exist = await _fishRepo.GetFishByIdAsync(itemDTO.FishId);
                if (exist == null)
                {
                    res.Success = false;
                    res.Message = "No fish with this Id";
                    return res;
                }
                var mapp = _mapper.Map<OrderItem>(itemDTO);
                await _repo.AddAsync(mapp);
                mapp.Price = exist.Price * itemDTO.Quantity;
                var result = _mapper.Map<ResponseOrderItemDTO>(mapp);
                
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

        public async Task<ServiceResponseFormat<ResponseOrderItemDTO>> CreatePackageItem(CreatePackageItemDTO itemDTO)
        {
            var res = new ServiceResponseFormat<ResponseOrderItemDTO>();
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
                var mapp = _mapper.Map<OrderItem>(itemDTO);
                await _repo.AddAsync(mapp);
                mapp.Price = exist.TotalPrice;
                var result = _mapper.Map<ResponseOrderItemDTO>(mapp);
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

        public async Task<ServiceResponseFormat<PaginationModel<ResponseOrderItemDTO>>> GetAllItem(int page, int pageSize, string search, string sort)
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
                if (itemCart != null)
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
                if (exist != null)
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
                    res.Message = "No Item Found";
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
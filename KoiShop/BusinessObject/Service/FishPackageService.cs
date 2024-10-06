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
    public class FishPackageService:IFishPackageService
    {
        private readonly IFishPackageRepo _repo;
        private readonly IMapper _mapper;
        public FishPackageService(IFishPackageRepo repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<ServiceResponseFormat<ResponseFishPackageDTO>> CreatePackage(CreateFishPackageDTO package)
        {
            var res = new ServiceResponseFormat<ResponseFishPackageDTO>();
            try
            {

                var mapp=_mapper.Map<FishPackage>(package);
                mapp.StatusId = 1;
                await _repo.CreatePackage(mapp);
                var result=_mapper.Map<ResponseFishPackageDTO>(mapp);
                res.Success = true;
                res.Message = "Package created successfully";
                res.Data = result;
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create User:{ex.Message}";
                return res;
            }
            
        }

        public async Task<ServiceResponseFormat<bool>> DeletePackage(int id)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var result = await _repo.DeletePackage(id);
                if (result)
                {
                    res.Success = true;
                    res.Message = "Package Deleted successfully";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Package";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to Delete Package";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<ResponseFishPackageDTO>> GetFishPackage(int id)
        {
            var res = new ServiceResponseFormat<ResponseFishPackageDTO>();
            try
            {
                var result=await _repo.GetFishPackage(id);
                if (result != null)
                {
                    var mapp = _mapper.Map<ResponseFishPackageDTO>(result);
                    res.Success = true;
                    res.Message = "Get Package successfully";
                    res.Data= mapp; 
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No package";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to Get Package:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<PaginationModel<ResponseFishPackageDTO>>> GetFishPackages(int page, int pageSize,
            string? search, string sort)
        {
            var res = new ServiceResponseFormat<PaginationModel<ResponseFishPackageDTO>>();
            try
            {
                var packages=await _repo.GetFishPackages();
                if (!string.IsNullOrEmpty(search))
                {
                    packages = packages.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
                }
                packages = sort.ToLower().Trim() switch
                {
                    "name" => packages.OrderBy(e => e.Name),
                    "remaining" => packages.OrderBy(e => e.RemainingPackage),
                    "fishinpackage" => packages.OrderBy(e => e.NumberOfFish),
                    "age" => packages.OrderBy(e => e.Age),
                    "price" => packages.OrderBy(e => e.TotalPrice),
                    _=>packages.OrderBy(e=>e.FishPackageId)
                };
                var mapp = _mapper.Map<IEnumerable<ResponseFishPackageDTO>>(packages);
                if (mapp.Any())
                {
                    var paginationModel = await Pagination.GetPaginationEnum(mapp, page, pageSize);
                    res.Success = true;
                    res.Message = "Get Package successfully";
                    res.Data = paginationModel;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Package";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to get Package:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<ResponseFishPackageDTO>> UpdatePackage(int id, ResponseFishPackageDTO fishPackage)
        {
            var res = new ServiceResponseFormat<ResponseFishPackageDTO>();
            try
            {
                var mapp = _mapper.Map<FishPackage>(fishPackage);
                var update = await _repo.UpdatePackage(id, mapp);
                if (update != null)
                {
                    res.Success = true;
                    res.Message = "Package Updated Successfully";
                    res.Data = fishPackage;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Package Found or got error at Repo";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to update Package:{ex.Message}";
                return res;
            }
        }
    }
}
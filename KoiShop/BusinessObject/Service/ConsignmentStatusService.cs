using AutoMapper;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using DataAccess.Entity;
using DataAccess.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public class ConsignmentStatusService:IConsignmentStatusService
    {
        private readonly IConsignmentStatusRepo _repo;
        private readonly IMapper _mapper;
        public ConsignmentStatusService(IConsignmentStatusRepo repo
            , IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ServiceResponseFormat<ResponseConsignmentStatusDTO>> CreateStatus(CreateConsignmentStatusDTO statusDTO)
        {
            var res = new ServiceResponseFormat<ResponseConsignmentStatusDTO>();
            try
            {
                var statuses = await _repo.GetAllAsync();
                if (statuses.Any(s => s.Name == statusDTO.Name))
                {
                    res.Success = true;
                    res.Message = "Name existed";
                    return res;
                }
                var map = _mapper.Map<ConsignmentStatus>(statusDTO);
                await _repo.AddAsync(map);
                var result = _mapper.Map<ResponseConsignmentStatusDTO>(map);
                res.Success = true;
                res.Message = "Status created successfully";
                res.Data = result;
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create Status: {ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<bool>> DeleteById(int id)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var result = await _repo.GetByIdAsync(id);
                if (result!=null)
                {
                    _repo.Remove(result);
                    res.Success = true;
                    res.Message = "Status Deleted Successfully";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Status found";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to delete status:{ex.Message}";
                return res;
            }
        }

        public Task<ServiceResponseFormat<bool>> DeleteByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponseFormat<PaginationModel<ResponseConsignmentStatusDTO>>> GetAll(int page, int pageSize, string? search, string sort)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponseFormat<ResponseConsignmentStatusDTO>> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }

        
}


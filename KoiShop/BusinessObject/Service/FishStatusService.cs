using AutoMapper;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.RequestDTO.UpdateReq.Entity;
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
    public class FishStatusService:IFishStatusService
    {
        private readonly IFishStatusRepo _repo;
        private readonly IMapper _mapper;
        public FishStatusService(IFishStatusRepo repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<ServiceResponseFormat<ResponseFishStatusDTO>> CreateStatus(CreateFishStatusDTO statusDTO)
        {
            var res = new ServiceResponseFormat<ResponseFishStatusDTO>();
            try
            {
                var statuses = await _repo.GetAllAsync();
                if(statuses.Any(s=>s.StatusName==statusDTO.StatusName))
                {
                    res.Success = false;
                    res.Message = "Status Exist";
                    return res;
                }
                var mapp = _mapper.Map<FishStatus>(statusDTO);
                await _repo.AddAsync(mapp);
                var result = _mapper.Map<ResponseFishStatusDTO>(mapp);
                res.Success = true;
                res.Message = "Create Status Successfully";
                res.Data = result;
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create Status:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<bool>> DeleteStatus(int id)
        {
            var res= new ServiceResponseFormat<bool>();
            try
            {
                var exist=await _repo.GetByIdAsync(id);
                if (exist != null)
                {
                    _repo.Remove(exist);
                    res.Success = true;
                    res.Message = "delete Status Successfully";
                    return res;
                }
                else
                {
                    res.Success = true;
                    res.Message = "No Status with this Id";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to delete Status:{ex.Message}";
                return res;
            }
        }

        public Task<ServiceResponseFormat<IEnumerable<ResponseFishStatusDTO>>> GetAllStatus()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponseFormat<ResponseFishStatusDTO>> UpdateStatus(UpdateFishStatusDTO statusDTO)
        {
            throw new NotImplementedException();
        }
    }
}

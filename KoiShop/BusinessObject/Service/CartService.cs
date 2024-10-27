using AutoMapper;
using BusinessObject.IService;
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
    public class CartService:ICartService
    {
        private readonly ICartRepo _repo;
        private readonly IMapper _mapper;
        public CartService(ICartRepo repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<ServiceResponseFormat<ResponseCartDTO>> GetCartByUserId(int id)
        {
            var res = new ServiceResponseFormat<ResponseCartDTO>();
            try
            {
                var list=await _repo.GetAll();
                var exist=list.FirstOrDefault(c => c.UserId == id);
                if (exist!=null)
                {
                    var result=_mapper.Map<ResponseCartDTO>(exist);
                    res.Success = true;
                    res.Message = "Get Cart Successfully";
                    res.Data = result;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Cart for this user";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to get Cart:{ex.Message}";
                return res;
            }
        }
    }
}

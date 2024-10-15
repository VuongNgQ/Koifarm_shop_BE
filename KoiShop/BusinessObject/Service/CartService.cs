using AutoMapper;
using BusinessObject.IService;
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

    }
}

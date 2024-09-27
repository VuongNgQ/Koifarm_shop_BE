using AutoMapper;
using BusinessObject.IService;
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

        public Task<ServiceResponseFormat<FishPackage>> CreatePackage(FishPackage package)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponseFormat<bool>> DeletePackage(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponseFormat<FishPackage>> GetFishPackage(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponseFormat<IEnumerable<FishPackage>>> GetFishPackages()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponseFormat<FishPackage>> UpdatePackage(int id, FishPackage fishPackage)
        {
            throw new NotImplementedException();
        }
    }
}

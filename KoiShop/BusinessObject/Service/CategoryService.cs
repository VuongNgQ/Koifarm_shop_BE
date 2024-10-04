using BusinessObject.IService;
using DataAccess;
using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public class CategoryService:ICategoryService
    {
        private readonly IBaseRepo<Category> _repo;
        public CategoryService(IBaseRepo<Category> repo)
        {
            _repo = repo;
        }

    }
}

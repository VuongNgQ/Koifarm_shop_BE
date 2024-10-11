using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepo
{
    public interface IProductStatusRepo:IBaseRepo<ProductStatus>
    {
        Task<ProductStatus> CreateProductStatus(ProductStatus status);
        Task<IEnumerable<ProductStatus>> GetProductStatuses();
        Task<ProductStatus> GetStatusByName(string name);
        Task<ProductStatus> GetStatusById(int id);
        Task<bool> DeleteProductStatus(int id);
        Task<bool> DeleteStatusByName(string name);
    }
}

using BusinessObject.Model.ResponseDTO;
using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface ICartService
    {
        Task<ServiceResponseFormat<ResponseCartDTO>> GetCartByUserId(int id);
    }
}

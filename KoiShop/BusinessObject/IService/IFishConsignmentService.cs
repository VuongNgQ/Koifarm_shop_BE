using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface IFishConsignmentService
    {
        Task<ServiceResponseFormat<FishConsignmentDTO>> CreateConsignment(CreateConsignmentDTO consignmentDto);
        Task<ServiceResponseFormat<FishConsignmentDTO>> GetConsignmentById(int consignmentId);
        Task<ServiceResponseFormat<List<FishConsignmentDTO>>> GetAllConsignments();
        Task<ServiceResponseFormat<FishConsignmentDTO>> UpdateConsignmentAsync(FishConsignmentDTO consignmentDto);
    }
}

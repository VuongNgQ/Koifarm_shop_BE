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
        Task<ServiceResponseFormat<FishConsignmentDTO>> CreateConsignmentAsync(CreateConsignmentDTO consignmentDto);
        Task<ServiceResponseFormat<bool>> ApproveConsignmentAsync(int consignmentId, ApproveConsignmentDTO approveDto);
        Task<ServiceResponseFormat<bool>> CompleteSaleConsignmentAsync(int consignmentId);
        Task<ServiceResponseFormat<FishConsignmentDTO>> GetConsignmentByIdAsync(int consignmentId);
        Task<ServiceResponseFormat<List<FishConsignmentDTO>>> GetAllConsignmentsAsync();
        Task<ServiceResponseFormat<FishConsignmentDTO>> GetConsignmentsByUserIdAsync(int userId);
        Task<ServiceResponseFormat<FishConsignmentDTO>> UpdateConsignmentAsync(FishConsignmentDTO consignmentDto);
    }
}

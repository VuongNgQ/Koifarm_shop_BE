using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using DataAccess.Entity;
using DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface IFishConsignmentService
    {
        Task<ServiceResponseFormat<int>> CreateCareConsignmentAsync(CareConsignmentDTO consignmentDto);
        Task<ServiceResponseFormat<int>> CreateSaleConsignmentAsync(SaleConsignmentDTO consignmentDto);
        Task<ServiceResponseFormat<bool>> ApproveConsignmentAsync(int consignmentId, ApproveConsignmentDTO approveDto);
        Task<ServiceResponseFormat<bool>> CompleteSaleConsignmentAsync(int consignmentId);
        Task<ServiceResponseFormat<object>> GetConsignmentByIdAsync(int consignmentId);
        Task<ServiceResponseFormat<List<FishConsignmentDTO>>> GetAllConsignmentsAsync();
        Task<ServiceResponseFormat<List<FishConsignmentDTO>>> GetConsignmentsByUserIdAsync(int userId);
        Task<ServiceResponseFormat<FishConsignmentDTO>> UpdateConsignmentAsync(FishConsignmentDTO consignmentDto);
        Task<ServiceResponseFormat<FishConsignmentDTO>> UpdateConsignmentStatusAsync(int consignmentId, ConsignmentStatusEnum newStatus, string? description = null);

    }
}

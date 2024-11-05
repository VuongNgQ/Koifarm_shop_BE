using AutoMapper;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using DataAccess.Entity;
using DataAccess.Enum;
using DataAccess.IRepo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public class FishConsignmentService : IFishConsignmentService
    {
        private readonly IFishConsignmentRepo _consignmentRepo;
        private readonly IFishRepo _fishRepo;
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public FishConsignmentService(IFishConsignmentRepo consignmentRepo, IMapper mapper, IPaymentService paymentService, IFishRepo fishRepo)
        {
            _consignmentRepo = consignmentRepo;
            _mapper = mapper;
            _paymentService = paymentService;
            _fishRepo = fishRepo;
        }

        // Tạo mới ký gửi
        public async Task<ServiceResponseFormat<FishConsignmentDTO>> CreateConsignmentAsync(CreateConsignmentDTO consignmentDTO)
        {
            var response = new ServiceResponseFormat<FishConsignmentDTO>();

            try
            {
                // Chuyển đổi DTO thành Entity
                var consignment = _mapper.Map<FishConsignment>(consignmentDTO);
                consignment.CreateDate = DateTime.Now;
                consignment.ConsignmentStatus = ConsignmentStatusEnum.PendingApproval;

                // Kiểm tra nguồn gốc của cá (của shop hay không)
                if (consignmentDTO.IsFromShop)
                {
                    // Nếu là từ shop, cần kiểm tra tồn tại của cá
                    if (consignmentDTO.FishId == null)
                    {
                        response.Success = false;
                        response.Message = "FishId is required when consignment is from shop.";
                        return response;
                    }
                }
                else
                {
                    // Nếu không phải từ shop, tạo cá mới sau khi duyệt ký gửi
                    consignment.FishId = null;
                }
                // Lưu ký gửi vào cơ sở dữ liệu
                await _consignmentRepo.AddFishConsignmentAsync(consignment);

                // Trả về DTO sau khi tạo thành công
                response.Data = _mapper.Map<FishConsignmentDTO>(consignment);
                response.Success = true;
                response.Message = "Consignment created successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error creating consignment: {ex.Message}";
            }

            return response;
        }

        // Duyệt ký gửi dựa trên mục đích (Care hoặc Sale) với switch-case
        public async Task<ServiceResponseFormat<bool>> ApproveConsignmentAsync(int consignmentId, ApproveConsignmentDTO approveDto)
        {
            var response = new ServiceResponseFormat<bool>();

            try
            {
                var consignment = await _consignmentRepo.GetFishConsignmentByIdAsync(consignmentId);
                if (consignment == null)
                {
                    response.Success = false;
                    response.Message = "Consignment not found.";
                    return response;
                }

                // Sử dụng switch-case cho các mục đích khác nhau
                switch (consignment.Purpose)
                {
                    case ConsignmentPurpose.Care:
                        if (!consignment.IsFromShop)
                        {
                            // Tạo cá mới cho ký gửi chăm sóc nếu không phải từ shop
                            var newFish = new Fish { /* Các thuộc tính dựa trên consignment */ };
                            await _fishRepo.AddFishAsync(newFish);
                            consignment.FishId = newFish.FishId;
                        }

                        // Cập nhật trạng thái và tạo thanh toán cọc
                        consignment.ConsignmentStatus = ConsignmentStatusEnum.Approved;
                        await _consignmentRepo.UpdateFishConsignmentAsync(consignment);
                        await _paymentService.CreateDepositPaymentAsync(
                            consignment.UserId.Value, consignment.FishConsignmentId, approveDto.DepositAmount.Value, "Deposit for care consignment"
                        );
                        response.Message = "Care consignment approved with deposit payment.";
                        break;

                    case ConsignmentPurpose.Sale:
                        if (!consignment.IsFromShop)
                        {
                            // Tạo cá mới cho ký gửi bán nếu không phải từ shop
                            var newFish = new Fish { /* Các thuộc tính dựa trên consignment */ };
                            await _fishRepo.AddFishAsync(newFish);
                            consignment.FishId = newFish.FishId;
                        }

                        // Cập nhật giá và trạng thái ký gửi bán
                        consignment.Price = approveDto.AgreedPrice;
                        consignment.ConsignmentStatus = ConsignmentStatusEnum.Approved;
                        await _consignmentRepo.UpdateFishConsignmentAsync(consignment);
                        response.Message = "Sale consignment approved with agreed price.";
                        break;

                    default:
                        response.Success = false;
                        response.Message = "Unsupported consignment purpose.";
                        return response;
                }

                response.Success = true;
                response.Data = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error approving consignment: {ex.Message}";
            }

            return response;
        }

        // Hoàn tất ký gửi bán và thanh toán cho khách hàng
        public async Task<ServiceResponseFormat<bool>> CompleteSaleConsignmentAsync(int consignmentId)
        {
            var response = new ServiceResponseFormat<bool>();

            try
            {
                var consignment = await _consignmentRepo.GetFishConsignmentByIdAsync(consignmentId);
                if (consignment == null || consignment.Purpose != ConsignmentPurpose.Sale)
                {
                    response.Success = false;
                    response.Message = "Consignment not found or invalid purpose.";
                    return response;
                }

                consignment.ConsignmentStatus = ConsignmentStatusEnum.Completed;
                await _consignmentRepo.UpdateFishConsignmentAsync(consignment);

                // Tạo thanh toán cho khách hàng sau khi hoàn tất bán
                await _paymentService.CreateSalePaymentAsync(consignment.UserId.Value, consignment.FishConsignmentId, consignment.Price.Value, "Payment for completed sale consignment");

                response.Success = true;
                response.Data = true;
                response.Message = "Sale consignment completed and payment processed successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error completing sale consignment: {ex.Message}";
            }

            return response;
        }

        // Lấy thông tin ký gửi theo ID
        public async Task<ServiceResponseFormat<FishConsignmentDTO>> GetConsignmentByIdAsync(int consignmentId)
        {
            var response = new ServiceResponseFormat<FishConsignmentDTO>();

            var consignment = await _consignmentRepo.GetFishConsignmentByIdAsync(consignmentId);
            if (consignment == null)
            {
                response.Success = false;
                response.Message = "Consignment not found.";
            }
            else
            {
                response.Data = _mapper.Map<FishConsignmentDTO>(consignment);
                response.Success = true;
            }

            return response;
        }

        // Lấy tất cả các ký gửi
        public async Task<ServiceResponseFormat<List<FishConsignmentDTO>>> GetAllConsignmentsAsync()
        {
            var consignments = await _consignmentRepo.GetAllFishConsignmentAsync();
            return new ServiceResponseFormat<List<FishConsignmentDTO>>
            {
                Data = _mapper.Map<List<FishConsignmentDTO>>(consignments),
                Success = true
            };
        }

        // Cập nhật thông tin ký gửi
        public async Task<ServiceResponseFormat<FishConsignmentDTO>> UpdateConsignmentAsync(FishConsignmentDTO consignmentDto)
        {
            var response = new ServiceResponseFormat<FishConsignmentDTO>();

            try
            {
                var consignment = _mapper.Map<FishConsignment>(consignmentDto);
                await _consignmentRepo.UpdateFishConsignmentAsync(consignment);

                response.Data = consignmentDto;
                response.Success = true;
                response.Message = "Consignment updated successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error updating consignment: {ex.Message}";
            }

            return response;
        }
        public async Task<ServiceResponseFormat<FishConsignmentDTO>> GetConsignmentsByUserIdAsync(int userId)
        {
            var consignments = await _consignmentRepo.GetConsignmentsByUserIdAsync(userId);
            return _mapper.Map<ServiceResponseFormat<FishConsignmentDTO>>(consignments);
        }
    }
}

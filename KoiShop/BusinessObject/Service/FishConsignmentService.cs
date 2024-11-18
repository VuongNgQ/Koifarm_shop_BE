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
        private readonly IUserFishOwnerShipRepo _ownerShipRepo;

        public FishConsignmentService(IFishConsignmentRepo consignmentRepo, IMapper mapper, IPaymentService paymentService, IFishRepo fishRepo, IUserFishOwnerShipRepo ownerShipRepo)
        {
            _consignmentRepo = consignmentRepo;
            _mapper = mapper;
            _paymentService = paymentService;
            _fishRepo = fishRepo;
            _ownerShipRepo = ownerShipRepo;
        }

        //public async Task<ServiceResponseFormat<FishConsignmentDTO>> CreateConsignmentAsync(CreateConsignmentDTO consignmentDTO)
        //{
        //    var response = new ServiceResponseFormat<FishConsignmentDTO>();

        //    try
        //    {
        //        var consignment = _mapper.Map<FishConsignment>(consignmentDTO);
        //        consignment.CreateDate = DateTime.Now;
        //        consignment.ConsignmentStatus = ConsignmentStatusEnum.PendingApproval;

        //        switch (consignmentDTO.Purpose)
        //        {
        //            case ConsignmentPurpose.Care:
        //                if (consignmentDTO.FishId == null)
        //                {
        //                    response.Success = false;
        //                    response.Message = "Fish is required for care consignments.";
        //                    return response;
        //                }

        //                var ownedFish = await _ownerShipRepo.GetOwnershipByUserAndFishAsync(consignmentDTO.UserId, consignmentDTO.FishId.Value);
        //                if (ownedFish == null)
        //                {
        //                    response.Success = false;
        //                    response.Message = "The selected fish does not belong to the user.";
        //                    return response;
        //                }
        //                break;

        //            case ConsignmentPurpose.Sale:
        //                break;

        //            default:
        //                response.Success = false;
        //                response.Message = "Invalid consignment type.";
        //                return response;
        //        }
        //        await _consignmentRepo.AddFishConsignmentAsync(consignment);

        //        response.Data = _mapper.Map<FishConsignmentDTO>(consignment);
        //        response.Success = true;
        //        response.Message = "Consignment created successfully.";
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Success = false;
        //        response.Message = $"Error creating consignment: {ex.Message}";
        //    }

        //    return response;
        //}
        public async Task<ServiceResponseFormat<FishConsignmentDTO>> CreateConsignmentAsync(CreateConsignmentDTO consignmentDTO)
        {
            var response = new ServiceResponseFormat<FishConsignmentDTO>();

            try
            {
                if (consignmentDTO.Purpose == ConsignmentPurpose.Care)
                {
                    response = await HandleCareConsignmentAsync(consignmentDTO as CreateCareConsignmentDTO);
                }
                else if (consignmentDTO.Purpose == ConsignmentPurpose.Sale)
                {
                    response = await HandleSaleConsignmentAsync(consignmentDTO as CreateSaleConsignmentDTO);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Invalid consignment type.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error creating consignment: {ex.Message}";
            }

            return response;
        }

        private async Task<ServiceResponseFormat<FishConsignmentDTO>> HandleCareConsignmentAsync(CreateCareConsignmentDTO dto)
        {
            var response = new ServiceResponseFormat<FishConsignmentDTO>();

            var ownedFish = await _ownerShipRepo.GetOwnershipByUserAndFishAsync(dto.UserId, dto.FishId);
            if (ownedFish == null)
            {
                response.Success = false;
                response.Message = "The selected fish does not belong to the user.";
                return response;
            }

            var consignment = new FishConsignment
            {
                UserId = dto.UserId,
                FishId = dto.FishId,
                Purpose = ConsignmentPurpose.Care,
                ConditionDescription = dto.ConditionDescription,
                ConsignmentStatus = ConsignmentStatusEnum.PendingApproval,
                TransferDate = dto.TransferDate,
                ReceiveDate = dto.ReceiveDate
            };

            await _consignmentRepo.AddFishConsignmentAsync(consignment);

            response.Data = _mapper.Map<FishConsignmentDTO>(consignment);
            response.Success = true;
            response.Message = "Care consignment created successfully.";

            return response;
        }

        private async Task<ServiceResponseFormat<FishConsignmentDTO>> HandleSaleConsignmentAsync(CreateSaleConsignmentDTO dto)
        {
            var response = new ServiceResponseFormat<FishConsignmentDTO>();

            var fish = new Fish
            {
                Name = dto.FishInfo.Name,
                Age = dto.FishInfo.Age,
                Gender = dto.FishInfo.Gender,
                Size = dto.FishInfo.Size,
                CategoryId = dto.FishInfo.CategoryId,
                ImageUrl = dto.FishInfo.ImageUrl,
                Status = FishStatusEnum.GOOD,
                ProductStatus = ProductStatusEnum.PENDINGAPPROVAL
            };

            await _fishRepo.AddFishAsync(fish);

            var consignment = new FishConsignment
            {
                UserId = dto.UserId,
                FishId = fish.FishId,
                Purpose = ConsignmentPurpose.Sale,
                ConditionDescription = dto.ConditionDescription,
                ConsignmentStatus = ConsignmentStatusEnum.PendingApproval
            };

            await _consignmentRepo.AddFishConsignmentAsync(consignment);

            response.Data = _mapper.Map<FishConsignmentDTO>(consignment);
            response.Success = true;
            response.Message = "Sale consignment created successfully.";

            return response;
        }

        public async Task<ServiceResponseFormat<bool>> ApproveConsignmentAsync(int consignmentId, ApproveConsignmentDTO approveDto)
        {
            var response = new ServiceResponseFormat<bool>();

            try
            {
                var consignment = await _consignmentRepo.GetFishConsignmentByIdAsync(consignmentId);
                if (consignment == null || consignment.ConsignmentStatus != ConsignmentStatusEnum.PendingApproval)
                {
                    response.Success = false;
                    response.Message = "Consignment not found or already processed.";
                    return response;
                }

                switch (consignment.Purpose)
                {
                    case ConsignmentPurpose.Care:

                        consignment.ConsignmentStatus = ConsignmentStatusEnum.Approved;
                        await _consignmentRepo.UpdateFishConsignmentAsync(consignment);
                        //await _paymentService.CreateDepositPaymentAsync(
                        //    consignment.UserId.Value, consignment.FishConsignmentId, approveDto.DepositAmount.Value, "Deposit for care consignment"
                        //);
                        response.Message = "Care consignment approved with deposit payment.";
                        break;

                    case ConsignmentPurpose.Sale:
                        consignment.Price = approveDto.AgreedPrice;
                        consignment.ConsignmentStatus = ConsignmentStatusEnum.PriceAgreed;
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

        public async Task<ServiceResponseFormat<List<FishConsignmentDTO>>> GetAllConsignmentsAsync()
        {
            var response = new ServiceResponseFormat<List<FishConsignmentDTO>>();

            try
            {
                var consignments = await _consignmentRepo.GetAllFishConsignmentAsync();

                response.Data = _mapper.Map<List<FishConsignmentDTO>>(consignments);
                response.Success = true;
                response.Message = "Consignments retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error: {ex.Message}";
            }
            return response;
        }

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

        public async Task<ServiceResponseFormat<FishConsignmentDTO>> UpdateConsignmentStatusAsync(int consignmentId, ConsignmentStatusEnum newStatus, string? description = null)
        {
            var consignment = await _consignmentRepo.GetFishConsignmentByIdAsync(consignmentId);
            if (consignment == null)
                return new ServiceResponseFormat<FishConsignmentDTO> { Success = false, Message = "Consignment not found." };

            consignment.ConsignmentStatus = newStatus;
            consignment.ConditionDescription = description;

            await _consignmentRepo.UpdateFishConsignmentAsync(consignment);
            return new ServiceResponseFormat<FishConsignmentDTO> { Success = true, Message = "Status updated successfully." };
        }
        public async Task<ServiceResponseFormat<FishConsignmentDTO>> GetConsignmentsByUserIdAsync(int userId)
        {
            var consignments = await _consignmentRepo.GetConsignmentsByUserIdAsync(userId);
            return _mapper.Map<ServiceResponseFormat<FishConsignmentDTO>>(consignments);
        }
    }
}

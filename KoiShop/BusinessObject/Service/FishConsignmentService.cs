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

        public FishConsignmentService(IFishConsignmentRepo consignmentRepo, IMapper mapper, IPaymentService paymentService, IFishRepo fishRepo)
        {
            _consignmentRepo = consignmentRepo;
            _mapper = mapper;
            _paymentService = paymentService;
            _fishRepo = fishRepo;
        }

        public async Task<ServiceResponseFormat<FishConsignmentDTO>> CreateConsignmentAsync(CreateConsignmentDTO consignmentDTO)
        {
            var response = new ServiceResponseFormat<FishConsignmentDTO>();

            try
            {
                var consignment = _mapper.Map<FishConsignment>(consignmentDTO);
                consignment.CreateDate = DateTime.Now;
                consignment.ConsignmentStatus = ConsignmentStatusEnum.PendingApproval;

                if (consignmentDTO.Purpose == ConsignmentPurpose.Care)
                {
                    if (consignmentDTO.FishId == null)
                    {
                        response.Success = false;
                        response.Message = "Fish is required for care consignments.";
                        return response;
                    }
                    var ownedFish = await _ownerShipRepo.GetOwnershipByUserAndFishAsync(consignmentDTO.UserId, consignmentDTO.FishId.Value);
                    if (ownedFish == null)
                    {
                        response.Success = false;
                        response.Message = "The selected fish does not belong to the user.";
                        return response;
                    }
                }
                else if (consignmentDTO.Purpose == ConsignmentPurpose.Sale)
                {
                    if (consignmentDTO.FishId != null)
                    {
                        var fishOwnership = await _ownerShipRepo.GetOwnershipByUserAndFishAsync(consignmentDTO.UserId, consignmentDTO.FishId.Value);
                        if (fishOwnership == null)
                        {
                            response.Success = false;
                            response.Message = "The selected fish does not belong to the user or was not purchased from the shop.";
                            return response;
                        }
                    }
                    else
                    {
                        consignment.FishId = null;
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Invalid consignment type.";
                    return response;
                }
                //if (consignmentDTO.IsFromShop)
                //{
                //    if (consignmentDTO.FishId == null)
                //    {
                //        response.Success = false;
                //        response.Message = "Fish is required when consignment is from shop.";
                //        return response;
                //    }
                //}
                //else
                //{
                //    consignment.FishId = null;
                //}
                await _consignmentRepo.AddFishConsignmentAsync(consignment);

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

                switch (consignment.Purpose)
                {
                    case ConsignmentPurpose.Care:
                        if (!consignment.IsFromShop)
                        {
                            var newFish = new Fish { /* Các thuộc tính dựa trên consignment */ };
                            await _fishRepo.AddFishAsync(newFish);
                            consignment.FishId = newFish.FishId;
                        }

                        consignment.ConsignmentStatus = ConsignmentStatusEnum.Approved;
                        await _consignmentRepo.UpdateFishConsignmentAsync(consignment);
                        await _paymentService.CreateDepositPaymentAsync(
                            consignment.UserId.Value, consignment.FishConsignmentId, approveDto.DepositAmount.Value, "Deposit for care consignment"
                        );
                        response.Message = "Care consignment approved with deposit payment.";
                        break;

                    case ConsignmentPurpose.Sale:
                        if (consignment.FishId == null)
                        {
                            var newFish = new Fish { /* Các thuộc tính dựa trên consignment */ };
                            await _fishRepo.AddFishAsync(newFish);
                            consignment.FishId = newFish.FishId;
                        }
                        else
                        {
                            var fish = await _fishRepo.GetFishByIdAsync(consignment.FishId.Value);
                            if (fish == null)
                            {
                                response.Success = false;
                                response.Message = "Fish linked to the consignment not found.";
                                return response;
                            }

                            fish.Price = approveDto.AgreedPrice;
                            fish.ProductStatus = ProductStatusEnum.AVAILABLE;
                            await _fishRepo.UpdateFishAsync(fish);
                        }

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
            var consignments = await _consignmentRepo.GetAllFishConsignmentAsync();
            return new ServiceResponseFormat<List<FishConsignmentDTO>>
            {
                Data = _mapper.Map<List<FishConsignmentDTO>>(consignments),
                Success = true
            };
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
        public async Task<ServiceResponseFormat<FishConsignmentDTO>> GetConsignmentsByUserIdAsync(int userId)
        {
            var consignments = await _consignmentRepo.GetConsignmentsByUserIdAsync(userId);
            return _mapper.Map<ServiceResponseFormat<FishConsignmentDTO>>(consignments);
        }
    }
}

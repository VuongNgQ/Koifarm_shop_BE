using AutoMapper;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using BusinessObject.Utils;
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

        public async Task<ServiceResponseFormat<int>> CreateCareConsignmentAsync(CareConsignmentDTO dto)
        {
            var response = new ServiceResponseFormat<int>();

            try
            {
                if (dto.TransferDate >= dto.ReceiveDate)
        {
            response.Success = false;
            response.Message = "Transfer date must be earlier than receive date.";
            return response;
        }
                if (dto.FishId == 0)
                {
                    response.Success = false;
                    response.Message = "FishId is required for Care Consignment.";
                    return response;
                }
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
                    StartDate = dto.TransferDate,
                    EndDate = dto.ReceiveDate,
                    InitialPrice = dto.Price
                };

                await _consignmentRepo.AddFishConsignmentAsync(consignment);

                response.Data = consignment.FishConsignmentId;
                response.Success = true;
                response.Message = "Care consignment created successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error approving consignment: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponseFormat<int>> CreateSaleConsignmentAsync(SaleConsignmentDTO dto)
        {
            var response = new ServiceResponseFormat<int>();

            try
            {
                var imageService = new CloudinaryService();
                string uploadedImageUrl = string.Empty;

                if (dto.FishInfo.ImageUrl != null)
                {
                    using (var stream = dto.FishInfo.ImageUrl.OpenReadStream())
                    {
                        uploadedImageUrl = await imageService.UploadImageAsync(stream, dto.FishInfo.ImageUrl.FileName);
                    }
                }
                var fish = new Fish
                {
                    Name = dto.FishInfo.Name,
                    Age = dto.FishInfo.Age,
                    Gender = dto.FishInfo.Gender,
                    Size = dto.FishInfo.Size,
                    CategoryId = dto.FishInfo.CategoryId,
                    ImageUrl = uploadedImageUrl,
                    Status = dto.FishInfo.Status,
                    ProductStatus = ProductStatusEnum.PENDINGAPPROVAL
                };
                await _fishRepo.AddFishAsync(fish);

                var consignment = new FishConsignment
                {
                    UserId = dto.UserId,
                    FishId = fish.FishId,
                    Purpose = ConsignmentPurpose.Sale,
                    ConditionDescription = dto.ConditionDescription,
                    Phone = dto.Phone,
                    Type = dto.Type,
                    InitialPrice = dto.Price,
                    ImageUrls = uploadedImageUrl,
                    Video = dto.Video,
                    ConsignmentStatus = ConsignmentStatusEnum.PendingApproval
                };
                await _consignmentRepo.AddFishConsignmentAsync(consignment);

                response.Data = consignment.FishConsignmentId;
                response.Success = true;
                response.Message = "Sale consignment created successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error approving consignment: {ex.Message}";
            }
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
                var fish = await _fishRepo.GetFishByIdAsync(consignment.FishId);
                if (fish == null)
                {
                    response.Success = false;
                    response.Message = "Fish associated with consignment not found.";
                    return response;
                }

                switch (consignment.Purpose)
                {
                    case ConsignmentPurpose.Care:

                        consignment.ConsignmentStatus = ConsignmentStatusEnum.Approved;
                        await _consignmentRepo.UpdateFishConsignmentAsync(consignment);
                        response.Message = "Care consignment approved.";
                        break;

                    case ConsignmentPurpose.Sale:
                        if (approveDto.AgreedPrice <= 0)
                        {
                            response.Success = false;
                            response.Message = "Agreed price must be greater than 0.";
                            return response;
                        }
                        consignment.FinalPrice = approveDto.AgreedPrice;
                        consignment.ServiceFee = approveDto.ServiceFee;
                        consignment.CommissionFee = approveDto.CommissionFee;
                        consignment.ConsignmentStatus = ConsignmentStatusEnum.Approved;
                        fish.Price = approveDto.AgreedPrice;
                        await _consignmentRepo.UpdateFishConsignmentAsync(consignment);
                        await _fishRepo.UpdateFishAsync(fish);
                        response.Message = "Sale consignment approved with agreed price and fish price updated.";
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
        public async Task<ServiceResponseFormat<bool>> ListFishForSaleAsync(int consignmentId)
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
                if (consignment.ConsignmentStatus != ConsignmentStatusEnum.Approved)
                {
                    response.Success = false;
                    response.Message = "Consignment is not approved.";
                    return response;
                }
                var fish = await _fishRepo.GetFishByIdAsync(consignment.FishId);
                if (fish == null)
                {
                    response.Success = false;
                    response.Message = "Fish associated with consignment not found.";
                    return response;
                }
                if (fish.ProductStatus != ProductStatusEnum.PENDINGAPPROVAL && fish.ProductStatus != ProductStatusEnum.AVAILABLE)
                {
                    response.Success = false;
                    response.Message = "Fish is not eligible for listing.";
                    return response;
                }
                if (fish.Price == null || fish.Price <= 0)
                {
                    response.Success = false;
                    response.Message = "Fish price must be set before listing.";
                    return response;
                }
                var currentDate = DateTime.UtcNow;
                consignment.StartDate = currentDate;
                consignment.EndDate = currentDate.AddMonths(consignment.ConsignmentDurationMonths);
                consignment.ConsignmentStatus = ConsignmentStatusEnum.OnProcessing;
                fish.ProductStatus = ProductStatusEnum.AVAILABLE;
                await _consignmentRepo.UpdateFishConsignmentAsync(consignment);
                await _fishRepo.UpdateFishAsync(fish);

                response.Success = true;
                response.Data = true;
                response.Message = "Fish has been successfully listed for sale.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error listing fish for sale: {ex.Message}";
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
                await _paymentService.CreateSalePaymentAsync(consignment.UserId.Value, consignment.FishConsignmentId, consignment.InitialPrice.Value, "Payment for completed sale consignment");

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

        public async Task<ServiceResponseFormat<object>> GetConsignmentByIdAsync(int consignmentId)
        {
            var response = new ServiceResponseFormat<object>();

            var checkConsignment = await _consignmentRepo.GetFishConsignmentByIdAsync(consignmentId);
            if (checkConsignment == null)
            {
                response.Success = false;
                response.Message = "Consignment not found.";
            }
            else
            {
                object mappedConsignment;
                if (checkConsignment.Purpose == ConsignmentPurpose.Care)
                {
                    mappedConsignment = _mapper.Map<FishConsignmentCareResponseDTO>(checkConsignment);
                }
                else if (checkConsignment.Purpose == ConsignmentPurpose.Sale)
                {
                    var saleDto = _mapper.Map<FishConsignmentSaleResponseDTO>(checkConsignment);
                    saleDto.FishInfo = _mapper.Map<FishInfoResponseDTO>(checkConsignment.Fish);
                    mappedConsignment = saleDto;
                }
                else
                {
                    mappedConsignment = new object();
                }
                response.Data = mappedConsignment;
                response.Success = true;
            }

            return response;
        }

        public async Task<ServiceResponseFormat<List<object>>> GetAllConsignmentsAsync()
        {
            var response = new ServiceResponseFormat<List<object>>();

            try
            {
                var consignments = await _consignmentRepo.GetAllFishConsignmentAsync();
                var mappedConsignments = consignments.Select(consignment =>
                {
                    if (consignment.Purpose == ConsignmentPurpose.Care)
                    {
                        return (object)_mapper.Map<FishConsignmentCareResponseDTO>(consignment);
                    }
                    else if (consignment.Purpose == ConsignmentPurpose.Sale)
                    {
                        var saleDto = _mapper.Map<FishConsignmentSaleResponseDTO>(consignment);
                        saleDto.FishInfo = _mapper.Map<FishInfoResponseDTO>(consignment.Fish);
                        return (object)saleDto;
                    }
                    return null;
                }).Where(dto => dto != null).ToList();
                response.Data = mappedConsignments;
                //response.Data = _mapper.Map<List<FishConsignmentDTO>>(consignments);
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
        public async Task<ServiceResponseFormat<List<object>>> GetConsignmentsByUserIdAsync(int userId)
        {
            var response = new ServiceResponseFormat<List<object>>();

            try
            {  
                var consignments = await _consignmentRepo.GetConsignmentsByUserIdAsync(userId);
                var mappedConsignments = consignments.Select(consignment =>
                {
                    if (consignment.Purpose == ConsignmentPurpose.Care)
                    {
                        return (object)_mapper.Map<FishConsignmentCareResponseDTO>(consignment);
                    }
                    else if (consignment.Purpose == ConsignmentPurpose.Sale)
                    {
                        var saleDto = _mapper.Map<FishConsignmentSaleResponseDTO>(consignment);
                        saleDto.FishInfo = _mapper.Map<FishInfoResponseDTO>(consignment.Fish);
                        return (object)saleDto;
                    }
                    return null;
                }).Where(dto => dto != null).ToList();
                response.Data = mappedConsignments;
                //response.Data = _mapper.Map<List<FishConsignmentDTO>>(consignments);
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
        private decimal CalculateServiceFee(decimal? size)
        {
            if (size == null) return 0;
            decimal baseFee = 20000;                      // Phí cơ bản
            decimal sizeMultiplier = size.Value * 10000;   // Tăng phí theo kích thước
            return baseFee + sizeMultiplier;
        }
    }
}

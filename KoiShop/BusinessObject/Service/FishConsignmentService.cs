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
        //public async Task<ServiceResponseFormat<FishConsignmentDTO>> CreateConsignmentAsync(CreateConsignmentDTO consignmentDTO)
        //{
        //    var response = new ServiceResponseFormat<FishConsignmentDTO>();

        //    try
        //    {
        //        if (consignmentDTO == null)
        //        {
        //            response.Success = false;
        //            response.Message = "Consignment data is null.";
        //            return response;
        //        }
        //        if (consignmentDTO.Purpose == ConsignmentPurpose.Care)
        //        {
        //            var careDto = consignmentDTO as CreateCareConsignmentDTO;
        //            if (careDto == null)
        //            {
        //                response.Success = false;
        //                response.Message = "Invalid data for Care Consignment.";
        //                return response;
        //            }
        //            response = await HandleCareConsignmentAsync(careDto);
        //        }
        //        else if (consignmentDTO.Purpose == ConsignmentPurpose.Sale)
        //        {
        //            var saleDto = consignmentDTO as CreateSaleConsignmentDTO;
        //            if (saleDto == null || saleDto.FishInfo == null)
        //            {
        //                response.Success = false;
        //                response.Message = "Invalid data for Sale Consignment.";
        //                return response;
        //            }
        //            response = await HandleSaleConsignmentAsync(saleDto);
        //        }
        //        else
        //        {
        //            response.Success = false;
        //            response.Message = "Invalid consignment type.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Success = false;
        //        response.Message = $"Error creating consignment: {ex.Message}";
        //    }

        //    return response;
        //}

        public async Task<ServiceResponseFormat<int>> CreateCareConsignmentAsync(CareConsignmentDTO dto)
        {
            var response = new ServiceResponseFormat<int>();

            try
            {
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
                    TransferDate = dto.TransferDate,
                    ReceiveDate = dto.ReceiveDate
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
                        //consignment.Fish.Price = approveDto.FishSaleAmount;
                        //await _fishRepo.UpdateFishAsync(consignment.Fish);
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
        public async Task<ServiceResponseFormat<List<FishConsignmentDTO>>> GetConsignmentsByUserIdAsync(int userId)
        {
            var response = new ServiceResponseFormat<List<FishConsignmentDTO>>();

            try
            {
                var consignments = await _consignmentRepo.GetConsignmentsByUserIdAsync(userId);
                //var mappedConsignments = consignments.Select(consignment =>
                //{
                //    if (consignment.Purpose == ConsignmentPurpose.Care)
                //    {
                //        return (object)_mapper.Map<FishConsignmentCareResponseDTO>(consignment);
                //    }
                //    else if (consignment.Purpose == ConsignmentPurpose.Sale)
                //    {
                //        var saleDto = _mapper.Map<FishConsignmentSaleResponseDTO>(consignment);
                //        saleDto.FishInfo = _mapper.Map<FishInfoResponseDTO>(consignment.Fish);
                //        return (object)saleDto;
                //    }
                //    return null;
                //}).Where(dto => dto != null).ToList();
                //response.Data = mappedConsignments;

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
    }
}

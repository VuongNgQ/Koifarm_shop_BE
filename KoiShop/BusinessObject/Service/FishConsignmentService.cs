using AutoMapper;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using DataAccess.Entity;
using DataAccess.Enum;
using DataAccess.IRepo;
using DataAccess.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public class FishConsignmentService : IFishConsignmentService
    {
        private readonly IFishConsignmentRepo _consignmentRepo;
        private readonly IMapper _mapper;

        public FishConsignmentService(IFishConsignmentRepo consignmentRepo, IMapper mapper)
        {
            _consignmentRepo = consignmentRepo;
            _mapper = mapper;
        }


        public async Task<ServiceResponseFormat<FishConsignmentDTO>> CreateConsignment(CreateConsignmentDTO consignmentDto)
        {
            var response = new ServiceResponseFormat<FishConsignmentDTO>();

            try
            {
                var consignment = _mapper.Map<FishConsignment>(consignmentDto);
                consignment.CreateDate = DateTime.Now;
                consignment.ConsignmentStatusId = ConsignmentStatusEnum.Pending;

                await _consignmentRepo.AddAsync(consignment);

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

        public async Task<ServiceResponseFormat<FishConsignmentDTO>> GetConsignmentById(int consignmentId)
        {
            var response = new ServiceResponseFormat<FishConsignmentDTO>();
            var consignment = await _consignmentRepo.GetConsignmentByIdAsync(consignmentId);

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

        public async Task<ServiceResponseFormat<List<FishConsignmentDTO>>> GetAllConsignments()
        {
            var consignments = await _consignmentRepo.GetAllConsignmentsAsync();
            return new ServiceResponseFormat<List<FishConsignmentDTO>>
            {
                Data = _mapper.Map<List<FishConsignmentDTO>>(consignments),
                Success = true
            };
        }
        //public async Task<ServiceResponseFormat<PaginationModel<FishConsignmentDTO>>> GetAllConsignments(int page = 1, int pageSize = 10, string search = "", string sort = "")
        //{
        //    var res = new ServiceResponseFormat<PaginationModel<FishConsignmentDTO>>();
        //    //var consignments = await _consignmentRepo.GetAllConsignmentsAsync();
        //    //response.Data = _mapper.Map<List<FishConsignmentDTO>>(consignments);
        //    //response.Success = true;
        //    try
        //    {
        //        var consignments = await _consignmentRepo.GetAllConsignmentsAsync();
        //        if (!string.IsNullOrEmpty(search))
        //        {
        //            consignments = consignments.Where(e => e.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
        //            e.Email.Contains(search, StringComparison.OrdinalIgnoreCase) ||
        //            e.Phone.Contains(search, StringComparison.OrdinalIgnoreCase));
        //        }
        //        consignments = sort.ToLower().Trim() switch
        //        {
        //            "name" => consignments.OrderBy(e => e.Name),
        //            "birthday" => consignments.OrderBy(e => e.DateOfBirth),
        //            _ => consignments.OrderBy(e => e.UserId)
        //        };
        //        var mapp = _mapper.Map<IEnumerable<FishConsignmentDTO>>(consignments);
        //        if (mapp.Any())
        //        {
        //            var paginationModel = await Utils.Pagination.GetPaginationEnum(mapp, page, pageSize);
        //            res.Success = true;
        //            res.Message = "Get Users successfully";
        //            res.Data = paginationModel;
        //            return res;
        //        }
        //        else
        //        {
        //            res.Success = false;
        //            res.Message = "No User Found.";
        //            return res;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Success = false;
        //        res.Message = $"Fail to get User:{ex.Message}";
        //    }
        //    return res;
        //}

        public async Task<ServiceResponseFormat<FishConsignmentDTO>> UpdateConsignmentAsync(FishConsignmentDTO consignmentDto)
        {
            var consignment = _mapper.Map<FishConsignment>(consignmentDto);

            try
            {
                await _consignmentRepo.UpdateConsignmentAsync(consignment);
                return new ServiceResponseFormat<FishConsignmentDTO>
                {
                    Data = consignmentDto,
                    Success = true,
                    Message = "Consignment updated successfully."
                };
            }
            catch (KeyNotFoundException ex)
            {
                return new ServiceResponseFormat<FishConsignmentDTO>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

    }
}

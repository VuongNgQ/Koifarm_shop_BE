using AutoMapper;
using Azure;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using BusinessObject.Utils;
using DataAccess.Entity;
using DataAccess.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public class PaymentMethodService: IPaymentMethodService
    {
        private readonly IPaymentMethodRepo _repo;
        private readonly IMapper _mapper;
        public PaymentMethodService(IPaymentMethodRepo repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<ServiceResponseFormat<ResponsePaymentMethodDTO>> CreatePayment(CreatePaymentMethodDTO paymentMethodDTO)
        {
            var res = new ServiceResponseFormat<ResponsePaymentMethodDTO>();
            try
            {
                var payments=await _repo.GetAllAsync();
                if(payments.Any(p=>p.Name==paymentMethodDTO.Name) )
                {
                    res.Success = false;
                    res.Message = "Name exist";
                    return res;
                }
                else
                {
                    var mapp = _mapper.Map<PaymentMethod>(paymentMethodDTO);
                    await _repo.AddAsync(mapp);
                    var result = _mapper.Map<ResponsePaymentMethodDTO>(mapp);
                    res.Success = true;
                    res.Message = "Create Payment Successfully";
                    res.Data= result;
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create Payment:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<bool>> DeletePaymentById(int id)
        {
            var res=new ServiceResponseFormat<bool>();
            try
            {
                var exist=await _repo.GetByIdAsync(id);
                if (exist != null)
                {
                    _repo.Remove(exist);
                    res.Success = true;
                    res.Message = "Delete Payment Successfully";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Payment";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to delete Payment:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<PaginationModel<ResponsePaymentMethodDTO>>> GetPayments(int page = 1, int pageSize = 10, string search = "", string sort = "")
        {
            var res = new ServiceResponseFormat<PaginationModel<ResponsePaymentMethodDTO>>();
            try
            {
                var payments = await _repo.GetAllAsync();
                if (!string.IsNullOrEmpty(search))
                {
                    payments = payments.Where(s => s.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
                }
                payments = sort.ToLower().Trim() switch
                {
                    "name" => payments.OrderBy(s => s.Name),
                    _ => payments.OrderBy(s => s.PaymentMethodId)
                };
                var mapp = _mapper.Map<IEnumerable<ResponsePaymentMethodDTO>>(payments);
                if (mapp.Any())
                {
                    var paginationModel = await Pagination.GetPaginationEnum(mapp, page, pageSize);
                    res.Success = true;
                    res.Message = "Get Payment successfully";
                    res.Data = paginationModel;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No Payment";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to get Payment:{ex.Message}";
                return res;
            }
        }
    }
}

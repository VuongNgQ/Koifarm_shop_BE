using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface IPaymentMethodService
    {
        Task<ServiceResponseFormat<ResponsePaymentMethodDTO>> CreatePayment(CreatePaymentMethodDTO paymentMethodDTO);

        Task<ServiceResponseFormat<PaginationModel<ResponsePaymentMethodDTO>>> GetPayments(int page = 1, int pageSize = 10, string search = "", string sort = "");
        Task<ServiceResponseFormat<bool>> DeletePaymentById(int id);
    }
}

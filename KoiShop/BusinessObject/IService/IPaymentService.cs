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
    public interface IPaymentService
    {
        Task<Payment?> GetPaymentByIdAsync(int id);
        Task<ServiceResponseFormat<List<PaymentDTO>>> GetPaymentByUserIdAsync(int id);
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<Payment> CreateDepositPaymentAsync(int userId, int relatedId, decimal amount, string description);
        Task<Payment> CreateSalePaymentAsync(int userId, int relatedId, decimal amount, string description);
        Task UpdatePaymentStatusAsync(int paymentId, PaymentStatus newStatus);
    }
}

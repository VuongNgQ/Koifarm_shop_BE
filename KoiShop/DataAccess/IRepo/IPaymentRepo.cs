using DataAccess.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.IRepo
{
    public interface IPaymentRepo
    {
        Task<Payment?> GetPaymentByIdAsync(int id);
        Task<List<Payment>> GetPaymentByUserIdAsync(int id);
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<Payment?> GetPaymentByTransactionIdAsync(int transactionId);
        Task<Payment?> AddPaymentAsync(Payment payment);
        Task<Payment?> UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(int id);
    }
}
using BusinessObject.IService;
using DataAccess.Entity;
using DataAccess.Enum;
using DataAccess.IRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepo _paymentRepository;

        public PaymentService(IPaymentRepo paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            return await _paymentRepository.GetPaymentByIdAsync(id);
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _paymentRepository.GetAllPaymentsAsync();
        }

        // Tạo thanh toán cọc cho dịch vụ ký gửi chăm sóc
        public async Task<Payment> CreateDepositPaymentAsync(int userId, int relatedId, decimal amount, string description)
        {
            var payment = new Payment
            {
                UserId = userId,
                TransactionType = TransactionType.CareConsignment,
                RelatedId = relatedId,
                Amount = amount,
                PaymentDate = DateTime.Now,
                PaymentStatus = PaymentStatus.Pending,
                Description = description
            };

            return await _paymentRepository.AddPaymentAsync(payment);
        }

        // Tạo thanh toán khi ký gửi bán hoàn tất
        public async Task<Payment> CreateSalePaymentAsync(int userId, int relatedId, decimal amount, string description)
        {
            var payment = new Payment
            {
                UserId = userId,
                TransactionType = TransactionType.SaleConsignment,
                RelatedId = relatedId,
                Amount = amount,
                PaymentDate = DateTime.Now,
                PaymentStatus = PaymentStatus.Completed,
                Description = description
            };

            return await _paymentRepository.AddPaymentAsync(payment);
        }

        public async Task UpdatePaymentStatusAsync(int paymentId, PaymentStatus newStatus)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
            if (payment == null)
                throw new KeyNotFoundException("Payment not found.");

            payment.PaymentStatus = newStatus;
            await _paymentRepository.UpdatePaymentAsync(payment);
        }

        // Thêm phương thức mới trong PaymentService
        public async Task<Payment> CreateRemainingCarePaymentAsync(int userId, int relatedId, decimal amount, string description)
        {
            var payment = new Payment
            {
                UserId = userId,
                TransactionType = TransactionType.CareConsignment,
                RelatedId = relatedId,
                Amount = amount,
                PaymentDate = DateTime.Now,
                PaymentStatus = PaymentStatus.Pending,
                Description = description
            };
            return await _paymentRepository.AddPaymentAsync(payment);
        }

        public async Task<Payment> CompleteSalePaymentToCustomerAsync(int userId, int relatedId, decimal amount, string description)
        {
            var payment = new Payment
            {
                UserId = userId,
                TransactionType = TransactionType.SaleConsignment,
                RelatedId = relatedId,
                Amount = amount,
                PaymentDate = DateTime.Now,
                PaymentStatus = PaymentStatus.Completed,
                Description = description
            };
            return await _paymentRepository.AddPaymentAsync(payment);
        }

    }
}

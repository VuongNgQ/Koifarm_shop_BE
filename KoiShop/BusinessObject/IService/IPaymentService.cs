﻿using BusinessObject.Model.ResponseDTO;
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
        Task<ServiceResponseFormat<PaymentDTO>> GetPaymentByIdAsync(int id);
        Task<ServiceResponseFormat<List<PaymentDTO>>> GetPaymentByUserIdAsync(int id);
        Task<ServiceResponseFormat<List<PaymentDTO>>> GetAllPaymentsAsync();
        Task<Payment> CreateSalePaymentAsync(int userId, int relatedId, decimal amount, string description);
        Task UpdatePaymentStatusAsync(int paymentId, PaymentStatus newStatus);
    }
}

﻿using BusinessObject.IService;
using DataAccess.Entity;
using DataAccess.Enum;
using DataAccess.IRepo;
using BusinessObject.Model.ResponseDTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Repo;
using AutoMapper;
using Azure;

namespace BusinessObject.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepo _paymentRepository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepo paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponseFormat<PaymentDTO>> GetPaymentByIdAsync(int id)
        {
            var response = new ServiceResponseFormat<PaymentDTO>();
            try
            {
                var payments = await _paymentRepository.GetPaymentByIdAsync(id);

                if (payments == null)
                {
                    throw new Exception("Payment not found.");
                }

                response.Data = _mapper.Map<PaymentDTO>(payments);
                response.Success = true;
                response.Message = "Payments successfully retrieved.";
            }
            catch (Exception ex)
            {

                response.Success = false;
                response.Message = $"Error getting transaction: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponseFormat<List<PaymentDTO>>> GetPaymentByUserIdAsync(int id)
        {
            var response = new ServiceResponseFormat<List<PaymentDTO>>();
            try
            {
                var payments = await _paymentRepository.GetPaymentByUserIdAsync(id);

                if (payments == null)
                {
                    throw new Exception("Payment not found.");
                }
                
                response.Data = _mapper.Map<List<PaymentDTO>>(payments);
                response.Success = true;
                response.Message = "Payments successfully retrieved.";
            }
            catch (Exception ex)
            {

                response.Success = false;
                response.Message = $"Error getting transaction: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponseFormat<List<PaymentDTO>>> GetAllPaymentsAsync()
        {
            var response = new ServiceResponseFormat<List<PaymentDTO>>();
            try
            {
                var payments = await _paymentRepository.GetAllPaymentsAsync();

                if (payments == null)
                {
                    throw new Exception("Payment not found.");
                }

                response.Data = _mapper.Map<List<PaymentDTO>>(payments);
                response.Success = true;
                response.Message = "Payments successfully retrieved.";
            }
            catch (Exception ex)
            {

                response.Success = false;
                response.Message = $"Error getting transaction: {ex.Message}";
            }
            return response;
        }

        public async Task<Payment> CreateSalePaymentAsync(int userId, int relatedId, decimal amount, string description)
        {
            var payment = new Payment
            {
                TransactionType = TransactionPurpose.SaleConsignment,
                FishConsignmentId = relatedId,
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
            {
                throw new KeyNotFoundException("Payment not found.");
            }

            payment.PaymentStatus = newStatus;
            await _paymentRepository.UpdatePaymentAsync(payment);
        }
        public async Task<Payment> CreateRemainingCarePaymentAsync(int userId, int relatedId, decimal amount, string description)
        {
            var payment = new Payment
            {
                TransactionType = TransactionPurpose.CareConsignment,
                FishConsignmentId = relatedId,
                Amount = amount,
                PaymentDate = DateTime.Now,
                PaymentStatus = PaymentStatus.Pending,
                Description = description
            };
            return await _paymentRepository.AddPaymentAsync(payment);
        }
    }
}

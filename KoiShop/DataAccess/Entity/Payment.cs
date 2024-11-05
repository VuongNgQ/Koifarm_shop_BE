using DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public TransactionType TransactionType { get; set; } // Loại giao dịch (CareConsignment, SaleConsignment, BuyFish, SellFishBatch)
        public int? RelatedId { get; set; } // Liên kết đến FishConsignment hoặc Fish
        public PaymentStatus PaymentStatus { get; set; } // Trạng thái thanh toán
        public decimal Amount { get; set; } // Số tiền thanh toán
        public string? Currency { get; set; } // Đơn vị tiền tệ
        public DateTime PaymentDate { get; set; } // Ngày thanh toán
        public DateTime? ExpireDate { get; set; }
        public string Description { get; set; }

    }

}

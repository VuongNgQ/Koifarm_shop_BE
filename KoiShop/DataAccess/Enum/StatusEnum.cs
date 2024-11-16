using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Enum
{
    
    public enum ProductStatusEnum
    {
        AVAILABLE,
        UNAVAILABLE,
        SOLDOUT,
        RESERVED,
        RETIRED
    }
    public enum FishStatusEnum
    {
        GOOD,
        MEDIUM,
        BAD
    }
    public enum OrderStatusEnum
    {
        PENDING,
        COMPLETED, 
        CANCELLED, 
        ONPORT,
        DELIVERED,
        FAILEDDELIVERY
    }
    public enum ConsignmentStatusEnum
    {
        PendingApproval,
        Approved,
        Rejected,
        Completed
    }
    public enum UserStatusEnum
    {
        ACTIVE,
        INACTIVE
    }
    public enum FishGenderEnum
    {
        Male,
        Female
    }
    public enum ConsignmentPurpose
    {
        Care = 0,
        Sale = 1
    }
    public enum TransactionType
    {
        CareConsignment = 0,
        SaleConsignment = 1,
        BuyFish = 2,
        SellFishBatch = 3,
        Refund = 4,
    }
    public enum PaymentMethod
    {
        CASH,
        ZALOPAY
    }
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        REFUNDED,
        CANCELLED
    }
}

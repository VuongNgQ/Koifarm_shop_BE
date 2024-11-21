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
        EMPTY,
        NOTFULL,
        UNAVAILABLE,
        PENDINGPAID,
        SOLDOUT,
        PENDINGAPPROVAL
    }
    public enum CartItemStatus
    {
        PENDING_FOR_ORDER,
        ADDED_IN_ORDER,
        READY_FOR_ORDER,
        COMPLETE_AT_ORDER,
        CANCEL_AT_ORDER,
        TAKEN_BY_OTHERS
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
        READY
    }
    public enum ConsignmentStatusEnum
    {
        PendingApproval,
        Approved,
        OnProcessing,
        Rejected,
        Completed,
        Cancelled
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
    public enum ConsignmentType
    {
        Offline = 0,
        Online = 1
    }
    public enum TransactionPurpose
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

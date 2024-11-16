﻿using System;
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
        PENDINGPAID,
        SOLDOUT
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
        SellFishBatch = 3
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
        Failed
    }
}

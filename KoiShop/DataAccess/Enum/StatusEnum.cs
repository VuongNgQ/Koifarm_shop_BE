﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Enum
{
    public enum GenderEnum
    {
        MALE,
        FEMALE
    }
    public enum ProductStatusEnum
    {
        AVAILABLE,
        UNAVAILABLE,
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
        CANCELLED
    }
    public enum ConsignmentStatusEnum
    {

    }
    public enum UserStatusEnum
    {
        ACTIVE,
        INACTIVE
    }
}

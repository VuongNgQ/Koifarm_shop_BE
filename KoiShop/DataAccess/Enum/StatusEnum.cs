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
        UNAVAILABLE
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
        Pending,
        Approved,
        Rejected,
        Completed
    }
    public enum UserStatusEnum
    {
        ACTIVE,
        INACTIVE
    }
}

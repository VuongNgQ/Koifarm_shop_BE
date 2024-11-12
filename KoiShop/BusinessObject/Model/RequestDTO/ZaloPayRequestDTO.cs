using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class ZaloPayRequestDTO
    {
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Item { get; set; } = string.Empty;
    }
    public class ZaloPayCallbackRequestDTO
    {
        public string Data { get; set; }
        public string Mac { get; set; }
        public int Type { get; set; }
    }
}

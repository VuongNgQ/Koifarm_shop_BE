using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class ZaloPayRequestDTO
    {
        //public string app_id { get; set; }
        //public string app_trans_id { get; set; }
        //public long app_time { get; set; }
        //public string app_user { get; set; }
        //public string amount { get; set; }
        //public string description { get; set; }
        //public string bank_code { get; set; } = "zalopayapp";
        //public string embed_data { get; set; }
        //public string items { get; set; }
        //public string mac { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Item { get; set; } = string.Empty;
    }
    public class ZaloPayCallbackRequestDTO
    {
        public int AppId { get; set; }
        public string AppTransId { get; set; }
        public decimal Amount { get; set; }
        public long AppTime { get; set; }
        public int ResultCode { get; set; }
        public string Mac { get; set; }
    }
}

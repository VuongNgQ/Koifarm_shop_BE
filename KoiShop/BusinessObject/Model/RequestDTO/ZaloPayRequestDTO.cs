using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class ZaloPayRequestDTO
    {
        public string app_id { get; set; }
        public string app_trans_id { get; set; }
        public long app_time { get; set; }
        public string app_user { get; set; }
        public string amount { get; set; }
        public string description { get; set; }
        public string bank_code { get; set; } = "zalopayapp";
        public string embed_data { get; set; }
        public string items { get; set; }
        public string mac { get; set; }
    }
    public class ZaloPayCallbackRequestDTO
    {
        public string data { get; set; }
        public string mac { get; set; }
    }
}

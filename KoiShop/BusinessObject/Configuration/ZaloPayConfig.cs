using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Configuration
{
    public class ZaloPayConfig
    {
        public string AppUser { get; set; } = string.Empty;
        public int AppId { get; set; }
        public string Key1 { get; set; } = string.Empty;
        public string Key2 { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
    }
}

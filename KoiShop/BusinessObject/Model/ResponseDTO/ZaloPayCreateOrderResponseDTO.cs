using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.ResponseDTO
{
    public class ZaloPayCreateOrderResponseDTO
    {
        [JsonProperty("returncode")]
        public int ReturnCode { get; set; }

        [JsonProperty("returnmessage")]
        public string ReturnMessage { get; set; }

        [JsonProperty("zptranstoken")]
        public string ZpTransToken { get; set; }

        [JsonProperty("orderurl")]
        public string OrderUrl { get; set; }
    }
}

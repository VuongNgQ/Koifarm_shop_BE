using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.ResponseDTO
{
    public class ResponseAddressDTO
    {
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string District { get; set; }
    }
}

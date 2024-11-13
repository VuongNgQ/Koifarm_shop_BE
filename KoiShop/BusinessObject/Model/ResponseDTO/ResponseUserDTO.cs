using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.ResponseDTO
{
    public class ResponseUserDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public String RoleName { get; set; }
        public int UserCartId {  get; set; }
        public string Status { get; set; }
        public List<ResponseAddressDTO> Addresses { get; set; }
        public List<UserOrder> Orders { get; set; }
    }
    public class UserOrder
    {
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Status { get; set; }
    }
    public class UserResponseDTO
    {
       
    }
}

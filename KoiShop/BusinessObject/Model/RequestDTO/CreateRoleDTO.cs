﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class CreateRoleDTO
    {
        [Required(ErrorMessage ="Name is required")]
        [StringLength(20,MinimumLength =2, ErrorMessage ="Min length is 2 and Max length is 20")]
        public string RoleName { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string? PermissionName { get; set; }
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}

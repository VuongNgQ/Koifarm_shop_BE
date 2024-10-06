﻿using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController:ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService service)
        {
            _roleService = service;
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleDTO role)
        {
            var result=await _roleService.CreateRole(role);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _roleService.GetRoles();
            return Ok(result);
        }
    }
}
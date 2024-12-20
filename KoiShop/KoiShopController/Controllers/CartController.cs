﻿using BusinessObject.IService;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;
        public CartController(ICartService service)
        {
            _service = service;
        }
        /// <summary>
        /// Get Cart By User ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Cart For User with the ID</returns>
        [HttpGet("User/{id}")]
        public async Task<IActionResult> GetByUserId(int id)
        {
            var result=await _service.GetCartByUserId(id);
            if(result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}

﻿using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.RequestDTO.UpdateReq.Entity;
using BusinessObject.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        public OrderController(IOrderService service)
        {
            _service = service;
        }
        /// <summary>
        /// Get all Orders
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="search"></param>
        /// <param name="sort"></param>
        /// <returns>A list of Order</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllOrder(int page = 1, int pageSize = 10, string? search = "", string sort = "")
        {
            var result = await _service.GetOrders(page, pageSize, search, sort);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }
        /// <summary>
        /// Create Order with item inside
        /// </summary>
        /// <param name="orderDTO"></param>
        /// <returns></returns>
        [HttpPost("IncludeItems")]
        public async Task<IActionResult> CreateOrderWithItem([FromForm] CreateOrderDTO orderDTO)
        {
            var result = await _service.CreateOrderWithItems(orderDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            else { return BadRequest(result.Message); }
        }
        /*/// <summary>
        /// Get Order by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <summary>
        /// Create Order
        /// </summary>
        /// <param name="orderDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromForm]CreateOrderDTO orderDTO)
        {
            var result = await _service.CreateOrder(orderDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            else { return BadRequest(result.Message); }
        }*/
        /// <summary>
        /// Get Order by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetOrderById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }

        /// <summary>
        /// Get Order by UserID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var result = await _service.GetOrdersByUserIdAsync(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }

        /// <summary>
        /// Delete Order by Order ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        
        public async Task<IActionResult> DeleteById(int id)
        {
            var result=await _service.DeleteOrder(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
        /// <summary>
        /// Update Order 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orderDTO">Only update Address, ...Can't Update Item</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        
        public async Task<IActionResult> UpdateById([FromRoute]int id, [FromForm]UpdateOrderDTO orderDTO)
        {
            var result = await _service.UpdateOrder(id, orderDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
        /// <summary>
        /// Change status of Order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status">COMPLETED, CANCELLED, READY,ONPORT,DELIVERED,FAILEDDELIVERY</param>
        /// <returns></returns>
        [HttpPatch("ChangeStatus/{id}&&{status}")]
        
        public async Task<IActionResult> ChangeStatus([FromRoute] int id, string status)
        {
            var result = await _service.ChangeStatus(id, status);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
        /*/// <summary>
        /// Completing Order
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns></returns>
        [HttpPatch("FinishOrder/{id}")]
        public async Task<IActionResult> FinishOrder([FromRoute] int id)
        {
            var result = await _service.FinishOrder(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
        [HttpPatch("CancelOrder/{id}")]
        public async Task<IActionResult> CancelOrder([FromRoute] int id)
        {
            var result = await _service.CancelOrder(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }*/
        /// <summary>
        /// Confirm finished Order.
        /// </summary>
        /// <param name="orderId">ID of order</param>
        [HttpPut("complete/{orderId}")]
        public async Task<IActionResult> ConfirmOrderCompletion(int orderId)
        {
            try
            {
                await _service.MarkOrderAsCompleted(orderId);
                return Ok(new
                {
                    Success = true,
                    Message = "Order has been marked as completed successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = $"Error while marking order as completed: {ex.Message}"
                });
            }
        }
    }
}

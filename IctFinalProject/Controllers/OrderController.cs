using System;
using System.Linq;
using System.Threading.Tasks;
using IctFinalProject.DTOs;
using IctFinalProject.Models;
using IctFinalProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IctFinalProject.Controllers
{
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public OrderController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpPost("{userId:int}")]
        public async Task<IActionResult> CreateUsersOrder(int userId, OrderCreationDto orderCreationDto)
        {
            var activeOrderId = await
                (from order in _context.Orders
                    join cart in _context.Carts on order.CartId equals cart.Id
                    orderby order.CreationDate
                    where cart.UserId.Equals(userId) && 
                          (order.OrderStatus != OrderStatus.InProgress || order.OrderStatus != OrderStatus.Ready)
                    select order).FirstOrDefaultAsync();

            if (activeOrderId is not null)
            {
                return BadRequest("There is an active order. Complete the last order to make new one");
            }
            
            // Get current cart, if it exists
            var usersCart = await _context.Carts.FirstOrDefaultAsync(x => x.IsActive && x.UserId.Equals(userId));
            if (usersCart is null)
            {
                return BadRequest("Cart is empty or not found");
            }
            usersCart.IsActive = false;

            _context.Orders.Add(new Order()
            {
                CartId = usersCart.Id,
                DeliveryTime = orderCreationDto.DeliveryTime,
                PhoneNumber = orderCreationDto.PhoneNumber
            });

            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet("{userid:int}")]
        public async Task<IActionResult> GetByUserId(int userid)
        {
            var result = await (
                from order in _context.Orders
                join cart in _context.Carts on order.CartId equals cart.Id
                where cart.UserId.Equals(userid) && 
                      (order.OrderStatus == OrderStatus.Ready || order.OrderStatus == OrderStatus.InProgress)
                select new OrderInfoViewModel
                {
                    OrderId = order.Id,
                    OrderCode = order.OrderCode,
                    DeliveryTime = order.DeliveryTime,
                    OrderStatus = order.OrderStatus,
                    PhoneNumber = order.PhoneNumber
                }).ToListAsync();

            if (result is null)
            {
                return BadRequest("No active order for this user");
            }

            foreach (var item in result)
            {
                item.Products = await (
                    from product in _context.Products
                    join productInCart in _context.ProductsInCarts on product.Id equals productInCart.ProductId
                    join cart in _context.Carts on productInCart.CartId equals cart.Id
                    join order in _context.Orders on cart.Id equals order.CartId
                    where item.OrderId.Equals(order.Id)
                    select product).ToListAsync();
            }

            return Ok(result);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var orders = await (
                from order in _context.Orders
                join cart in _context.Carts on order.CartId equals cart.Id
                orderby order.OrderStatus
                select new OrderInfoViewModel
                {
                    OrderId = order.Id,
                    OrderCode = order.OrderCode,
                    DeliveryTime = order.DeliveryTime,
                    OrderStatus = order.OrderStatus,
                    PhoneNumber = order.PhoneNumber
                }).ToListAsync();

            foreach (var order in orders)
            {
                order.Products = await
                    (from product in _context.Products
                        join productInCart in _context.ProductsInCarts on product.Id equals productInCart.ProductId
                        join cart in _context.Carts on productInCart.CartId equals cart.Id
                        join o in _context.Orders on cart.Id equals o.CartId
                        where o.Id.Equals(order.OrderId)
                        select product).ToListAsync();
            }

            return Ok(orders);
        }

        [HttpPatch("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromQuery] OrderStatus orderStatus)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (order is null)
            {
                return NotFound("order is not found");
            }

            order.OrderStatus = orderStatus;
            
            await _context.SaveChangesAsync();
            return Ok();
        }
        
    }
}
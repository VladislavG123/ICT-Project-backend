using System;
using System.Linq;
using System.Threading.Tasks;
using IctFinalProject.Models;
using IctFinalProject.Models.Models;
using IctFinalProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IctFinalProject.Controllers
{
    public class CartController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public CartController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("/api/products/from_cart/{userId:int}")]
        public async Task<IActionResult> GetUsersCart(int userId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.IsActive);
            if (cart is null)
            {
                return NotFound("Cart is not found");
            }

            var products = await
                (from productInCart in _context.ProductsInCarts
                    join product in _context.Products on productInCart.ProductId equals product.Id
                    where productInCart.CartId.Equals(cart.Id)
                    select product).ToListAsync();

            return Ok(products);
        }

        [HttpPost("/api/products/{productId:guid}/cart/{userId:int}")]
        public async Task<IActionResult> AddToCart(Guid productId, int userId)
        {
            if (!await _context.Products.AnyAsync(x => x.Id.Equals(productId) && x.IsActive))
            {
                return BadRequest("Product is not found");
            }

            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.IsActive);

            if (cart is null)
            {
                cart = new Cart
                {
                    UserId = userId
                };
                _context.Carts.Add(cart);
            };

            _context.ProductsInCarts.Add(new ProductInCart
            {
                CartId = cart.Id,
                ProductId = productId
            });

            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpDelete("/api/products/{productId:guid}/cart/{userId:int}")]
        public async Task<IActionResult> RemoveFromCart(Guid productId, int userId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.IsActive);

            if (cart is null)
            {
                return NotFound("Cart is not found");
            }

            var product = await _context.ProductsInCarts
                .FirstOrDefaultAsync(x => x.CartId.Equals(cart.Id) && x.ProductId.Equals(productId));

            if (product is null)
            {
                return NotFound("Product is not found");
            }

            _context.ProductsInCarts.Remove(product);

            await _context.SaveChangesAsync();

            return Ok("Deleted");
        }
        
        
    }
}
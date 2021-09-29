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
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ProductController(ApplicationContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// GET all products
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromBody] GetAllProductParameter parameter)
        {
            var products = await _context.Products
                .Where(x => (parameter.ShowInactive || x.IsActive) && !x.IsDeleted)
                .ToListAsync();

            return Ok(products);
        }

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id.Equals(id));

            return product is null ? NotFound("Product with given id is not found") : Ok(product);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateProjectParameter parameter)
        {
            _context.Products.Add(new Product
            {
                Title = parameter.Title,
                Details = parameter.Details,
                Price = parameter.Price
            });

            await _context.SaveChangesAsync();
            
            return Ok("Created");
        }

        
        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (product is null)
            {
                return NotFound("Product is not found");
            }

            product.IsDeleted = true;

            return Ok("Deleted");
        }

        [HttpPatch("{id:guid}/change_isActive")]
        [Authorize]
        public async Task<IActionResult> ChangeIsActive(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (product is null)
            {
                return NotFound("Product is not found");
            }

            product.IsActive = !product.IsActive;

            return Ok("Changed");
        }
        
        [HttpPatch("make_inactive")]
        [Authorize]
        public async Task<IActionResult> ChangeIsActive()
        {
            var products = await _context.Products.Where(x => x.IsActive && !x.IsDeleted).ToListAsync();

            foreach (var product in products)
            {
                product.IsActive = false;
            }

            await _context.SaveChangesAsync();

            return Ok("Changed");
        }
        
    }
}
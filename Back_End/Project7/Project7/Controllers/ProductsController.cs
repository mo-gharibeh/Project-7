using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project7.Models;

namespace Project7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ProductsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public IActionResult  GetProducts()
        {
            var product = _context.Products.ToList();
            return Ok(product);
        }


        [Route("category/{id}")]
        [HttpGet]
        public IActionResult GetProductById(int id)
        {


            var products = _context.Products.Where(c => c.CategoryId == id).ToList();

            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok (product);
        }


        
        // GET: api/Products/priceRange?minPrice=0&maxPrice=6
        [HttpGet("priceRange")]
        public IActionResult GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            var products = _context.Products
                                  .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                                  .ToList();

            if (products.Count == 0)
            {
                return NotFound(new { message = "No products found within the specified price range." });
            }

            return Ok(products);
        }

        // GET: api/Products/search?query={query}
        [Route("search")]
        [HttpGet]
        public IActionResult SearchProducts(string query)
        {
            var products = _context.Products
                                   .Where(p => p.ProductName.Contains(query))
                                   .ToList();

            if (products.Count == 0)
            {
                return NotFound(new { message = "No products found matching the search criteria." });
            }

            return Ok(products);
        }







    }
}
        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutProduct(int id, Product product)
        //{
        //    if (id != product.ProductId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(product).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProductExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Product>> PostProduct(Product product)
        //{
        //    _context.Products.Add(product);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        //}

        // DELETE: api/Products/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProduct(int id)
        //{
        //    var product = await _context.Products.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Products.Remove(product);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool ProductExists(int id)
        //{
        //    return _context.Products.Any(e => e.ProductId == id);
        //}

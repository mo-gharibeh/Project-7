﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project7.Models;

namespace Project7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CategoriesController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        
        [HttpGet]
        public IActionResult GetCategory()
        {
            var categories = _context.Categories.ToList();
            return Ok(categories);
        }



        // GET: api/Categories/5
        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var category =  _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

    }
}
        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCategory(int id, Category category)
        //{
        //    if (id != category.CategoryId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(category).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CategoryExists(id))
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

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Category>> PostCategory(Category category)
        //{
        //    _context.Categories.Add(category);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);
        //}

        // DELETE: api/Categories/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCategory(int id)
        //{
        //    var category = await _context.Categories.FindAsync(id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Categories.Remove(category);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool CategoryExists(int id)
        //{
        //    return _context.Categories.Any(e => e.CategoryId == id);
        //}

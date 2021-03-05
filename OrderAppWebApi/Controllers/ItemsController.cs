﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderAppWebApi.Data;
using OrderAppWebApi.Models;

namespace OrderAppWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly AppDbContext _context; //DB Context Instance

        public ItemsController(AppDbContext context)//instance with one parameter
        {
            _context = context;
        }

        // GET: api/Items
        [HttpGet]//Get All must add data to url:.../api/items
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            return await _context.Items.
                            ToListAsync();
        }

        // GET: api/Items/5
        [HttpGet("{id}")]// Get by PK
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _context.Items
                //.Include(c => c.Customer)
                .SingleOrDefaultAsync(o => o.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }
        //Update/ Change
        // PUT: api/Items/5  
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // Add/ Create
        // POST: api/Items 
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.Id }, item);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return item;
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}

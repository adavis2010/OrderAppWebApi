using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderAppWebApi.Data;
using OrderAppWebApi.Models;

namespace OrderAppWebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context) {
            _context = context;
        }


        // GET: api/Orders(GET ALL ORDERS)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders() {
            return await _context.Orders
                            .Include(c => c.Customer)//Do these to fill FK (only works on read operation)
                            .ToListAsync();
        }



        // GET: api/Orders (RETURN ALL ORDERS IN PROPOSED STATUS)
        [HttpGet("proposed")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersInProposedStatus() {
            return await _context.Orders
                            .Include(c => c.Customer)//Do these to fill FK (only works on read operation)
                            .Where(o => o.Status == "PROPOSED")
                            .ToListAsync();   

           

        }

        // PUT: api/Orders/Edit/5 = id number of order
        [HttpPut("edit/{id}")] //Add method on status
        public async Task<IActionResult> SetOrderStatusToEdit(int id) {
            //whatever gets passed in on url gets passed in on this method
            var order = await _context.Orders.FindAsync(id);
            if (order == null) {
                return NotFound();
            }
            order.Status = "EDIT"; // Set property to string Edit
            return await PutOrder(order.Id, order);
        }


        //PUT: api/orders/proposed/5 = id number of order PROPOSE
        [HttpPut("proposed/{id}")] //Add method on status
        public async Task<IActionResult> SetOrderStatusToProposed(int id) {
            //whatever gets passed in on url gets passed in on this method
            var order = await _context.Orders.FindAsync(id);
            if (order == null) {
                return NotFound();
            }
            order.Status = (order.Total <= 100) ? "FINAL" : "PROPOSED"; // used ternary operator
            return await PutOrder(order.Id, order); // Set property to string Edit
            

        }
        //PUT: api/orders//5 = id number of order FINAL
        [HttpPut("final/{id}")] //Add method on status
        public async Task<IActionResult> SetOrderStatusToFinal(int id) {
            //whatever gets passed in on url gets passed in on this method
            var order = await _context.Orders.FindAsync(id);
            if (order == null) {
                return NotFound();
            }
            
            return await PutOrder(order.Id, order);
        }


        // GET: api/Orders/5
        [HttpGet("{id}")] //Get by PK
        public async Task<ActionResult<Order>> GetOrder(int id) {
            var order = await _context.Orders
                        .Include(c => c.Customer) //Do these to fill FK ( only works on read operations)
                        .Include(l => l.Orderlines)// include = talks about collection I am working with
                        .ThenInclude(i => i.Item) // Then includes item instance in orderlines
                        .Include(s => s.Salesperson)
                        .SingleOrDefaultAsync(o => o.Id == id);

            if (order == null) {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order) {
            if (id != order.Id) {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!OrderExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order) {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id) {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private bool OrderExists(int id) {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}

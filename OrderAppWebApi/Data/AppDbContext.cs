using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderAppWebApi.Models;

namespace OrderAppWebApi.Data {
    public class AppDbContext : DbContext {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) {
        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Orderline> Orderlines { get; set; }
        public DbSet<Salesperson> Salespersons { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Customer>(e => {     //makes value unique
                e.HasIndex(c => c.Code).IsUnique(true);

            });
    }

        public DbSet<OrderAppWebApi.Models.Orderline> Orderline { get; set; }

        public DbSet<OrderAppWebApi.Models.Salesperson> Salesperson { get; set; }
    }
}

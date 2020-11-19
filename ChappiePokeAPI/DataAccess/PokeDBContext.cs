using EntityModels.EntityModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChappiePokeAPI.DataAccess
{
    public class PokeDBContext : DbContext
    {
        public PokeDBContext(DbContextOptions<PokeDBContext> options) : base(options)
        {

        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ImageGroup> ImageGroups { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<Pull> Pulls { get; set; }
        public DbSet<SaleOrderProduct> SaleOrderProducts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RegisterCode> RegisterCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

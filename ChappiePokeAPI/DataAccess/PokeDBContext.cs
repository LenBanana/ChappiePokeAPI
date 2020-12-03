using EntityModels.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        //public DbSet<Group> Groups { get; set; }
        //public DbSet<Image> Images { get; set; }
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
            builder.Entity<User>(e =>
            {
                e.HasKey(x => x.UserID);
                e.HasMany(x => x.Orders).WithOne(x => x.User).HasForeignKey(x => x.OrderID);
                e.HasMany(x => x.Customers).WithOne(x => x.User).HasForeignKey(x => x.CustomerID);
            });
            builder.Entity<Customer>(e =>
            {
                e.HasKey(x => x.CustomerID);
                //e.HasOne(x => x.BillingAddress).WithMany(x => x.Customers).HasForeignKey(x => x.BillingAddressAddressID);
                //e.HasOne(x => x.ShippingAddress).WithMany(x => x.Customers).HasForeignKey(x => x.ShippingAddressAddressID);
            });
            builder.Entity<Address>(e =>
            {
                e.HasKey(x => x.AddressID);
                //e.HasMany(x => x.Customers).WithOne(x => x.BillingAddress).HasForeignKey(x => x.BillingAddressAddressID);
                //e.HasMany(x => x.Customers).WithOne(x => x.ShippingAddress).HasForeignKey(x => x.ShippingAddressAddressID);
            });
            builder.Entity<Card>(e =>
            {
                e.HasKey(x => x.CardID);
                e.HasOne(x => x.Product).WithMany(x => x.Cards).HasForeignKey(x => x.ProductID);
            });
            builder.Entity<Product>(e =>
            {
                e.HasKey(x => x.ProductID);
                e.HasMany(x => x.ProductGroups).WithOne(x => x.Product).HasForeignKey(x => x.ProductGroupID);
            });
            builder.Entity<ProductGroup>(e =>
            {
                e.HasKey(x => x.ProductGroupID);
                e.HasOne(x => x.Product).WithMany(x => x.ProductGroups).HasForeignKey(x => x.ProductID);
                //e.HasOne(x => x.Group).WithOne(x => x.ProductGroup);
            });
            //builder.Entity<Group>(e =>
            //{
            //    e.HasKey(x => x.GroupID);
            //    e.HasOne(x => x.ProductGroup).WithOne(x => x.Group);
            //    e.HasAlternateKey(x => x.GroupName);
            //});

            //base.OnModelCreating(builder);
        }
    }
}

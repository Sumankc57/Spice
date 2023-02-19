using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Spice.Models;
using Spice.Models.TableModel;

namespace Spice.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }
        public DbSet<MenuItem> MenuItem { get; set; }
        public DbSet<Coupon> Coupon { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

        public DbSet<TableBooking> TablesBooking{ get; set; }
        public DbSet<TableType> TableTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);
            model.Entity<TableBooking>().HasOne(a => a.tableType)
                .WithOne(t => t.tableBooking)
                .HasForeignKey<TableBooking>(f => f.TypeID);

            model.Entity<TableType>()
               .Property(x => x.IsBusy)
               .HasDefaultValue(false);

            model.Entity<TableBooking>()
                .Property(x => x.Status)
                .HasDefaultValue("pending");


        }


    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketPortal09.Models;
using System.Reflection.Emit;

namespace TicketPortal09.Data
{
    public class TicketDbContext : IdentityDbContext<IdentityUser>
    {
        public TicketDbContext(DbContextOptions<TicketDbContext> options)
        : base(options)
        {
            //modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(p => new { p.LoginProvider, p.ProviderKey });
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> Subcategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            


        }
    }

}

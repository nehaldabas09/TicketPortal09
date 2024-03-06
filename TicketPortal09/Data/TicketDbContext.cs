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
            //modelBuilder.Entity<ApplicationUserLogin<string>>().HasKey(p => new { p.LoginProvider, p.ProviderKey });
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> Subcategories { get; set; }

        public DbSet<Remark> Remarks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            base.OnModelCreating(modelBuilder);

/*            modelBuilder.Entity<Remark>().HasKey(r=>r.Id);
*/
            modelBuilder.Entity<Ticket>()
                    .HasOne(t => t.Agent)
                    .WithMany(a => a.AgentTickets)
                    .HasForeignKey(t => t.AgentId)
                    .IsRequired(false);

            modelBuilder.Entity<Ticket>()
                  .HasOne(t => t.User)
                  .WithMany(a => a.UserTickets)
                  .HasForeignKey(t => t.UserId)
                  .IsRequired(false);

            modelBuilder.Entity<Ticket>()
              .HasOne(t => t.SubCategory)
              .WithMany(a => a.Tickets)
              .HasForeignKey(t => t.SubCategoryId)
              .IsRequired(false);

           /* modelBuilder.Entity<Ticket>()
               .HasOne(t => t.Category)                     
                  .WithMany(c => c.Tickets)                    
                 .HasForeignKey(t => t.CategoryId)           
                      .IsRequired(false);*/
        }
    }

}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuoteWall.Models;

namespace QuoteWall.Data
{
    public class ApplicationDbContext: IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Quote> Quotes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Configure relationships and constraints if needed
            builder.Entity<User>()
                .HasOne(u => u.Quotes)
                .WithOne()
                .HasForeignKey<User>(u => u.QuoteId)
                .OnDelete(DeleteBehavior.SetNull);
        }

    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MvcFPTBook.Areas.Identity.Data;

public class MvcFPTBookIdentityDbContext : IdentityDbContext<IdentityUser>
{
    public MvcFPTBookIdentityDbContext(DbContextOptions<MvcFPTBookIdentityDbContext> options)
        : base(options)
    {
    }

     protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MvcFPTBook.Models.Book>()
            .Property(p => p.Price).HasColumnType("decimal(18,4)");
        }

        public DbSet<MvcFPTBook.Models.Author> Author { get; set; } = default!;

        public DbSet<MvcFPTBook.Models.Publisher>? Publisher { get; set; }

        public DbSet<MvcFPTBook.Models.Category>? Category { get; set; }

        public DbSet<MvcFPTBook.Models.Book>? Book { get; set; }

        public DbSet<MvcFPTBook.Models.OrderDetail>? OrderDetail { get; set; }

        public DbSet<MvcFPTBook.Models.Order>? Order { get; set; }
    
}

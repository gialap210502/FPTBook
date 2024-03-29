using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcFPTBook.Areas.Identity.Data;

namespace MvcFPTBook.Areas.Identity.Data;

public class MvcFPTBookIdentityDbContext : IdentityDbContext<BookUser>
{
    public MvcFPTBookIdentityDbContext(DbContextOptions<MvcFPTBookIdentityDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<MvcFPTBook.Models.Book>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,4)");
        modelBuilder
            .Entity<MvcFPTBook.Models.Order>()
            .Property(p => p.Total)
            .HasColumnType("decimal(18,4)");
    }

    public DbSet<MvcFPTBook.Models.Author> Author { get; set; } = default!;

    public DbSet<MvcFPTBook.Models.Publisher>? Publisher { get; set; }

    public DbSet<MvcFPTBook.Models.Category>? Category { get; set; }

    public DbSet<MvcFPTBook.Models.Book>? Book { get; set; }

    public DbSet<MvcFPTBook.Models.OrderDetail>? OrderDetail { get; set; }

    public DbSet<MvcFPTBook.Models.Order>? Order { get; set; }

    public DbSet<MvcFPTBook.Areas.Identity.Data.BookUser> BookUser { get; set; }
}

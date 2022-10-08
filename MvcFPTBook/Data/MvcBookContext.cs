using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcFPTBook.Models;

    public class MvcBookContext : DbContext
    {
        public MvcBookContext (DbContextOptions<MvcBookContext> options)
            : base(options)
        {
        }

        public DbSet<MvcFPTBook.Models.Author> Author { get; set; } = default!;

        public DbSet<MvcFPTBook.Models.Publisher>? Publisher { get; set; }

        public DbSet<MvcFPTBook.Models.Category>? Category { get; set; }

        public DbSet<MvcFPTBook.Models.User>? User { get; set; }

        public DbSet<MvcFPTBook.Models.Book>? Book { get; set; }

        public DbSet<MvcFPTBook.Models.OrderDetail>? OrderDetail { get; set; }

        public DbSet<MvcFPTBook.Models.Order>? Order { get; set; }
    }

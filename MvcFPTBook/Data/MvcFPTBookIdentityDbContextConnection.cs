using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcFPTBook.Models;

    public class MvcFPTBookIdentityDbContextConnection : DbContext
    {
        public MvcFPTBookIdentityDbContextConnection (DbContextOptions<MvcFPTBookIdentityDbContextConnection> options)
            : base(options)
        {
        }

        public DbSet<MvcFPTBook.Models.Order> Order { get; set; } = default!;
    }

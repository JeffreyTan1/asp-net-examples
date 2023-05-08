using System;
using System.Collections.Generic;
using AdminWebsite.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminWebsite.Data;

public partial class McbaContext : DbContext
{
    public McbaContext(DbContextOptions<McbaContext> options)
        : base(options)
    {
    }
    
    public DbSet<Customer> Customer { get; set; }

    public DbSet<Login> Login { get; set; }

    public DbSet<BillPay> BillPay { get; set; }
}

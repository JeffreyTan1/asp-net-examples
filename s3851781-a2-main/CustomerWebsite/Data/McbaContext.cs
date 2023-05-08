using System;
using System.Collections.Generic;
using CustomerWebsite.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerWebsite.Data;

public partial class McbaContext : DbContext
{
    public McbaContext(DbContextOptions<McbaContext> options)
        : base(options)
    {
    }

    public DbSet<Account> Account { get; set; }

    public DbSet<Customer> Customer { get; set; }

    public DbSet<Login> Login { get; set; }

    public DbSet<Transaction> Transaction { get; set; }

    public DbSet<BillPay> BillPay { get; set; }

    public DbSet<Payee> Payee { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasOne(d => d.Account).WithMany(p => p.AccountTransactions)
                .HasForeignKey(d => d.AccountNumber)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.DestinationAccount).WithMany(p => p.DestinationAccountTransactions)
                .HasForeignKey(d => d.DestinationAccountNumber)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.Property(e => e.DestinationAccountNumber).IsRequired(false);
            entity.Property(e => e.Comment).IsRequired(false);

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

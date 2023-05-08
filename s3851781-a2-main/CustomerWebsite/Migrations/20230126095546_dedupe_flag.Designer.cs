﻿// <auto-generated />
using System;
using CustomerWebsite.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CustomerWebsite.Migrations
{
    [DbContext(typeof(McbaContext))]
    [Migration("20230126095546_dedupe_flag")]
    partial class dedupeflag
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CustomerWebsite.Models.Account", b =>
                {
                    b.Property<int>("AccountNumber")
                        .HasMaxLength(4)
                        .HasColumnType("int");

                    b.Property<string>("AccountType")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.HasKey("AccountNumber");

                    b.HasIndex("CustomerID");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("CustomerWebsite.Models.BillPay", b =>
                {
                    b.Property<int>("BillPayID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BillPayID"));

                    b.Property<int>("AccountNumber")
                        .HasColumnType("int");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<decimal>("Amount")
                        .HasColumnType("money");

                    b.Property<bool>("Paid")
                        .HasColumnType("bit");

                    b.Property<int>("PayeeID")
                        .HasColumnType("int");

                    b.Property<string>("Period")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<DateTime>("ScheduleTimeUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("BillPayID");

                    b.HasIndex("AccountNumber");

                    b.HasIndex("PayeeID");

                    b.ToTable("BillPay");
                });

            modelBuilder.Entity("CustomerWebsite.Models.Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .HasMaxLength(4)
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("City")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Mobile")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PostCode")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("State")
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("TFN")
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.HasKey("CustomerID");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("CustomerWebsite.Models.Login", b =>
                {
                    b.Property<string>("LoginID")
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<bool>("Blocked")
                        .HasColumnType("bit");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(94)
                        .HasColumnType("nvarchar(94)");

                    b.HasKey("LoginID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Login");
                });

            modelBuilder.Entity("CustomerWebsite.Models.Payee", b =>
                {
                    b.Property<int>("PayeeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PayeeID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.Property<string>("PostCode")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.HasKey("PayeeID");

                    b.ToTable("Payee");
                });

            modelBuilder.Entity("CustomerWebsite.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionID"));

                    b.Property<int>("AccountNumber")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("money");

                    b.Property<string>("Comment")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int?>("DestinationAccountNumber")
                        .HasColumnType("int");

                    b.Property<DateTime>("TransactionTimeUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.HasKey("TransactionID");

                    b.HasIndex("AccountNumber");

                    b.HasIndex("DestinationAccountNumber");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("CustomerWebsite.Models.Account", b =>
                {
                    b.HasOne("CustomerWebsite.Models.Customer", "Customer")
                        .WithMany("Accounts")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("CustomerWebsite.Models.BillPay", b =>
                {
                    b.HasOne("CustomerWebsite.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CustomerWebsite.Models.Payee", "Payee")
                        .WithMany()
                        .HasForeignKey("PayeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Payee");
                });

            modelBuilder.Entity("CustomerWebsite.Models.Login", b =>
                {
                    b.HasOne("CustomerWebsite.Models.Customer", "Customer")
                        .WithMany("Logins")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("CustomerWebsite.Models.Transaction", b =>
                {
                    b.HasOne("CustomerWebsite.Models.Account", "Account")
                        .WithMany("AccountTransactions")
                        .HasForeignKey("AccountNumber")
                        .IsRequired();

                    b.HasOne("CustomerWebsite.Models.Account", "DestinationAccount")
                        .WithMany("DestinationAccountTransactions")
                        .HasForeignKey("DestinationAccountNumber");

                    b.Navigation("Account");

                    b.Navigation("DestinationAccount");
                });

            modelBuilder.Entity("CustomerWebsite.Models.Account", b =>
                {
                    b.Navigation("AccountTransactions");

                    b.Navigation("DestinationAccountTransactions");
                });

            modelBuilder.Entity("CustomerWebsite.Models.Customer", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Logins");
                });
#pragma warning restore 612, 618
        }
    }
}

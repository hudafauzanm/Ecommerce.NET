﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Razor.Data;

namespace Razor.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200217035255_change_column_purchase")]
    partial class change_column_purchase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Razor.Models.Cart", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("published_at")
                        .HasColumnType("datetime2");

                    b.Property<double>("total")
                        .HasColumnType("float");

                    b.HasKey("id");

                    b.ToTable("Cart");
                });

            modelBuilder.Entity("Razor.Models.Item", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("item_name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("price")
                        .HasColumnType("float");

                    b.Property<DateTime>("published_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("rating")
                        .HasColumnType("int");

                    b.Property<string>("url_img")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("Razor.Models.Purchase", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("User_id")
                        .HasColumnType("int");

                    b.Property<string>("alamat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("kodepos")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nama_pemesan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("payment_type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("published_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("transaction_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("User_id");

                    b.HasIndex("transaction_id");

                    b.ToTable("Purchase");
                });

            modelBuilder.Entity("Razor.Models.Transaction_Details", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("_account")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_actions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fraud_status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("gross_amount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("merchant_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("order_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("signature_key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("status_code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("status_message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("transaction_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("transaction_status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("transaction_time")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.ToTable("Transaction_Details");
                });

            modelBuilder.Entity("Razor.Models.Transaksi", b =>
                {
                    b.Property<int>("Item_id")
                        .HasColumnType("int");

                    b.Property<int>("Cart_id")
                        .HasColumnType("int");

                    b.Property<int>("User_id")
                        .HasColumnType("int");

                    b.HasKey("Item_id", "Cart_id", "User_id");

                    b.HasIndex("Cart_id");

                    b.HasIndex("User_id");

                    b.ToTable("Transaksi");
                });

            modelBuilder.Entity("Razor.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("published_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("role")
                        .HasColumnType("int");

                    b.Property<string>("username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Razor.Models.Purchase", b =>
                {
                    b.HasOne("Razor.Models.User", "User")
                        .WithMany("Purchases")
                        .HasForeignKey("User_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Razor.Models.Transaction_Details", "Transaction_Details")
                        .WithMany()
                        .HasForeignKey("transaction_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Razor.Models.Transaksi", b =>
                {
                    b.HasOne("Razor.Models.Cart", "Cart")
                        .WithMany("Transaksi")
                        .HasForeignKey("Cart_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Razor.Models.Item", "Item")
                        .WithMany("Transaksi")
                        .HasForeignKey("Item_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Razor.Models.User", "User")
                        .WithMany("Transaksi")
                        .HasForeignKey("User_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
﻿// <auto-generated />
using System;
using Adnc.Infr.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Adnc.Ord.Migrations.Migrations
{
    [DbContext(typeof(AdncDbContext))]
    partial class AdncDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Adnc.Ord.Domain.Entities.Order", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<long?>("CreateBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<string>("Remark")
                        .HasColumnType("varchar(64) CHARACTER SET utf8mb4")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("RowVersion")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("Adnc.Ord.Domain.Entities.OrderItem", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<long>("OrderId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItem");
                });

            modelBuilder.Entity("Adnc.Ord.Domain.Entities.Order", b =>
                {
                    b.OwnsOne("Adnc.Ord.Domain.Entities.OrderReceiver", "Receiver", b1 =>
                        {
                            b1.Property<long>("OrderId")
                                .HasColumnType("bigint");

                            b1.Property<string>("Address")
                                .IsRequired()
                                .HasColumnName("ReceiverAddress")
                                .HasColumnType("varchar(64) CHARACTER SET utf8mb4")
                                .HasMaxLength(64);

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnName("ReceiverName")
                                .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                                .HasMaxLength(16);

                            b1.Property<string>("Phone")
                                .IsRequired()
                                .HasColumnName("ReceiverPhone")
                                .HasColumnType("varchar(11) CHARACTER SET utf8mb4")
                                .HasMaxLength(11);

                            b1.HasKey("OrderId");

                            b1.ToTable("Order");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.OwnsOne("Adnc.Ord.Domain.Entities.OrderStatus", "Status", b1 =>
                        {
                            b1.Property<long>("OrderId")
                                .HasColumnType("bigint");

                            b1.Property<string>("ChangesReason")
                                .HasColumnName("StatusChangesReason")
                                .HasColumnType("varchar(32) CHARACTER SET utf8mb4")
                                .HasMaxLength(32);

                            b1.Property<int>("Code")
                                .HasColumnName("StatusCode")
                                .HasColumnType("int");

                            b1.HasKey("OrderId");

                            b1.ToTable("Order");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });
                });

            modelBuilder.Entity("Adnc.Ord.Domain.Entities.OrderItem", b =>
                {
                    b.HasOne("Adnc.Ord.Domain.Entities.Order", null)
                        .WithMany("Items")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Adnc.Ord.Domain.Entities.OrderItemProduct", "Product", b1 =>
                        {
                            b1.Property<long>("OrderItemId")
                                .HasColumnType("bigint");

                            b1.Property<long>("Id")
                                .HasColumnName("ProductId")
                                .HasColumnType("bigint");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnName("ProductName")
                                .HasColumnType("varchar(64) CHARACTER SET utf8mb4")
                                .HasMaxLength(64);

                            b1.Property<decimal>("Price")
                                .HasColumnName("ProductPrice")
                                .HasColumnType("decimal(18,4)");

                            b1.HasKey("OrderItemId");

                            b1.ToTable("OrderItem");

                            b1.WithOwner()
                                .HasForeignKey("OrderItemId");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}

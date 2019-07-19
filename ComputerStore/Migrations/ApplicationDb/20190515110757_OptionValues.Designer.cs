﻿// <auto-generated />
using ComputerStore.Models.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ComputerStore.Migrations.ApplicationDb
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190515110757_OptionValues")]
    partial class OptionValues
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ComputerStore.Models.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ComputerStore.Models.FilterOption", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<long>("SubcategoryID");

                    b.HasKey("Id");

                    b.HasIndex("SubcategoryID");

                    b.ToTable("FilterOptions");
                });

            modelBuilder.Entity("ComputerStore.Models.FilterOptionValue", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("FilterOptionID");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("FilterOptionID");

                    b.ToTable("FilterOptionValues");
                });

            modelBuilder.Entity("ComputerStore.Models.Product", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CategoryID");

                    b.Property<string>("Description");

                    b.Property<string>("Image");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<decimal>("Price");

                    b.Property<long>("SubcategoryID");

                    b.HasKey("Id");

                    b.HasIndex("CategoryID");

                    b.HasIndex("SubcategoryID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ComputerStore.Models.ProductProperty", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<long>("ProductID");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ProductID");

                    b.ToTable("ProductProperties");
                });

            modelBuilder.Entity("ComputerStore.Models.Subcategory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CategoryID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("CategoryID");

                    b.ToTable("Subcategories");
                });

            modelBuilder.Entity("ComputerStore.Models.FilterOption", b =>
                {
                    b.HasOne("ComputerStore.Models.Subcategory", "Subcategory")
                        .WithMany("FilterOptions")
                        .HasForeignKey("SubcategoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ComputerStore.Models.FilterOptionValue", b =>
                {
                    b.HasOne("ComputerStore.Models.FilterOption", "FilterOption")
                        .WithMany("Values")
                        .HasForeignKey("FilterOptionID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ComputerStore.Models.Product", b =>
                {
                    b.HasOne("ComputerStore.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ComputerStore.Models.Subcategory", "Subcategory")
                        .WithMany("Products")
                        .HasForeignKey("SubcategoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ComputerStore.Models.ProductProperty", b =>
                {
                    b.HasOne("ComputerStore.Models.Product", "Product")
                        .WithMany("Properties")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ComputerStore.Models.Subcategory", b =>
                {
                    b.HasOne("ComputerStore.Models.Category", "Category")
                        .WithMany("Subcategories")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

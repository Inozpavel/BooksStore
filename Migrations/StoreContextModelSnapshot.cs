﻿using System;
using BooksStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BooksStore.Migrations
{
    [DbContext(typeof(StoreContext))]
    internal class StoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("BooksStore.Models.Author", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<string>("Description")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Authors");
            });

            modelBuilder.Entity("BooksStore.Models.Book", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<int>("AuthorId")
                    .HasColumnType("int");

                b.Property<int>("CategoryId")
                    .HasColumnType("int");

                b.Property<string>("Description")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<decimal>("Price")
                    .HasColumnType("decimal(18,2)");

                b.Property<int>("ProductImageId")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("AuthorId");

                b.HasIndex("CategoryId");

                b.ToTable("Books");
            });

            modelBuilder.Entity("BooksStore.Models.CartItem", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<int>("BookId")
                    .HasColumnType("int");

                b.Property<int>("Count")
                    .HasColumnType("int");

                b.Property<int>("UserId")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("BookId");

                b.HasIndex("UserId");

                b.ToTable("CartsItems");
            });

            modelBuilder.Entity("BooksStore.Models.Category", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<string>("Description")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Categories");
            });

            modelBuilder.Entity("BooksStore.Models.ProductImage", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<int>("BookId")
                    .HasColumnType("int");

                b.Property<byte[]>("Image")
                    .HasColumnType("varbinary(max)");

                b.HasKey("Id");

                b.HasIndex("BookId");

                b.ToTable("Images");
            });

            modelBuilder.Entity("BooksStore.Models.Role", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Roles");
            });

            modelBuilder.Entity("BooksStore.Models.User", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<string>("Email")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Password")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Phone")
                    .HasColumnType("nvarchar(max)");

                b.Property<DateTime>("RegistrationTime")
                    .HasColumnType("datetime2");

                b.Property<int>("RoleId")
                    .HasColumnType("int");

                b.Property<string>("SecondName")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.HasIndex("RoleId");

                b.ToTable("Users");
            });

            modelBuilder.Entity("BooksStore.Models.Book", b =>
            {
                b.HasOne("BooksStore.Models.Author", "Author")
                    .WithMany()
                    .HasForeignKey("AuthorId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("BooksStore.Models.Category", "Category")
                    .WithMany()
                    .HasForeignKey("CategoryId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Author");

                b.Navigation("Category");
            });

            modelBuilder.Entity("BooksStore.Models.CartItem", b =>
            {
                b.HasOne("BooksStore.Models.Book", "Book")
                    .WithMany()
                    .HasForeignKey("BookId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("BooksStore.Models.User", "User")
                    .WithMany("CartsItems")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Book");

                b.Navigation("User");
            });

            modelBuilder.Entity("BooksStore.Models.ProductImage", b =>
            {
                b.HasOne("BooksStore.Models.Book", null)
                    .WithMany("BookImages")
                    .HasForeignKey("BookId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("BooksStore.Models.User", b =>
            {
                b.HasOne("BooksStore.Models.Role", "Role")
                    .WithMany("Users")
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Role");
            });

            modelBuilder.Entity("BooksStore.Models.Book", b => { b.Navigation("BookImages"); });

            modelBuilder.Entity("BooksStore.Models.Role", b => { b.Navigation("Users"); });

            modelBuilder.Entity("BooksStore.Models.User", b => { b.Navigation("CartsItems"); });
#pragma warning restore 612, 618
        }
    }
}
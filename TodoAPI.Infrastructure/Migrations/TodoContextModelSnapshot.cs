﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoAPI.DbContext.Contexts;


#nullable disable

namespace TodoAPI.Migrations
{
    [DbContext(typeof(TodoContext))]
    partial class TodoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TodoAPI.Entities.TodoEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Done")
                        .HasColumnType("bit");

                    b.Property<string>("TimeStamp")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("UserAuthenticationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserAuthenticationId");

                    b.ToTable("Todos", (string)null);
                });

            modelBuilder.Entity("TodoAPI.Entities.UserAuthenticationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("UserAuthentications", (string)null);
                });

            modelBuilder.Entity("TodoAPI.Entities.TodoEntity", b =>
                {
                    b.HasOne("TodoAPI.Entities.UserAuthenticationEntity", "UserAuthentication")
                        .WithMany("Todos")
                        .HasForeignKey("UserAuthenticationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserAuthentication");
                });

            modelBuilder.Entity("TodoAPI.Entities.UserAuthenticationEntity", b =>
                {
                    b.Navigation("Todos");
                });
#pragma warning restore 612, 618
        }
    }
}

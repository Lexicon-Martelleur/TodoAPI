﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoAPI.DBContext;

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

                    b.HasKey("Id");

                    b.ToTable("Todos");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Author = "author1",
                            Description = "description1",
                            Done = false,
                            TimeStamp = "1724162544",
                            Title = "title1"
                        },
                        new
                        {
                            Id = 2,
                            Author = "author2",
                            Description = "description2",
                            Done = false,
                            TimeStamp = "1724162544",
                            Title = "title2"
                        },
                        new
                        {
                            Id = 3,
                            Author = "author3",
                            Description = "description3",
                            Done = false,
                            TimeStamp = "1724162544",
                            Title = "title3"
                        },
                        new
                        {
                            Id = 4,
                            Author = "author4",
                            Description = "description4",
                            Done = false,
                            TimeStamp = "1724162544",
                            Title = "title4"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}

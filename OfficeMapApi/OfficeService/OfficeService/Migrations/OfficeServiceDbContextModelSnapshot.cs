﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OfficeService.Database;

namespace OfficeService.Migrations
{
    [DbContext(typeof(OfficeServiceDbContext))]
    partial class OfficeServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OfficeService.Database.Entities.DbOffice", b =>
                {
                    b.Property<int>("OfficeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Building")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("City")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("House")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<bool>("Obsolete")
                        .HasColumnType("bit");

                    b.Property<Guid>("OfficeGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("OfficeGUID")
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("PhoneNumber")
                        .HasColumnName("Phone_Number")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Street")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("OfficeId");

                    b.ToTable("Offices", "dbo");
                });

            modelBuilder.Entity("OfficeService.Database.Entities.DbSpace", b =>
                {
                    b.Property<int>("SpaceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MapId")
                        .HasColumnType("int");

                    b.Property<int>("OfficeId")
                        .HasColumnType("int");

                    b.Property<int>("ParentId")
                        .HasColumnType("int");

                    b.Property<Guid>("SpaceGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("SpaceGUID")
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.HasKey("SpaceId")
                        .HasName("PK__Spaces");

                    b.HasIndex("OfficeId");

                    b.HasIndex("ParentId");

                    b.ToTable("Spaces", "dbo");
                });

            modelBuilder.Entity("OfficeService.Database.Entities.DbSpace", b =>
                {
                    b.HasOne("OfficeService.Database.Entities.DbOffice", "Office")
                        .WithMany("Spaces")
                        .HasForeignKey("OfficeId")
                        .IsRequired();

                    b.HasOne("OfficeService.Database.Entities.DbSpace", "Parent")
                        .WithMany("InverseParent")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TMS.Infrastructure.Persistence;

#nullable disable

namespace TMS.Infrastructure.Migrations
{
    [DbContext(typeof(MainContext))]
    [Migration("20240407020804_StudentsAndTeachers")]
    partial class StudentsAndTeachers
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("StudentTeacher", b =>
                {
                    b.Property<Guid>("StudentsId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("TeachersId")
                        .HasColumnType("char(36)");

                    b.HasKey("StudentsId", "TeachersId");

                    b.HasIndex("TeachersId");

                    b.ToTable("StudentTeacher");
                });

            modelBuilder.Entity("TMS.Domain.Students.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<DateOnly>("BillDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("TMS.Domain.Students.Student", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("TMS.Domain.Teachers.Teacher", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<DateTime>("EndOfSubscription")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("JoinDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValue(new DateTime(2024, 4, 7, 2, 8, 4, 131, DateTimeKind.Utc).AddTicks(9136));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(26)
                        .HasColumnType("varchar(26)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)");

                    b.HasKey("Id");

                    b.ToTable("Teacher");
                });

            modelBuilder.Entity("StudentTeacher", b =>
                {
                    b.HasOne("TMS.Domain.Students.Student", null)
                        .WithMany()
                        .HasForeignKey("StudentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TMS.Domain.Teachers.Teacher", null)
                        .WithMany()
                        .HasForeignKey("TeachersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TMS.Domain.Students.Payment", b =>
                {
                    b.HasOne("TMS.Domain.Students.Student", null)
                        .WithMany("Payments")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("TMS.Domain.Students.Student", b =>
                {
                    b.Navigation("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}

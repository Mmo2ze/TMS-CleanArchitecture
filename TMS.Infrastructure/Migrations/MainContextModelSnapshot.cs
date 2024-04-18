﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TMS.Infrastructure.Persistence;

#nullable disable

namespace TMS.Infrastructure.Migrations
{
    [DbContext(typeof(MainContext))]
    partial class MainContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.InboxState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("Consumed")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("ConsumerId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("Delivered")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<long?>("LastSequenceNumber")
                        .HasColumnType("bigint");

                    b.Property<Guid>("LockId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("char(36)");

                    b.Property<int>("ReceiveCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("Received")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp(6)");

                    b.HasKey("Id");

                    b.HasAlternateKey("MessageId", "ConsumerId");

                    b.HasIndex("Delivered");

                    b.ToTable("InboxState");
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.OutboxMessage", b =>
                {
                    b.Property<long>("SequenceNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("SequenceNumber"));

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<Guid?>("ConversationId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("CorrelationId")
                        .HasColumnType("char(36)");

                    b.Property<string>("DestinationAddress")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<DateTime?>("EnqueueTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FaultAddress")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("Headers")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("InboxConsumerId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("InboxMessageId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("InitiatorId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("char(36)");

                    b.Property<string>("MessageType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("OutboxId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Properties")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("RequestId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ResponseAddress")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<DateTime>("SentTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("SourceAddress")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("SequenceNumber");

                    b.HasIndex("EnqueueTime");

                    b.HasIndex("ExpirationTime");

                    b.HasIndex("OutboxId", "SequenceNumber")
                        .IsUnique();

                    b.HasIndex("InboxMessageId", "InboxConsumerId", "SequenceNumber")
                        .IsUnique();

                    b.ToTable("OutboxMessage");
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.OutboxState", b =>
                {
                    b.Property<Guid>("OutboxId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("Delivered")
                        .HasColumnType("datetime(6)");

                    b.Property<long?>("LastSequenceNumber")
                        .HasColumnType("bigint");

                    b.Property<Guid>("LockId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp(6)");

                    b.HasKey("OutboxId");

                    b.HasIndex("Created");

                    b.ToTable("OutboxState");
                });

            modelBuilder.Entity("StudentTeacher", b =>
                {
                    b.Property<string>("StudentsId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("TeachersId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("StudentsId", "TeachersId");

                    b.HasIndex("TeachersId");

                    b.ToTable("StudentTeacher");
                });

            modelBuilder.Entity("TMS.Domain.Admins.Admin", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(26)
                        .HasColumnType("varchar(26)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.ToTable("Admins", (string)null);
                });

            modelBuilder.Entity("TMS.Domain.Assistants.Assistant", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Email")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(26)
                        .HasColumnType("varchar(26)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)");

                    b.Property<string>("TeacherId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.HasIndex("TeacherId");

                    b.ToTable("Assistants", (string)null);
                });

            modelBuilder.Entity("TMS.Domain.OutBox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Error")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("OccurredOn")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("ProcessedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("RetryCount")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages");
                });

            modelBuilder.Entity("TMS.Domain.Parents.Parent", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Email")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(26)
                        .HasColumnType("varchar(26)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.ToTable("Parents", (string)null);
                });

            modelBuilder.Entity("TMS.Domain.Students.Payment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("AssistantId")
                        .HasColumnType("varchar(255)");

                    b.Property<DateOnly>("BillDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("TeacherId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("AssistantId");

                    b.HasIndex("StudentId");

                    b.HasIndex("TeacherId");

                    b.ToTable("StudentPayments", (string)null);
                });

            modelBuilder.Entity("TMS.Domain.Students.Student", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ParentId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Phone")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("ParentId");

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.ToTable("Students", (string)null);
                });

            modelBuilder.Entity("TMS.Domain.Teachers.Teacher", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Email")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<DateOnly>("EndOfSubscription")
                        .HasColumnType("date");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(26)
                        .HasColumnType("varchar(26)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.ToTable("Teachers", (string)null);
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

            modelBuilder.Entity("TMS.Domain.Assistants.Assistant", b =>
                {
                    b.HasOne("TMS.Domain.Teachers.Teacher", null)
                        .WithMany("Assistants")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TMS.Domain.Students.Payment", b =>
                {
                    b.HasOne("TMS.Domain.Assistants.Assistant", null)
                        .WithMany()
                        .HasForeignKey("AssistantId");

                    b.HasOne("TMS.Domain.Students.Student", null)
                        .WithMany("Payments")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TMS.Domain.Teachers.Teacher", null)
                        .WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TMS.Domain.Students.Student", b =>
                {
                    b.HasOne("TMS.Domain.Parents.Parent", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.OwnsMany("TMS.Domain.Students.Attendance", "Attendances", b1 =>
                        {
                            b1.Property<string>("StudentId")
                                .HasColumnType("varchar(255)");

                            b1.Property<int>("Id")
                                .HasColumnType("int");

                            b1.Property<DateOnly>("Date")
                                .HasColumnType("date");

                            b1.Property<string>("Status")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.Property<string>("TeacherId")
                                .IsRequired()
                                .HasColumnType("varchar(255)");

                            b1.HasKey("StudentId", "Id");

                            b1.HasIndex("TeacherId");

                            b1.HasIndex("Date", "TeacherId", "StudentId")
                                .IsUnique();

                            b1.ToTable("Attendances", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("StudentId");

                            b1.HasOne("TMS.Domain.Teachers.Teacher", null)
                                .WithMany()
                                .HasForeignKey("TeacherId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();
                        });

                    b.Navigation("Attendances");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("TMS.Domain.Parents.Parent", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("TMS.Domain.Students.Student", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("TMS.Domain.Teachers.Teacher", b =>
                {
                    b.Navigation("Assistants");
                });
#pragma warning restore 612, 618
        }
    }
}

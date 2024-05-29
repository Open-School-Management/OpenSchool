﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ResourceManager.Infrastructure.Contexts;

#nullable disable

namespace ResourceManager.Infrastructure.Migrations
{
    [DbContext(typeof(ResourceManagerDbContext))]
    partial class ResourceManagerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("IntegrationEventLogs.IntegrationEventLogEntry", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("EventTypeName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<int>("TimesSent")
                        .HasColumnType("int");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("char(36)");

                    b.HasKey("EventId");

                    b.ToTable("integration_event_log", (string)null);
                });

            modelBuilder.Entity("ResourceManager.Domain.Entities.ActivityLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ActionType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("DestinationId")
                        .HasColumnType("char(36)");

                    b.Property<string>("NewValue")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("OldValue")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("ResourceId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("SourceId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("activity_log");
                });

            modelBuilder.Entity("ResourceManager.Domain.Entities.Config", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CreatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("char(255)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("CurrentUsage")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("DeletedBy")
                        .HasMaxLength(255)
                        .HasColumnType("char(255)");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<Guid?>("LastModifiedBy")
                        .HasMaxLength(255)
                        .HasColumnType("char(255)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("MaxCapacity")
                        .HasColumnType("bigint");

                    b.Property<long>("MaxFileSize")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("OwnerId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("resource_config");
                });

            modelBuilder.Entity("ResourceManager.Domain.Entities.Directory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CreatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("char(255)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("DeletedBy")
                        .HasMaxLength(255)
                        .HasColumnType("char(255)");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DuplicateNo")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<Guid?>("LastModifiedBy")
                        .HasMaxLength(255)
                        .HasColumnType("char(255)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<Guid?>("OwnerId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("varchar(512)");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("directory");
                });

            modelBuilder.Entity("ResourceManager.Domain.Entities.File", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CreatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("char(255)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("DeletedBy")
                        .HasMaxLength(255)
                        .HasColumnType("char(255)");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("DirectoryId")
                        .HasColumnType("char(36)");

                    b.Property<string>("FileExtension")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<Guid?>("LastModifiedBy")
                        .HasMaxLength(255)
                        .HasColumnType("char(255)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("OriginalFileName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<Guid?>("OwnerId")
                        .HasColumnType("char(36)");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("DirectoryId");

                    b.ToTable("file");
                });

            modelBuilder.Entity("ResourceManager.Domain.Entities.LockedDirectory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("DirectoryId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("EnabledLock")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("OwnerId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("DirectoryId")
                        .IsUnique();

                    b.ToTable("locked_directory");
                });

            modelBuilder.Entity("ResourceManager.Domain.Entities.RecycleBin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("OwnerId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ResourceId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ResourceType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("RestoredAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("recycle_Bin");
                });

            modelBuilder.Entity("ResourceManager.Domain.Entities.Directory", b =>
                {
                    b.HasOne("ResourceManager.Domain.Entities.Directory", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("ResourceManager.Domain.Entities.File", b =>
                {
                    b.HasOne("ResourceManager.Domain.Entities.Directory", "Directory")
                        .WithMany("Files")
                        .HasForeignKey("DirectoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Directory");
                });

            modelBuilder.Entity("ResourceManager.Domain.Entities.LockedDirectory", b =>
                {
                    b.HasOne("ResourceManager.Domain.Entities.Directory", "Directory")
                        .WithOne("LockedDirectory")
                        .HasForeignKey("ResourceManager.Domain.Entities.LockedDirectory", "DirectoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Directory");
                });

            modelBuilder.Entity("ResourceManager.Domain.Entities.Directory", b =>
                {
                    b.Navigation("Files");

                    b.Navigation("LockedDirectory");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VehicleGenius.Api.Models;
using VehicleGenius.Api.Services.Vehicles.VinAudit;

#nullable disable

namespace VehicleGenius.Api.Migrations
{
    [DbContext(typeof(VehicleGeniusDbContext))]
    [Migration("20230430130304_AddDatesToSummaryTemplate")]
    partial class AddDatesToSummaryTemplate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("VehicleGenius.Api.Models.Entities.SummaryTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Template")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("VinAuditDataVersion")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("SummaryTemplates");
                });

            modelBuilder.Entity("VehicleGenius.Api.Models.Entities.Vehicle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Vin")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<VinAuditData>("VinAuditData")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("VinAuditDataVersion")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Vehicles");
                });
#pragma warning restore 612, 618
        }
    }
}

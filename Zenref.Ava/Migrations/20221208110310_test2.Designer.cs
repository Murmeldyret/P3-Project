﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Zenref.Ava.Models;

#nullable disable

namespace Zenref.Ava.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20221208110310_test2")]
    partial class test2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("Zenref.Ava.Models.Reference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Author")
                        .HasColumnType("TEXT");

                    b.Property<string>("BookTitle")
                        .HasColumnType("TEXT");

                    b.Property<string>("Chapters")
                        .HasColumnType("TEXT");

                    b.Property<string>("Commentary")
                        .HasColumnType("TEXT");

                    b.Property<string>("Education")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ExamEvent")
                        .HasColumnType("TEXT");

                    b.Property<string>("Language")
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double?>("Match")
                        .HasColumnType("REAL");

                    b.Property<string>("OriReference")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("Pages")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PubType")
                        .HasColumnType("TEXT");

                    b.Property<string>("Publisher")
                        .HasColumnType("TEXT");

                    b.Property<string>("RefId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Season")
                        .HasColumnType("TEXT");

                    b.Property<string>("Semester")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Source")
                        .HasColumnType("TEXT");

                    b.Property<string>("Syllabus")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("Volume")
                        .HasColumnType("TEXT");

                    b.Property<int?>("YearRef")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("YearReport")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Reference", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Zenref.Ava.Models;

#nullable disable

namespace Zenref.Ava.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.10");

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

                    b.Property<string?>("DOI")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Edu")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExamEvent")
                        .HasColumnType("TEXT");

                    b.Property<string?>("ISBN")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Language")
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Match")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Pages")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PubType")
                        .HasColumnType("TEXT");

                    b.Property<string>("Publisher")
                        .HasColumnType("TEXT");

                    b.Property<string>("Season")
                        .HasColumnType("TEXT");

                    b.Property<string>("Semester")
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

                    b.Property<string>("OriReference")
                        .HasColumnType("STRING");

                    b.HasKey("Id");

                    b.ToTable("References");
                });

            modelBuilder.Entity("Zenref.Ava.Models.Spreadsheet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Spreadsheets");
                });
#pragma warning restore 612, 618
        }
    }
}

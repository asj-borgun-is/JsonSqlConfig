﻿// <auto-generated />
using System;
using JsonSqlConfigDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JsonSqlConfigDb.Migrations
{
    [DbContext(typeof(JsonSqlContext))]
    [Migration("20230628192500_Origin")]
    partial class Origin
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JsonSqlConfigDb.Model.JsonUnit", b =>
                {
                    b.Property<long>("JsonUnitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("JsonUnitId"));

                    b.Property<int>("CompositeType")
                        .HasColumnType("int");

                    b.Property<string>("Group")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)")
                        .HasDefaultValue("");

                    b.Property<int?>("Index")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<long?>("ParentId")
                        .HasColumnType("bigint");

                    b.Property<string>("Path")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)")
                        .HasDefaultValue("");

                    b.Property<int>("SimpleType")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.HasKey("JsonUnitId");

                    b.HasIndex("ParentId");

                    b.HasIndex(new[] { "Group", "Path" }, "IX_JsonUnits_Group_Path")
                        .IsUnique();

                    b.ToTable("JsonUnits", "JsonSqlConfig");
                });

            modelBuilder.Entity("JsonSqlConfigDb.Model.JsonUnit", b =>
                {
                    b.HasOne("JsonSqlConfigDb.Model.JsonUnit", "Parent")
                        .WithMany("Child")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("JsonSqlConfigDb.Model.JsonUnit", b =>
                {
                    b.Navigation("Child");
                });
#pragma warning restore 612, 618
        }
    }
}

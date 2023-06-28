﻿
using JsonSqlConfigDb.Model;
using Microsoft.EntityFrameworkCore;

namespace JsonSqlConfigDb
{
    public class JsonSqlConfigContext : DbContext
    {
        public DbSet<JsonUnit> JsonUnits { get; set; }

        public JsonSqlConfigContext(DbContextOptions<JsonSqlConfigContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<JsonUnit>().ToTable("JsonUnits", "JsonSqlConfig");
            modelbuilder.Entity<JsonUnit>().HasIndex(e => new { e.Group, e.Path }, "IX_JsonUnits_Group_Path").IsUnique();
            modelbuilder.Entity<JsonUnit>().Property(e => e.Name).HasMaxLength(128);
            modelbuilder.Entity<JsonUnit>().Property(e => e.Group).HasMaxLength(128).IsRequired().HasDefaultValue(string.Empty);
            modelbuilder.Entity<JsonUnit>().Property(e => e.Path).HasMaxLength(512).IsRequired().HasDefaultValue(string.Empty);
            modelbuilder.Entity<JsonUnit>().Property(e => e.Value).HasMaxLength(2048);
        }
    }
}


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

        public const string Schema = "JsonSqlConfig";
        public const string JsonUnitsTable = "JsonUnits";

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<JsonUnit>().ToTable(JsonUnitsTable, Schema);
            modelbuilder.Entity<JsonUnit>().Property(e => e.Name).HasMaxLength(128);
            modelbuilder.Entity<JsonUnit>().Property(e => e.Path).HasMaxLength(512);
            modelbuilder.Entity<JsonUnit>().Property(e => e.Value).HasMaxLength(2048);
        }
    }
}

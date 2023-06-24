
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

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<JsonUnit>().ToTable(nameof(JsonUnit), Schema);
            modelbuilder.Entity<JsonUnit>().Property(ju => ju.Name).HasMaxLength(128);
            modelbuilder.Entity<JsonUnit>().Property(ju => ju.Path).HasMaxLength(512);
            modelbuilder.Entity<JsonUnit>().Property(ju => ju.Value).HasMaxLength(2048);
        }
    }
}

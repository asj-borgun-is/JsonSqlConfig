
using JsonSqlConfigDb.Model;
using Microsoft.EntityFrameworkCore;

namespace JsonSqlConfigDb
{
    public class JsonSqlContext : DbContext
    {
        public DbSet<JsonUnit> JsonUnits { get; set; }

        public JsonSqlContext(DbContextOptions<JsonSqlContext> options)
            : base(options) { }

        public static Action<DbContextOptionsBuilder> OptionsAction { get; set; }
        public static DbContextOptions<JsonSqlContext> Options { 
            get 
            {
                var builder = new DbContextOptionsBuilder<JsonSqlContext>();
                OptionsAction(builder);
                return builder.Options;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<JsonUnit>().ToTable("JsonUnits", "JsonSqlConfig");
            modelbuilder.Entity<JsonUnit>().HasIndex(e => new { e.Path }, "IX_JsonUnits_Path").IsUnique();
            modelbuilder.Entity<JsonUnit>().Property(e => e.Name).HasMaxLength(128);
            modelbuilder.Entity<JsonUnit>().Property(e => e.Group).HasMaxLength(128).IsRequired().HasDefaultValue(string.Empty);
            modelbuilder.Entity<JsonUnit>().Property(e => e.Path).HasMaxLength(512).IsRequired().HasDefaultValue(string.Empty);
            modelbuilder.Entity<JsonUnit>().Property(e => e.Value).HasMaxLength(2048);
        }
    }
}

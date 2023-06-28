using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JsonSqlConfigDb.Migrations
{
    /// <inheritdoc />
    public partial class Origin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "JsonSqlConfig");

            migrationBuilder.CreateTable(
                name: "JsonUnits",
                schema: "JsonSqlConfig",
                columns: table => new
                {
                    JsonUnitId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SimpleType = table.Column<int>(type: "int", nullable: false),
                    CompositeType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Index = table.Column<int>(type: "int", nullable: true),
                    Group = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false, defaultValue: ""),
                    Path = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false, defaultValue: ""),
                    Value = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JsonUnits", x => x.JsonUnitId);
                    table.ForeignKey(
                        name: "FK_JsonUnits_JsonUnits_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "JsonSqlConfig",
                        principalTable: "JsonUnits",
                        principalColumn: "JsonUnitId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_JsonUnits_Group_Path",
                schema: "JsonSqlConfig",
                table: "JsonUnits",
                columns: new[] { "Group", "Path" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JsonUnits_ParentId",
                schema: "JsonSqlConfig",
                table: "JsonUnits",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JsonUnits",
                schema: "JsonSqlConfig");
        }
    }
}

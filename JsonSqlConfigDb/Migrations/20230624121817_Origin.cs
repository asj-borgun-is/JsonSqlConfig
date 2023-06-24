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
                name: "JsonUnit",
                schema: "JsonSqlConfig",
                columns: table => new
                {
                    JsonUnitId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsInactive = table.Column<bool>(type: "bit", nullable: false),
                    ValueType = table.Column<int>(type: "int", nullable: false),
                    ChildType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Index = table.Column<int>(type: "int", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JsonUnit", x => x.JsonUnitId);
                    table.ForeignKey(
                        name: "FK_JsonUnit_JsonUnit_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "JsonSqlConfig",
                        principalTable: "JsonUnit",
                        principalColumn: "JsonUnitId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_JsonUnit_ParentId",
                schema: "JsonSqlConfig",
                table: "JsonUnit",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JsonUnit",
                schema: "JsonSqlConfig");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.EntityFrameworkCore.Sqlite.Migrations
{
    public partial class MigrationCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NodaTimeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Instant = table.Column<string>(type: "TEXT", nullable: false),
                    LocalTime = table.Column<string>(type: "TEXT", nullable: false),
                    LocalDate = table.Column<string>(type: "TEXT", nullable: false),
                    LocalDateTime = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodaTimeTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "NodaTimeTypes",
                columns: new[] { "Id", "Instant", "LocalDate", "LocalDateTime", "LocalTime" },
                values: new object[] { 1, "2020-10-10 23:42:16.321", "2020-10-10", "2020-10-10 23:42:16.321", "23:42:16.321" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NodaTimeTypes");
        }
    }
}

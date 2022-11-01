using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenref.Ava.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "References",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Author = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    PubType = table.Column<string>(type: "TEXT", nullable: true),
                    Publisher = table.Column<string>(type: "TEXT", nullable: true),
                    YearRef = table.Column<int>(type: "INTEGER", nullable: true),
                    ISBN = table.Column<int>(type: "INTEGER", nullable: true),
                    DOI = table.Column<int>(type: "INTEGER", nullable: true),
                    Edu = table.Column<string>(type: "TEXT", nullable: true),
                    Location = table.Column<string>(type: "TEXT", nullable: true),
                    Semester = table.Column<string>(type: "TEXT", nullable: true),
                    Language = table.Column<string>(type: "TEXT", nullable: true),
                    YearReport = table.Column<int>(type: "INTEGER", nullable: true),
                    Match = table.Column<int>(type: "INTEGER", nullable: true),
                    Commentary = table.Column<string>(type: "TEXT", nullable: true),
                    Syllabus = table.Column<string>(type: "TEXT", nullable: true),
                    Season = table.Column<string>(type: "TEXT", nullable: true),
                    ExamEvent = table.Column<string>(type: "TEXT", nullable: true),
                    Source = table.Column<string>(type: "TEXT", nullable: true),
                    Pages = table.Column<int>(type: "INTEGER", nullable: true),
                    Volume = table.Column<string>(type: "TEXT", nullable: true),
                    Chapters = table.Column<string>(type: "TEXT", nullable: true),
                    BookTitle = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_References", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Spreadsheets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spreadsheets", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "References");

            migrationBuilder.DropTable(
                name: "Spreadsheets");
        }
    }
}

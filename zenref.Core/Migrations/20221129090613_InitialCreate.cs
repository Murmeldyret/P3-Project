using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace zenref.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "References",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Author = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    PubType = table.Column<string>(type: "TEXT", nullable: false),
                    Publisher = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    IDRef = table.Column<int>(type: "INTEGER", nullable: false),
                    Edu = table.Column<string>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    Semester = table.Column<string>(type: "TEXT", nullable: false),
                    Language = table.Column<string>(type: "TEXT", nullable: false),
                    YearReport = table.Column<int>(type: "INTEGER", nullable: false),
                    OriReference = table.Column<string>(type: "TEXT", nullable: false),
                    Match = table.Column<double>(type: "REAL", nullable: false),
                    Commentary = table.Column<string>(type: "TEXT", nullable: false),
                    Syllabus = table.Column<string>(type: "TEXT", nullable: false),
                    Season = table.Column<string>(type: "TEXT", nullable: false),
                    ExamEvent = table.Column<string>(type: "TEXT", nullable: false),
                    Source = table.Column<string>(type: "TEXT", nullable: false),
                    Pages = table.Column<int>(type: "INTEGER", nullable: false),
                    ISBN = table.Column<string>(type: "TEXT", nullable: false),
                    Volume = table.Column<string>(type: "TEXT", nullable: false),
                    Chapters = table.Column<string>(type: "TEXT", nullable: false),
                    BookTitle = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_References", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "References");
        }
    }
}

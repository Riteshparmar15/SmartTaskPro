using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartTaskPro.Migrations
{
    /// <inheritdoc />
    public partial class FirstDatabaseUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

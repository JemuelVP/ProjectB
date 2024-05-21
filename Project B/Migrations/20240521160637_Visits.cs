using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectB.Migrations
{
    /// <inheritdoc />
    public partial class Visits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Visits",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Visits",
                table: "Users");
        }
    }
}

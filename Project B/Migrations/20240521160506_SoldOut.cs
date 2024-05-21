using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectB.Migrations
{
    /// <inheritdoc />
    public partial class SoldOut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SoldOut",
                table: "Schedule",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoldOut",
                table: "Schedule");
        }
    }
}

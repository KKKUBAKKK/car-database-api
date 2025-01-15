using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace car_database_api.Migrations
{
    /// <inheritdoc />
    public partial class AcceptingReturnMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "rentalName",
                table: "Rentals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rentalName",
                table: "Rentals");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace car_database_api.Migrations
{
    /// <inheritdoc />
    public partial class BaseUrlAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "baseUrl",
                table: "CustomerApis",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "baseUrl",
                table: "CustomerApis");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace car_database_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    producer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    yearOfProduction = table.Column<int>(type: "int", nullable: false),
                    numberOfSeats = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    isAvailable = table.Column<bool>(type: "bit", nullable: false),
                    location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerApis",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerApis", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    firstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    externalId = table.Column<int>(type: "int", nullable: false),
                    rentalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    firstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    birthday = table.Column<DateOnly>(type: "date", nullable: false),
                    driverLicenseReceiveDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RentalOffers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    carId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    dailyRate = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    insuranceRate = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    validUntil = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalOffers", x => x.id);
                    table.ForeignKey(
                        name: "FK_RentalOffers_Cars_carId",
                        column: x => x.carId,
                        principalTable: "Cars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentalOffers_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rentals",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    carId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    totalPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    status = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    startLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    endLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Employeeid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rentals", x => x.id);
                    table.ForeignKey(
                        name: "FK_Rentals_Cars_carId",
                        column: x => x.carId,
                        principalTable: "Cars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rentals_Employees_Employeeid",
                        column: x => x.Employeeid,
                        principalTable: "Employees",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Rentals_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentalId = table.Column<int>(type: "int", nullable: false),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    Condition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FrontPhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackPhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RightPhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeftPhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnRecords_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnRecords_Rentals_RentalId",
                        column: x => x.RentalId,
                        principalTable: "Rentals",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentalOffers_carId",
                table: "RentalOffers",
                column: "carId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalOffers_userId",
                table: "RentalOffers",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_carId",
                table: "Rentals",
                column: "carId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_Employeeid",
                table: "Rentals",
                column: "Employeeid");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_userId",
                table: "Rentals",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRecords_EmployeeID",
                table: "ReturnRecords",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRecords_RentalId",
                table: "ReturnRecords",
                column: "RentalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerApis");

            migrationBuilder.DropTable(
                name: "RentalOffers");

            migrationBuilder.DropTable(
                name: "ReturnRecords");

            migrationBuilder.DropTable(
                name: "Rentals");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

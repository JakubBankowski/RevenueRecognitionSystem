using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RevenueRecognitionSystem.Migrations
{
    /// <inheritdoc />
    public partial class RevenueMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    IsReturningClient = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Krs = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Pesel = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Value = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Software",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CurrentVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Software", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    SoftwareId = table.Column<int>(type: "int", nullable: false),
                    SoftwareVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdditionalSupportYears = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contracts_Software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateReceived = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "Discriminator", "Email", "FirstName", "IsDeleted", "IsReturningClient", "LastName", "Pesel", "Phone" },
                values: new object[] { 1, "Serkowa 2", "Individual", "serek@pl", "John", false, false, "Doe", "12345678912", "111222333" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "CompanyName", "Discriminator", "Email", "IsDeleted", "IsReturningClient", "Krs", "Phone" },
                values: new object[] { 2, "Serkowa 5", "SerekCo", "Company", "serek@com", false, false, "1234567890", "444222333" });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "From", "Name", "To", "Value" },
                values: new object[] { 1, new DateTime(2025, 6, 1, 7, 47, 0, 0, DateTimeKind.Unspecified), "Wiosenna", new DateTime(2026, 6, 1, 7, 47, 0, 0, DateTimeKind.Unspecified), 50.00m });

            migrationBuilder.InsertData(
                table: "Software",
                columns: new[] { "Id", "BasePrice", "Category", "CurrentVersion", "Description", "Name" },
                values: new object[] { 1, 123456m, "serowa", "1.1", "OS serkowe takie o ", "SerekOS" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "Role", "Username" },
                values: new object[] { 1, "AQAAAAIAAYagAAAAEAHs0S3qs6Y77j5fRU9X+akhT0z421ZyRiDfTrgekvo2hmcx9O7AgYEW/3jfulJatw==", "Admin", "admin" });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "AdditionalSupportYears", "CustomerId", "EndDate", "IsPaid", "SoftwareId", "SoftwareVersion", "StartDate", "TotalPrice" },
                values: new object[] { 1, 0, 1, new DateTime(2026, 6, 1, 7, 47, 0, 0, DateTimeKind.Unspecified), true, 1, "1.1", new DateTime(2025, 6, 1, 7, 47, 0, 0, DateTimeKind.Unspecified), 123456m });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "Amount", "ContractId", "DateReceived" },
                values: new object[] { 1, 123456m, 1, new DateTime(2026, 6, 1, 7, 47, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CustomerId",
                table: "Contracts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_SoftwareId",
                table: "Contracts",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Krs",
                table: "Customers",
                column: "Krs",
                unique: true,
                filter: "[Krs] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Pesel",
                table: "Customers",
                column: "Pesel",
                unique: true,
                filter: "[Pesel] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ContractId",
                table: "Payments",
                column: "ContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Software");
        }
    }
}

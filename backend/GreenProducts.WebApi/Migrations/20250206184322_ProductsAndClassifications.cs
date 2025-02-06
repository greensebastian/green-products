using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenProducts.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class ProductsAndClassifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductClassifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductClassifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProductTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductClassifications_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductClassifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductProductClassification",
                columns: table => new
                {
                    AvailableColoursId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductClassification", x => new { x.AvailableColoursId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ProductProductClassification_ProductClassifications_Availab~",
                        column: x => x.AvailableColoursId,
                        principalTable: "ProductClassifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductProductClassification_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductClassifications_Type",
                table: "ProductClassifications",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ProductClassifications_Type_Value",
                table: "ProductClassifications",
                columns: new[] { "Type", "Value" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductClassification_ProductId",
                table: "ProductProductClassification",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatedOn",
                table: "Products",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeId",
                table: "Products",
                column: "ProductTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductProductClassification");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductClassifications");
        }
    }
}

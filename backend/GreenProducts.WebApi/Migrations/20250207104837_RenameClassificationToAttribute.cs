using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenProducts.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class RenameClassificationToAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductClassifications_ProductTypeId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductProductClassification");

            migrationBuilder.DropTable(
                name: "ProductClassifications");

            migrationBuilder.CreateTable(
                name: "ProductAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductProductAttribute",
                columns: table => new
                {
                    AvailableColoursId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductAttribute", x => new { x.AvailableColoursId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ProductProductAttribute_ProductAttributes_AvailableColoursId",
                        column: x => x.AvailableColoursId,
                        principalTable: "ProductAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductProductAttribute_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributes_CreatedOn",
                table: "ProductAttributes",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributes_Type",
                table: "ProductAttributes",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributes_Type_Value",
                table: "ProductAttributes",
                columns: new[] { "Type", "Value" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductAttribute_ProductId",
                table: "ProductProductAttribute",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductAttributes_ProductTypeId",
                table: "Products",
                column: "ProductTypeId",
                principalTable: "ProductAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductAttributes_ProductTypeId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductProductAttribute");

            migrationBuilder.DropTable(
                name: "ProductAttributes");

            migrationBuilder.CreateTable(
                name: "ProductClassifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductClassifications", x => x.Id);
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
                name: "IX_ProductClassifications_CreatedOn",
                table: "ProductClassifications",
                column: "CreatedOn");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductClassifications_ProductTypeId",
                table: "Products",
                column: "ProductTypeId",
                principalTable: "ProductClassifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

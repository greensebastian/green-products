using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenProducts.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class UniqueConstraintForClassificationAndProductName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Name",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductAttributes_Type_Value",
                table: "ProductAttributes");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Products_Name",
                table: "Products",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProductAttributes_Type_Value",
                table: "ProductAttributes",
                columns: new[] { "Type", "Value" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Products_Name",
                table: "Products");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProductAttributes_Type_Value",
                table: "ProductAttributes");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributes_Type_Value",
                table: "ProductAttributes",
                columns: new[] { "Type", "Value" });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenProducts.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class ClassificationCreatedOn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "ProductClassifications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_ProductClassifications_CreatedOn",
                table: "ProductClassifications",
                column: "CreatedOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductClassifications_CreatedOn",
                table: "ProductClassifications");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ProductClassifications");
        }
    }
}

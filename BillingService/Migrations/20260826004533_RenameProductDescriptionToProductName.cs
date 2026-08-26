using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillingService.Migrations
{
    /// <inheritdoc />
    public partial class RenameProductDescriptionToProductName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductDescription",
                table: "InvoiceItems",
                newName: "ProductName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "InvoiceItems",
                newName: "ProductDescription");
        }
    }
}

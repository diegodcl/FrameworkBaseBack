using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingCustomerOwned : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Properties",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_ClientId",
                table: "Properties",
                newName: "IX_Properties_CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Properties",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_CustomerId",
                table: "Properties",
                newName: "IX_Properties_ClientId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixUserCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Client",
                table: "AspNetUsers",
                newName: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "AspNetUsers",
                newName: "Client");
        }
    }
}

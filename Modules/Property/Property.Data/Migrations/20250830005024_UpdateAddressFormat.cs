using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAddressFormat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address_State",
                table: "Properties",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_State",
                table: "Properties");
        }
    }
}

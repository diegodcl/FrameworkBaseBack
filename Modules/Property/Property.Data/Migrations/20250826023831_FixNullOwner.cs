using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixNullOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid[]>(
                name: "Owner",
                table: "Properties",
                type: "uuid[]",
                nullable: true,
                oldClrType: typeof(Guid[]),
                oldType: "uuid[]");

            migrationBuilder.AlterColumn<string>(
                name: "Address_Area",
                table: "Properties",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid[]>(
                name: "Owner",
                table: "Properties",
                type: "uuid[]",
                nullable: false,
                defaultValue: new Guid[0],
                oldClrType: typeof(Guid[]),
                oldType: "uuid[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address_Area",
                table: "Properties",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AreaTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Owner = table.Column<Guid[]>(type: "uuid[]", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Address_Area = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address_City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address_Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Address_Line1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address_Line2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Address_PostCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Email_Address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blueprints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PropertyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blueprints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blueprints_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Size = table.Column<double>(type: "double precision", nullable: true),
                    AreaTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    BlueprintId = table.Column<Guid>(type: "uuid", nullable: false),
                    UnitType = table.Column<int>(type: "integer", nullable: false),
                    AreaId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Areas_AreaTypes_AreaTypeId",
                        column: x => x.AreaTypeId,
                        principalTable: "AreaTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Areas_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Areas_Blueprints_BlueprintId",
                        column: x => x.BlueprintId,
                        principalTable: "Blueprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Structures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Size = table.Column<double>(type: "double precision", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    BlueprintId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Structures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Structures_Blueprints_BlueprintId",
                        column: x => x.BlueprintId,
                        principalTable: "Blueprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AreaStructure",
                columns: table => new
                {
                    AreasId = table.Column<Guid>(type: "uuid", nullable: false),
                    StructuresId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaStructure", x => new { x.AreasId, x.StructuresId });
                    table.ForeignKey(
                        name: "FK_AreaStructure_Areas_AreasId",
                        column: x => x.AreasId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AreaStructure_Structures_StructuresId",
                        column: x => x.StructuresId,
                        principalTable: "Structures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Areas_AreaId",
                table: "Areas",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_AreaTypeId",
                table: "Areas",
                column: "AreaTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_BlueprintId",
                table: "Areas",
                column: "BlueprintId");

            migrationBuilder.CreateIndex(
                name: "IX_AreaStructure_StructuresId",
                table: "AreaStructure",
                column: "StructuresId");

            migrationBuilder.CreateIndex(
                name: "IX_Blueprints_PropertyId",
                table: "Blueprints",
                column: "PropertyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_ClientId",
                table: "Properties",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Structures_BlueprintId",
                table: "Structures",
                column: "BlueprintId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaStructure");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Structures");

            migrationBuilder.DropTable(
                name: "AreaTypes");

            migrationBuilder.DropTable(
                name: "Blueprints");

            migrationBuilder.DropTable(
                name: "Properties");
        }
    }
}

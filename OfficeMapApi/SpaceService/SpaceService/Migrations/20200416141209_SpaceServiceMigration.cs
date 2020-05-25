using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SpaceService.Migrations
{
    public partial class SpaceServiceMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MapFiles",
                columns: table => new
                {
                    MapId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Content = table.Column<byte[]>(fixedLength: true, maxLength: 50, nullable: true),
                    Extension = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    MapGUID = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Obsolete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.MapId);
                });

            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    OfficeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Street = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    House = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Building = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Phone_Number = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    OfficeGUID = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Obsolete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.OfficeId);
                });

            migrationBuilder.CreateTable(
                name: "SpaceTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bookable = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    Description = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    SpaceTypeGUID = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Obsolete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpaceTypes", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "Spaces",
                columns: table => new
                {
                    SpaceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    MapId = table.Column<int>(nullable: true),
                    TypeId = table.Column<int>(nullable: true),
                    SpaceName = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Description = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Capacity = table.Column<int>(nullable: true),
                    Floor = table.Column<int>(nullable: true),
                    SpaceGUID = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Obsolete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Spaces", x => x.SpaceId);
                    table.ForeignKey(
                        name: "FK_Spaces_MapFiles_MapId",
                        column: x => x.MapId,
                        principalTable: "MapFiles",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Spaces_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "OfficeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Spaces_Spaces_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Spaces",
                        principalColumn: "SpaceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Spaces_SpaceTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "SpaceTypes",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Spaces_MapId",
                table: "Spaces",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Spaces_OfficeId",
                table: "Spaces",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Spaces_ParentId",
                table: "Spaces",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Spaces_TypeId",
                table: "Spaces",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Spaces");

            migrationBuilder.DropTable(
                name: "MapFiles");

            migrationBuilder.DropTable(
                name: "Offices");

            migrationBuilder.DropTable(
                name: "SpaceTypes");
        }
    }
}

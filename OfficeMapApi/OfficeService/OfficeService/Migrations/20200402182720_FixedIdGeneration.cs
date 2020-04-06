using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OfficeService.Migrations
{
    public partial class FixedIdGeneration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OfficeId",
                table: "Offices",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateTable(
                name: "Spaces",
                columns: table => new
                {
                    SpaceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    MapId = table.Column<int>(nullable: false),
                    OfficeId = table.Column<int>(nullable: false),
                    SpaceGUID = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Spaces", x => x.SpaceId);
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Spaces_OfficeId",
                table: "Spaces",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Spaces_ParentId",
                table: "Spaces",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Spaces");

            migrationBuilder.AlterColumn<int>(
                name: "OfficeId",
                table: "Offices",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}

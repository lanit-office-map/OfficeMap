using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkplaceService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MapFiles",
                columns: table => new
                {
                    MapId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<byte[]>(fixedLength: true, maxLength: 50, nullable: true),
                    MapGUID = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Obsolete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.MapId);
                });

            migrationBuilder.CreateTable(
                name: "Workplaces",
                columns: table => new
                {
                    WorkplaceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(nullable: true),
                    SpaceId = table.Column<int>(nullable: true),
                    WorkplaceGUID = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Obsolete = table.Column<bool>(nullable: false),
                    MapId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workplace", x => x.WorkplaceId);
                    table.ForeignKey(
                        name: "FK_Workplaces_MapFiles",
                        column: x => x.MapId,
                        principalTable: "MapFiles",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workplaces_MapId",
                table: "Workplaces",
                column: "MapId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workplaces");

            migrationBuilder.DropTable(
                name: "MapFiles");
        }
    }
}

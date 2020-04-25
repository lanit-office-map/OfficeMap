using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MapService.Migrations
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
                    MapGUID = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Extension = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Content = table.Column<byte[]>(fixedLength: true, maxLength: 50, nullable: true),
                    Obsolete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.MapId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapFiles");
        }
    }
}

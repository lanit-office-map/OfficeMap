using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OfficeService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    OfficeId = table.Column<int>(nullable: false),
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Offices");
        }
    }
}

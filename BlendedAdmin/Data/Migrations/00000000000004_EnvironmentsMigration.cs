using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlendedAdmin.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("00000000000004_EnvironmentsMigration")]
    public class EnvironmentsModelMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Environments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Color = table.Column<string>(nullable: true),
                    Index = table.Column<int>(nullable: false),
                    TenantId = table.Column<string>(maxLength: 100, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Environments", x => x.Id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "Environments_TenantIdName_Index",
                table: "Environments",
                unique: true,
                columns: new string[] { "TenantId", "Name" });
        }
    }
}

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlendedAdmin.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("00000000000003_ItemsMigration")]
    public class ItemsModelMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false, maxLength: 250),
                    Category = table.Column<string>(nullable: true, maxLength: 250),
                    Code = table.Column<string>(nullable: true),
                    TenantId = table.Column<string>(maxLength: 100, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "Items_TenantIdName_Index",
                table: "Items",
                unique: true,
                columns: new string[] { "TenantId", "Name" });
        }
    }
}

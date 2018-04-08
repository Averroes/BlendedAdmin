using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlendedAdmin.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("00000000000005_VariablesMigration")]
    public class VariablesModelMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Variables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Value = table.Column<string>(nullable: true),
                    TenantId = table.Column<string>(maxLength: 100, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variables", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "VariablesEnvironments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Value = table.Column<string>(nullable: true),
                    VariableId = table.Column<int>(nullable: true),
                    EnvironmentId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariablesEnvironments", x => x.Id);
                    table.ForeignKey("FK_VariablesEnvironments_EnvironmentId", x => x.EnvironmentId, "Environments", "Id", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_VariablesEnvironments_VariableId", x => x.VariableId, "Variables", "Id", onDelete: ReferentialAction.Cascade);
                }
            );

            migrationBuilder.CreateIndex(
                name: "Variables_TenantIdName_Index",
                table: "Variables",
                unique: true,
                columns: new string[] { "TenantId", "Name" });
        }
    }
}

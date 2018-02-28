using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BlendedAdmin.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("3_VariablesModelMigration")]
    public class VariablesModelMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Variables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variables", x => x.Id);
                    table.UniqueConstraint("UK_Variables_Name", x => x.Name);
                }
            );

            migrationBuilder.CreateTable(
                name: "VariablesEnvironments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    VariableId = table.Column<int>(nullable: true),
                    EnvironmentId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariablesEnvironments", x => x.Id);
                    table.ForeignKey("FK_VariablesEnvironments_EnvironmentId", x => x.EnvironmentId, "Environments", "Id", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_VariablesEnvironments_VariableId", x => x.VariableId, "Variables", "Id", onDelete: ReferentialAction.Cascade);
                    //table.UniqueConstraint("UK_VariablesEnvironments_EnvVar", x => x.EnvironmentId);
                    //table.UniqueConstraint("UK_VariablesEnvironments_EnvVar", x => x.VariableId);
                }
            );
        }
    }
}

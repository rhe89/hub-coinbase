using Microsoft.EntityFrameworkCore.Migrations;

namespace Coinbase.Data.Migrations
{
    public partial class UpdateStuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InitiatedBy",
                schema: "dbo",
                table: "WorkerLog",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitiatedBy",
                schema: "dbo",
                table: "WorkerLog");
        }
    }
}

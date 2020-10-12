using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Coinbase.Data.Migrations
{
    public partial class InsertSetting_AccountWorkerRunInterval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                $@"INSERT INTO dbo.Setting ([Key], [Value], [CreatedDate], [UpdatedDate]) 
                VALUES ('AccountWorkerRunInterval', '60', '{DateTime.Now}', '{DateTime.Now}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

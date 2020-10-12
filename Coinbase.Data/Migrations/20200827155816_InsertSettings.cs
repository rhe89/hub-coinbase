using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Coinbase.Data.Migrations
{
    public partial class InsertSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                $@"INSERT INTO dbo.Setting ([Key], [Value], [CreatedDate], [UpdatedDate]) 
                VALUES ('CoinbaseApiKey', '', '{DateTime.Now}', '{DateTime.Now}'),
                       ('CoinbaseApiSecret', '', '{DateTime.Now}', '{DateTime.Now}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

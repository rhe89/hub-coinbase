using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Coinbase.Data.Migrations
{
    public partial class AddAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"INSERT INTO dbo.Account (CreatedDate, Currency)
                                   VALUES ('{DateTime.Now}', 'BTC')");

            migrationBuilder.Sql($@"INSERT INTO dbo.Account (CreatedDate, Currency)
                                   VALUES ('{DateTime.Now}', 'ETH')");

            migrationBuilder.Sql($@"INSERT INTO dbo.Account (CreatedDate, Currency)
                                   VALUES ('{DateTime.Now}', 'LTC')");

            migrationBuilder.Sql($@"INSERT INTO dbo.Account (CreatedDate, Currency)
                                   VALUES ('{DateTime.Now}', 'BCH')");

            migrationBuilder.Sql($@"INSERT INTO dbo.Account (CreatedDate, Currency)
                                   VALUES ('{DateTime.Now}', 'ZRX')");

            migrationBuilder.Sql($@"INSERT INTO dbo.Account (CreatedDate, Currency)
                                   VALUES ('{DateTime.Now}', 'BAT')");

            migrationBuilder.Sql($@"INSERT INTO dbo.Account (CreatedDate, Currency)
                                   VALUES ('{DateTime.Now}', 'ZEC')");

            migrationBuilder.Sql($@"INSERT INTO dbo.Account (CreatedDate, Currency)
                                   VALUES ('{DateTime.Now}', 'BSV')");

            migrationBuilder.Sql($@"INSERT INTO dbo.Account (CreatedDate, Currency)
                                   VALUES ('{DateTime.Now}', 'ETC')");

            migrationBuilder.Sql($@"INSERT INTO dbo.Account (CreatedDate, Currency)
                                   VALUES ('{DateTime.Now}', 'USD')");

            migrationBuilder.Sql($@"INSERT INTO dbo.Account (CreatedDate, Currency)
                                   VALUES ('{DateTime.Now}', 'XRP')");

            migrationBuilder.Sql($@"INSERT INTO dbo.Account (CreatedDate, Currency)
                                   VALUES ('{DateTime.Now}', 'USDC')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

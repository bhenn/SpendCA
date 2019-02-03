using Microsoft.EntityFrameworkCore.Migrations;

namespace SpendCA.Api.Migrations
{
    public partial class value_long : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Value",
                table: "Spends",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Spends",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aban.DataLayer.Migrations
{
    public partial class Convert_AccountNumber_To_double : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "AccountNumber",
                table: "UserAccount",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "UserAccount",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}

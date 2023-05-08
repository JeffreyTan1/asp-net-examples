using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerWebsite.Migrations
{
    /// <inheritdoc />
    public partial class addadminflags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Blocked",
                table: "Login",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Blocked",
                table: "BillPay",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Cancelled",
                table: "BillPay",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Blocked",
                table: "Login");

            migrationBuilder.DropColumn(
                name: "Blocked",
                table: "BillPay");

            migrationBuilder.DropColumn(
                name: "Cancelled",
                table: "BillPay");
        }
    }
}

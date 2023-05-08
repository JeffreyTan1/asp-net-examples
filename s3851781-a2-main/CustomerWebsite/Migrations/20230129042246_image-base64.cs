using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerWebsite.Migrations
{
    /// <inheritdoc />
    public partial class imagebase64 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paid",
                table: "BillPay");

            migrationBuilder.AddColumn<string>(
                name: "ImageBase64",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageBase64",
                table: "Customer");

            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "BillPay",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

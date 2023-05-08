using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerWebsite.Migrations
{
    /// <inheritdoc />
    public partial class dedupeflag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Blocked",
                table: "BillPay");

            migrationBuilder.RenameColumn(
                name: "Cancelled",
                table: "BillPay",
                newName: "Active");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Active",
                table: "BillPay",
                newName: "Cancelled");

            migrationBuilder.AddColumn<bool>(
                name: "Blocked",
                table: "BillPay",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

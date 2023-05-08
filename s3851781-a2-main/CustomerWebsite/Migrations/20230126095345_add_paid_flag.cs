using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerWebsite.Migrations
{
    /// <inheritdoc />
    public partial class addpaidflag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "BillPay",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paid",
                table: "BillPay");
        }
    }
}

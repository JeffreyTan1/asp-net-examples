using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerWebsite.Migrations
{
    /// <inheritdoc />
    public partial class failedflag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Failed",
                table: "BillPay",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Failed",
                table: "BillPay");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace xPayServer.Migrations
{
    /// <inheritdoc />
    public partial class AddCashAndFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Savings",
                table: "AspNetUsers",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WithdrawalLimit",
                table: "AspNetUsers",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Savings",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WithdrawalLimit",
                table: "AspNetUsers");
        }
    }
}

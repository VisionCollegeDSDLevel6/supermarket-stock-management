using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupermarketStockManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityUserIdToStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Staff",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Staff");
        }
    }
}

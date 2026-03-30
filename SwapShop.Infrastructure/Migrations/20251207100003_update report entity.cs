using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwapShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatereportentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReportType",
                table: "UserReport",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportType",
                table: "UserReport");
        }
    }
}

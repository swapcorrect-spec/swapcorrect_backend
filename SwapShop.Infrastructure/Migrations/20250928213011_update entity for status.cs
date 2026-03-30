using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwapShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateentityforstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SwapListStatus",
                table: "ListingItems",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SwapListStatus",
                table: "ListingItems");
        }
    }
}

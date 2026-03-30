using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwapShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class feetype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FeeType",
                table: "Payments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SwapId",
                table: "Payments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SwapId",
                table: "Payments",
                column: "SwapId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_SwappingProceedings_SwapId",
                table: "Payments",
                column: "SwapId",
                principalTable: "SwappingProceedings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_SwappingProceedings_SwapId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_SwapId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "FeeType",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "SwapId",
                table: "Payments");
        }
    }
}

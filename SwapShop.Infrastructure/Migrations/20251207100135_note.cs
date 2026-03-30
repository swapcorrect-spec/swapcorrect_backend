using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwapShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class note : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportAdminNote_UserReport_ReportId",
                table: "ReportAdminNote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportAdminNote",
                table: "ReportAdminNote");

            migrationBuilder.RenameTable(
                name: "ReportAdminNote",
                newName: "ReportAdminNotes");

            migrationBuilder.RenameIndex(
                name: "IX_ReportAdminNote_ReportId",
                table: "ReportAdminNotes",
                newName: "IX_ReportAdminNotes_ReportId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportAdminNotes",
                table: "ReportAdminNotes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportAdminNotes_UserReport_ReportId",
                table: "ReportAdminNotes",
                column: "ReportId",
                principalTable: "UserReport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportAdminNotes_UserReport_ReportId",
                table: "ReportAdminNotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportAdminNotes",
                table: "ReportAdminNotes");

            migrationBuilder.RenameTable(
                name: "ReportAdminNotes",
                newName: "ReportAdminNote");

            migrationBuilder.RenameIndex(
                name: "IX_ReportAdminNotes_ReportId",
                table: "ReportAdminNote",
                newName: "IX_ReportAdminNote_ReportId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportAdminNote",
                table: "ReportAdminNote",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportAdminNote_UserReport_ReportId",
                table: "ReportAdminNote",
                column: "ReportId",
                principalTable: "UserReport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

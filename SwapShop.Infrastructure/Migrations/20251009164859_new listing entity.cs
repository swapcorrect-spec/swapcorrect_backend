using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwapShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newlistingentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRooms_AspNetUsers_FriendUserId",
                table: "UserRooms");

            migrationBuilder.DropColumn(
                name: "DisplayMessage",
                table: "RoomMessages");

            migrationBuilder.RenameColumn(
                name: "FriendUserId",
                table: "UserRooms",
                newName: "SwapperId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRooms_FriendUserId",
                table: "UserRooms",
                newName: "IX_UserRooms_SwapperId");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "ListingItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastSeen",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SwappingProceedings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ListId = table.Column<string>(type: "text", nullable: false),
                    Userid = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwappingProceedings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SwappingProceedings_AspNetUsers_Userid",
                        column: x => x.Userid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SwappingProceedings_ListingItems_ListId",
                        column: x => x.ListId,
                        principalTable: "ListingItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SwappingProceedings_ListId",
                table: "SwappingProceedings",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "IX_SwappingProceedings_Userid",
                table: "SwappingProceedings",
                column: "Userid");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRooms_AspNetUsers_SwapperId",
                table: "UserRooms",
                column: "SwapperId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRooms_AspNetUsers_SwapperId",
                table: "UserRooms");

            migrationBuilder.DropTable(
                name: "SwappingProceedings");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "ListingItems");

            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastSeen",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "SwapperId",
                table: "UserRooms",
                newName: "FriendUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRooms_SwapperId",
                table: "UserRooms",
                newName: "IX_UserRooms_FriendUserId");

            migrationBuilder.AddColumn<string>(
                name: "DisplayMessage",
                table: "RoomMessages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRooms_AspNetUsers_FriendUserId",
                table: "UserRooms",
                column: "FriendUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

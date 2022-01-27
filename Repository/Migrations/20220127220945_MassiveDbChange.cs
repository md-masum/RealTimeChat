using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class MassiveDbChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                schema: "Chat",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "IsDeleteToUserUser",
                schema: "Chat",
                table: "ChatMessages",
                newName: "IsRead");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                schema: "Chat",
                table: "ResetPasswordTokenHistories",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "ResetToken",
                schema: "Chat",
                table: "ResetPasswordTokenHistories",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "ResetOtp",
                schema: "Chat",
                table: "ResetPasswordTokenHistories",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                schema: "Chat",
                table: "ResetPasswordTokenHistories",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleteToUser",
                schema: "Chat",
                table: "ChatMessages",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UserImages",
                schema: "Chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "TEXT", nullable: true),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    ImageDescription = table.Column<string>(type: "TEXT", nullable: true),
                    IsProfile = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserImages_User_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "Chat",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_FromUserId",
                schema: "Chat",
                table: "ChatMessages",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ToUserId",
                schema: "Chat",
                table: "ChatMessages",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserImages_ApplicationUserId",
                schema: "Chat",
                table: "UserImages",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_User_FromUserId",
                schema: "Chat",
                table: "ChatMessages",
                column: "FromUserId",
                principalSchema: "Chat",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_User_ToUserId",
                schema: "Chat",
                table: "ChatMessages",
                column: "ToUserId",
                principalSchema: "Chat",
                principalTable: "User",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_User_FromUserId",
                schema: "Chat",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_User_ToUserId",
                schema: "Chat",
                table: "ChatMessages");

            migrationBuilder.DropTable(
                name: "UserImages",
                schema: "Chat");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_FromUserId",
                schema: "Chat",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_ToUserId",
                schema: "Chat",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "IsDeleteToUser",
                schema: "Chat",
                table: "ChatMessages");

            migrationBuilder.RenameColumn(
                name: "IsRead",
                schema: "Chat",
                table: "ChatMessages",
                newName: "IsDeleteToUserUser");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                schema: "Chat",
                table: "User",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                schema: "Chat",
                table: "ResetPasswordTokenHistories",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ResetToken",
                schema: "Chat",
                table: "ResetPasswordTokenHistories",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ResetOtp",
                schema: "Chat",
                table: "ResetPasswordTokenHistories",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                schema: "Chat",
                table: "ResetPasswordTokenHistories",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}

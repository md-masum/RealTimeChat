using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class UpdateUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DateOfBirth",
                schema: "Chat",
                table: "User",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "Chat",
                table: "User",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Chat",
                table: "User",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "Chat",
                table: "User",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PermanentAddress",
                schema: "Chat",
                table: "User",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PresentAddress",
                schema: "Chat",
                table: "User",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                schema: "Chat",
                table: "User",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                schema: "Chat",
                table: "User");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "Chat",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Chat",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "Chat",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PermanentAddress",
                schema: "Chat",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PresentAddress",
                schema: "Chat",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                schema: "Chat",
                table: "User");
        }
    }
}

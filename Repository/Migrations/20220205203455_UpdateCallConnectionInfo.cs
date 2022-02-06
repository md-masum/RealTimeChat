using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class UpdateCallConnectionInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToUser",
                schema: "Chat",
                table: "CallConnectionInfos",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "FromUser",
                schema: "Chat",
                table: "CallConnectionInfos",
                newName: "ReceiverId");

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                schema: "Chat",
                table: "CallConnectionInfos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClosed",
                schema: "Chat",
                table: "CallConnectionInfos");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                schema: "Chat",
                table: "CallConnectionInfos",
                newName: "ToUser");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                schema: "Chat",
                table: "CallConnectionInfos",
                newName: "FromUser");
        }
    }
}

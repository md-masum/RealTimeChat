using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class UpdateCallConnectionInfo1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Candidate",
                schema: "Chat",
                table: "CallConnectionInfos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Offer",
                schema: "Chat",
                table: "CallConnectionInfos",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Candidate",
                schema: "Chat",
                table: "CallConnectionInfos");

            migrationBuilder.DropColumn(
                name: "Offer",
                schema: "Chat",
                table: "CallConnectionInfos");
        }
    }
}

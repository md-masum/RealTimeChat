using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class AddCallCOnnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CallConnectionInfos",
                schema: "Chat",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    FromUser = table.Column<string>(type: "TEXT", nullable: true),
                    ToUser = table.Column<string>(type: "TEXT", nullable: true),
                    CallKey = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallConnectionInfos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallConnectionInfos",
                schema: "Chat");
        }
    }
}

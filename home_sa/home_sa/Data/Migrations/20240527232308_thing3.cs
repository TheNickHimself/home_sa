using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace home_sa.Data.Migrations
{
    public partial class thing3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EncryptedSymmetricKey",
                table: "JobReply",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IV",
                table: "JobReply",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EncryptedSymmetricKey",
                table: "JobReply");

            migrationBuilder.DropColumn(
                name: "IV",
                table: "JobReply");
        }
    }
}

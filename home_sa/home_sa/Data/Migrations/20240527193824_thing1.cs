using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace home_sa.Data.Migrations
{
    public partial class thing1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrivateKey",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PublicKey",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrivateKey",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PublicKey",
                table: "AspNetUsers");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace home_sa.Data.Migrations
{
    public partial class thing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoggedIn",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobOportuneties",
                columns: table => new
                {
                    jobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    jobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    jobDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOportuneties", x => x.jobId);
                    table.ForeignKey(
                        name: "FK_JobOportuneties_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobReply",
                columns: table => new
                {
                    replyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    jobId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobOpportunityjobId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobReply", x => x.replyId);
                    table.ForeignKey(
                        name: "FK_JobReply_JobOportuneties_JobOpportunityjobId",
                        column: x => x.JobOpportunityjobId,
                        principalTable: "JobOportuneties",
                        principalColumn: "jobId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobOportuneties_ApplicationUserId",
                table: "JobOportuneties",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobReply_JobOpportunityjobId",
                table: "JobReply",
                column: "JobOpportunityjobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobReply");

            migrationBuilder.DropTable(
                name: "JobOportuneties");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastLoggedIn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}

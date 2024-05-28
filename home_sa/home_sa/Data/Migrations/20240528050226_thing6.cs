using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace home_sa.Data.Migrations
{
    public partial class thing6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobReply_JobOportuneties_JobOpportunityjobId",
                table: "JobReply");

            migrationBuilder.DropIndex(
                name: "IX_JobReply_JobOpportunityjobId",
                table: "JobReply");

            migrationBuilder.DropColumn(
                name: "JobOpportunityjobId",
                table: "JobReply");

            migrationBuilder.RenameColumn(
                name: "jobId",
                table: "JobReply",
                newName: "JobOpportunityId");

            migrationBuilder.CreateIndex(
                name: "IX_JobReply_JobOpportunityId",
                table: "JobReply",
                column: "JobOpportunityId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobReply_JobOportuneties_JobOpportunityId",
                table: "JobReply",
                column: "JobOpportunityId",
                principalTable: "JobOportuneties",
                principalColumn: "jobId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobReply_JobOportuneties_JobOpportunityId",
                table: "JobReply");

            migrationBuilder.DropIndex(
                name: "IX_JobReply_JobOpportunityId",
                table: "JobReply");

            migrationBuilder.RenameColumn(
                name: "JobOpportunityId",
                table: "JobReply",
                newName: "jobId");

            migrationBuilder.AddColumn<int>(
                name: "JobOpportunityjobId",
                table: "JobReply",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobReply_JobOpportunityjobId",
                table: "JobReply",
                column: "JobOpportunityjobId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobReply_JobOportuneties_JobOpportunityjobId",
                table: "JobReply",
                column: "JobOpportunityjobId",
                principalTable: "JobOportuneties",
                principalColumn: "jobId");
        }
    }
}

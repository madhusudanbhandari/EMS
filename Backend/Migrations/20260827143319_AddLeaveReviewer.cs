using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddLeaveReviewer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Leaves_ReviewedBy",
                table: "Leaves",
                column: "ReviewedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_Users_ReviewedBy",
                table: "Leaves",
                column: "ReviewedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_Users_ReviewedBy",
                table: "Leaves");

            migrationBuilder.DropIndex(
                name: "IX_Leaves_ReviewedBy",
                table: "Leaves");
        }
    }
}

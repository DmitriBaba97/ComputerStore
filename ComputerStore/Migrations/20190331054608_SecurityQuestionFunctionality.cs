using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerStore.Migrations
{
    public partial class SecurityQuestionFunctionality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecretQuestionAnswer",
                table: "AspNetUsers",
                newName: "SecurityQuestionAnswer");

            migrationBuilder.RenameColumn(
                name: "SecretQuestion",
                table: "AspNetUsers",
                newName: "SecurityQuestion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecurityQuestionAnswer",
                table: "AspNetUsers",
                newName: "SecretQuestionAnswer");

            migrationBuilder.RenameColumn(
                name: "SecurityQuestion",
                table: "AspNetUsers",
                newName: "SecretQuestion");
        }
    }
}

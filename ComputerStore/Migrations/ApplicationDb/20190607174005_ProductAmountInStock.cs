using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerStore.Migrations.ApplicationDb
{
    public partial class ProductAmountInStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AmountInStock",
                table: "Products",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountInStock",
                table: "Products");
        }
    }
}

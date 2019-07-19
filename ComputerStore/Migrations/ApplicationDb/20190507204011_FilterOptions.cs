using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerStore.Migrations.ApplicationDb
{
    public partial class FilterOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProperties_Products_ProductId",
                table: "ProductProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Subcategories_SubcategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Subcategories_Categories_CategoryId",
                table: "Subcategories");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Subcategories",
                newName: "CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Subcategories_CategoryId",
                table: "Subcategories",
                newName: "IX_Subcategories_CategoryID");

            migrationBuilder.RenameColumn(
                name: "SubcategoryId",
                table: "Products",
                newName: "SubcategoryID");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Products",
                newName: "CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_SubcategoryId",
                table: "Products",
                newName: "IX_Products_SubcategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                newName: "IX_Products_CategoryID");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductProperties",
                newName: "ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductProperties_ProductId",
                table: "ProductProperties",
                newName: "IX_ProductProperties_ProductID");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryID",
                table: "Subcategories",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SubcategoryID",
                table: "Products",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CategoryID",
                table: "Products",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ProductID",
                table: "ProductProperties",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "FilterOptions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    SubcategoryID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilterOptions_Subcategories_SubcategoryID",
                        column: x => x.SubcategoryID,
                        principalTable: "Subcategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FilterOptionValues",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<string>(nullable: true),
                    FilterOptionID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterOptionValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilterOptionValues_FilterOptions_FilterOptionID",
                        column: x => x.FilterOptionID,
                        principalTable: "FilterOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilterOptions_SubcategoryID",
                table: "FilterOptions",
                column: "SubcategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_FilterOptionValues_FilterOptionID",
                table: "FilterOptionValues",
                column: "FilterOptionID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProperties_Products_ProductID",
                table: "ProductProperties",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Subcategories_SubcategoryID",
                table: "Products",
                column: "SubcategoryID",
                principalTable: "Subcategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subcategories_Categories_CategoryID",
                table: "Subcategories",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProperties_Products_ProductID",
                table: "ProductProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Subcategories_SubcategoryID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Subcategories_Categories_CategoryID",
                table: "Subcategories");

            migrationBuilder.DropTable(
                name: "FilterOptionValues");

            migrationBuilder.DropTable(
                name: "FilterOptions");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "Subcategories",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Subcategories_CategoryID",
                table: "Subcategories",
                newName: "IX_Subcategories_CategoryId");

            migrationBuilder.RenameColumn(
                name: "SubcategoryID",
                table: "Products",
                newName: "SubcategoryId");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "Products",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_SubcategoryID",
                table: "Products",
                newName: "IX_Products_SubcategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                newName: "IX_Products_CategoryId");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "ProductProperties",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductProperties_ProductID",
                table: "ProductProperties",
                newName: "IX_ProductProperties_ProductId");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                table: "Subcategories",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "SubcategoryId",
                table: "Products",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                table: "Products",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "ProductProperties",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProperties_Products_ProductId",
                table: "ProductProperties",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Subcategories_SubcategoryId",
                table: "Products",
                column: "SubcategoryId",
                principalTable: "Subcategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subcategories_Categories_CategoryId",
                table: "Subcategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

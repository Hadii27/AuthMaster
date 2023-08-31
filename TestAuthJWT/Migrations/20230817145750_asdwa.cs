using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAuthJWT.Migrations
{
    /// <inheritdoc />
    public partial class asdwa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_categories_categoryModelCategoryId",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_categoryModelCategoryId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "categoryModelCategoryId",
                table: "products");

            migrationBuilder.CreateIndex(
                name: "IX_products_categoryId",
                table: "products",
                column: "categoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_products_categories_categoryId",
                table: "products",
                column: "categoryId",
                principalTable: "categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_categories_categoryId",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_categoryId",
                table: "products");

            migrationBuilder.AddColumn<int>(
                name: "categoryModelCategoryId",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_products_categoryModelCategoryId",
                table: "products",
                column: "categoryModelCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_products_categories_categoryModelCategoryId",
                table: "products",
                column: "categoryModelCategoryId",
                principalTable: "categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

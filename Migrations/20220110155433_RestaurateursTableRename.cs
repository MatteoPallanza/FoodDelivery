using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodDelivery.Migrations
{
    public partial class RestaurateursTableRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurateur_AspNetUsers_UserId",
                table: "Restaurateur");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurateur_RestaurateurCategories_CategoryId",
                table: "Restaurateur");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Restaurateur",
                table: "Restaurateur");

            migrationBuilder.RenameTable(
                name: "Restaurateur",
                newName: "Restaurateurs");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurateur_CategoryId",
                table: "Restaurateurs",
                newName: "IX_Restaurateurs_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Restaurateurs",
                table: "Restaurateurs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurateurs_AspNetUsers_UserId",
                table: "Restaurateurs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurateurs_RestaurateurCategories_CategoryId",
                table: "Restaurateurs",
                column: "CategoryId",
                principalTable: "RestaurateurCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurateurs_AspNetUsers_UserId",
                table: "Restaurateurs");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurateurs_RestaurateurCategories_CategoryId",
                table: "Restaurateurs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Restaurateurs",
                table: "Restaurateurs");

            migrationBuilder.RenameTable(
                name: "Restaurateurs",
                newName: "Restaurateur");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurateurs_CategoryId",
                table: "Restaurateur",
                newName: "IX_Restaurateur_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Restaurateur",
                table: "Restaurateur",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurateur_AspNetUsers_UserId",
                table: "Restaurateur",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurateur_RestaurateurCategories_CategoryId",
                table: "Restaurateur",
                column: "CategoryId",
                principalTable: "RestaurateurCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

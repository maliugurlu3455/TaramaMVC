using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaramaMVC.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YayinAlintiBilgisis_PersonelYayinBilgileris_PersonelYayinBilgileriId",
                table: "YayinAlintiBilgisis");

            migrationBuilder.DropIndex(
                name: "IX_YayinAlintiBilgisis_PersonelYayinBilgileriId",
                table: "YayinAlintiBilgisis");

            migrationBuilder.DropColumn(
                name: "PersonelYayinBilgileriId",
                table: "YayinAlintiBilgisis");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonelYayinBilgileriId",
                table: "YayinAlintiBilgisis",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_YayinAlintiBilgisis_PersonelYayinBilgileriId",
                table: "YayinAlintiBilgisis",
                column: "PersonelYayinBilgileriId");

            migrationBuilder.AddForeignKey(
                name: "FK_YayinAlintiBilgisis_PersonelYayinBilgileris_PersonelYayinBilgileriId",
                table: "YayinAlintiBilgisis",
                column: "PersonelYayinBilgileriId",
                principalTable: "PersonelYayinBilgileris",
                principalColumn: "Id");
        }
    }
}

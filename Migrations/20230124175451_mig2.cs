using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaramaMVC.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonelYayinBilgileris_Personels_PersonelId",
                table: "PersonelYayinBilgileris");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonelYayinBilgileris_YayinBilgisi_YayinBilgisiId",
                table: "PersonelYayinBilgileris");

            migrationBuilder.DropTable(
                name: "YayinBilgisi");

            migrationBuilder.DropIndex(
                name: "IX_PersonelYayinBilgileris_YayinBilgisiId",
                table: "PersonelYayinBilgileris");

            migrationBuilder.DropColumn(
                name: "YayinBilgisiId",
                table: "PersonelYayinBilgileris");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "PersonelYayinBilgileris",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PersonelId",
                table: "PersonelYayinBilgileris",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Alinti",
                table: "PersonelYayinBilgileris",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Baslik",
                table: "PersonelYayinBilgileris",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Yil",
                table: "PersonelYayinBilgileris",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonelYayinBilgileris_Personels_PersonelId",
                table: "PersonelYayinBilgileris",
                column: "PersonelId",
                principalTable: "Personels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonelYayinBilgileris_Personels_PersonelId",
                table: "PersonelYayinBilgileris");

            migrationBuilder.DropColumn(
                name: "Alinti",
                table: "PersonelYayinBilgileris");

            migrationBuilder.DropColumn(
                name: "Baslik",
                table: "PersonelYayinBilgileris");

            migrationBuilder.DropColumn(
                name: "Yil",
                table: "PersonelYayinBilgileris");

            migrationBuilder.AlterColumn<int>(
                name: "UpdateDate",
                table: "PersonelYayinBilgileris",
                type: "int",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "PersonelId",
                table: "PersonelYayinBilgileris",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "YayinBilgisiId",
                table: "PersonelYayinBilgileris",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "YayinBilgisi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Alinti = table.Column<int>(type: "int", nullable: false),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Yil = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YayinBilgisi", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonelYayinBilgileris_YayinBilgisiId",
                table: "PersonelYayinBilgileris",
                column: "YayinBilgisiId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonelYayinBilgileris_Personels_PersonelId",
                table: "PersonelYayinBilgileris",
                column: "PersonelId",
                principalTable: "Personels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonelYayinBilgileris_YayinBilgisi_YayinBilgisiId",
                table: "PersonelYayinBilgileris",
                column: "YayinBilgisiId",
                principalTable: "YayinBilgisi",
                principalColumn: "Id");
        }
    }
}

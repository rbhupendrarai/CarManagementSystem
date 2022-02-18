using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarManagementSystem.Data.Migrations
{
    public partial class cridsubmodelimgupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Model_MO_Id1",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_SubModel_Model_MO_Id",
                table: "SubModel");

            migrationBuilder.DropIndex(
                name: "IX_Images_MO_Id1",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "MO_Id1",
                table: "Images");

            migrationBuilder.AlterColumn<Guid>(
                name: "MO_Id",
                table: "SubModel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MO_Id",
                table: "Images",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Images_MO_Id",
                table: "Images",
                column: "MO_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Model_MO_Id",
                table: "Images",
                column: "MO_Id",
                principalTable: "Model",
                principalColumn: "MO_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubModel_Model_MO_Id",
                table: "SubModel",
                column: "MO_Id",
                principalTable: "Model",
                principalColumn: "MO_Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Model_MO_Id",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_SubModel_Model_MO_Id",
                table: "SubModel");

            migrationBuilder.DropIndex(
                name: "IX_Images_MO_Id",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "MO_Id",
                table: "Images");

            migrationBuilder.AlterColumn<Guid>(
                name: "MO_Id",
                table: "SubModel",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "MO_Id1",
                table: "Images",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_MO_Id1",
                table: "Images",
                column: "MO_Id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Model_MO_Id1",
                table: "Images",
                column: "MO_Id1",
                principalTable: "Model",
                principalColumn: "MO_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubModel_Model_MO_Id",
                table: "SubModel",
                column: "MO_Id",
                principalTable: "Model",
                principalColumn: "MO_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

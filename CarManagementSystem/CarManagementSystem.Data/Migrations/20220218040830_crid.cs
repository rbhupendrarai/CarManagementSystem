using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarManagementSystem.Data.Migrations
{
    public partial class crid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Model_Car_CR_Id",
                table: "Model");

            migrationBuilder.AlterColumn<Guid>(
                name: "CR_Id",
                table: "Model",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Model_Car_CR_Id",
                table: "Model",
                column: "CR_Id",
                principalTable: "Car",
                principalColumn: "CR_Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Model_Car_CR_Id",
                table: "Model");

            migrationBuilder.AlterColumn<Guid>(
                name: "CR_Id",
                table: "Model",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Model_Car_CR_Id",
                table: "Model",
                column: "CR_Id",
                principalTable: "Car",
                principalColumn: "CR_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

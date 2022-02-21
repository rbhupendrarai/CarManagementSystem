using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarManagementSystem.Data.Migrations
{
    public partial class cridsubmodelimgupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropForeignKey(
                name: "FK_SubModel_Model_MO_Id",
                table: "SubModel");



          

            migrationBuilder.AlterColumn<Guid>(
                name: "MO_Id",
                table: "SubModel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

          

         

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
                name: "FK_SubModel_Model_MO_Id",
                table: "SubModel");

           

            migrationBuilder.AlterColumn<Guid>(
                name: "MO_Id",
                table: "SubModel",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

          

            

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

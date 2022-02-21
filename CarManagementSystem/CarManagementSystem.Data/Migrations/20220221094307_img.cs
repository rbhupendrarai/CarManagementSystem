using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarManagementSystem.Data.Migrations
{
    public partial class img : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Img_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Img = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "DateTime", maxLength: 30, nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    MO_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Img_Id);
                    table.ForeignKey(
                        name: "FK_Images_Model_MO_Id",
                        column: x => x.MO_Id,
                        principalTable: "Model",
                        principalColumn: "MO_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_MO_Id",
                table: "Images",
                column: "MO_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}

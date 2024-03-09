using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageEditor.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ImageEditor");

            migrationBuilder.CreateTable(
                name: "TB_User",
                schema: "ImageEditor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false),
                    Email = table.Column<string>(type: "varchar(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_Image",
                schema: "ImageEditor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    S3OriginalImage = table.Column<string>(type: "varchar(1024)", nullable: false),
                    S3EditedImage = table.Column<string>(type: "varchar(1024)", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_Image_TB_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "ImageEditor",
                        principalTable: "TB_User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_Image_UserId",
                schema: "ImageEditor",
                table: "TB_Image",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_Image",
                schema: "ImageEditor");

            migrationBuilder.DropTable(
                name: "TB_User",
                schema: "ImageEditor");
        }
    }
}

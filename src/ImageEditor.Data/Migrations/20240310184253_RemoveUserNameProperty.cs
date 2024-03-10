using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageEditor.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserNameProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                schema: "ImageEditor",
                table: "TB_User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "ImageEditor",
                table: "TB_User",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");
        }
    }
}

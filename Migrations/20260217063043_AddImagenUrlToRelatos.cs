using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Relatosxxx.Migrations
{
    /// <inheritdoc />
    public partial class AddImagenUrlToRelatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "Relatos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "Relatos");
        }
    }
}

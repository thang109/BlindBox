using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlindBoxWebsite.Migrations
{
    /// <inheritdoc />
    public partial class BlindBoxDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Product",
                table: "blind_boxes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Product",
                table: "blind_boxes");
        }
    }
}

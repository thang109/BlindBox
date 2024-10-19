using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlindBoxWebsite.Migrations
{
    /// <inheritdoc />
    public partial class ImagesOrderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "order_items",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "order_items");
        }
    }
}

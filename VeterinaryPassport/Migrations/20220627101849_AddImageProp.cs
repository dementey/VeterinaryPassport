using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeterinaryPassport.Migrations
{
    public partial class AddImageProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Pets");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace MusteriKazan.DataAccess.Migrations
{
    public partial class musterikazanupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Musteriler",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Musteriler");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace MusteriKazan.DataAccess.Migrations
{
    public partial class musterikazanupdate4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUse",
                table: "MusteriAktivasyonlar",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUse",
                table: "MusteriAktivasyonlar");
        }
    }
}

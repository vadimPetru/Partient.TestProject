using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Partient.TestProject.Infrastructure.Migrations
{
    public partial class GeoIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_BirthDate",
                table: "Patients");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:btree_gist", ",,");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_BirthDate_GiST",
                table: "Patients",
                column: "BirthDate")
                .Annotation("Npgsql:IndexMethod", "gist");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_BirthDate_GiST",
                table: "Patients");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:btree_gist", ",,");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_BirthDate",
                table: "Patients",
                column: "BirthDate");
        }
    }
}

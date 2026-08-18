using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Checador.API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRolesYFotosAsistencia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Asistencia");

            migrationBuilder.AddColumn<string>(
                name: "fotoEntrada",
                table: "Asistencia",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fotoSalida",
                table: "Asistencia",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "permisoId",
                table: "Asistencia",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "salidaAnticipada",
                table: "Asistencia",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Asistencia_permisoId",
                table: "Asistencia",
                column: "permisoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Asistencia_Permisos_permisoId",
                table: "Asistencia",
                column: "permisoId",
                principalTable: "Permisos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asistencia_Permisos_permisoId",
                table: "Asistencia");

            migrationBuilder.DropIndex(
                name: "IX_Asistencia_permisoId",
                table: "Asistencia");

            migrationBuilder.DropColumn(
                name: "fotoEntrada",
                table: "Asistencia");

            migrationBuilder.DropColumn(
                name: "fotoSalida",
                table: "Asistencia");

            migrationBuilder.DropColumn(
                name: "permisoId",
                table: "Asistencia");

            migrationBuilder.DropColumn(
                name: "salidaAnticipada",
                table: "Asistencia");

            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "Empleados",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "Asistencia",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

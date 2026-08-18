using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Checador.API.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurarPermisosYRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permisos_Empleados_EmpleadoNumeroEmpleado",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "AutorizadoPorId",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "EmpleadoId",
                table: "Permisos");

            migrationBuilder.RenameColumn(
                name: "EmpleadoNumeroEmpleado",
                table: "Permisos",
                newName: "numeroEmpleado");

            migrationBuilder.RenameIndex(
                name: "IX_Permisos_EmpleadoNumeroEmpleado",
                table: "Permisos",
                newName: "IX_Permisos_numeroEmpleado");

            migrationBuilder.AlterColumn<string>(
                name: "Motivo",
                table: "Permisos",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Permisos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "AutorizadoPor",
                table: "Permisos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Huella",
                table: "Admins",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "Admins",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Permisos_Empleados_numeroEmpleado",
                table: "Permisos",
                column: "numeroEmpleado",
                principalTable: "Empleados",
                principalColumn: "numeroEmpleado",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permisos_Empleados_numeroEmpleado",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "AutorizadoPor",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Admins");

            migrationBuilder.RenameColumn(
                name: "numeroEmpleado",
                table: "Permisos",
                newName: "EmpleadoNumeroEmpleado");

            migrationBuilder.RenameIndex(
                name: "IX_Permisos_numeroEmpleado",
                table: "Permisos",
                newName: "IX_Permisos_EmpleadoNumeroEmpleado");

            migrationBuilder.AlterColumn<string>(
                name: "Motivo",
                table: "Permisos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Permisos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<int>(
                name: "AutorizadoPorId",
                table: "Permisos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmpleadoId",
                table: "Permisos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Huella",
                table: "Admins",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Permisos_Empleados_EmpleadoNumeroEmpleado",
                table: "Permisos",
                column: "EmpleadoNumeroEmpleado",
                principalTable: "Empleados",
                principalColumn: "numeroEmpleado",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

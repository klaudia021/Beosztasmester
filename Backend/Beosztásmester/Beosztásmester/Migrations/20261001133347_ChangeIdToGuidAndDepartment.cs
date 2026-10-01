using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Beosztasmester.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIdToGuidAndDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Demand_Templates_Organizational_Units_Organizational_Unit_Id",
                table: "Demand_Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_Demands_Organizational_Units_Organizational_Unit_Id",
                table: "Demands");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Organizational_Units_Organizational_Unit_Id",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Horizons_Organizational_Units_Organizational_Unit_Id",
                table: "Horizons");

            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Organizational_Units_Organizational_Unit_Id",
                table: "Rules");

            migrationBuilder.DropForeignKey(
                name: "FK_Shift_Types_Organizational_Units_Organizational_Unit_Id",
                table: "Shift_Types");

            migrationBuilder.DropTable(
                name: "Organizational_Units");

            migrationBuilder.DropIndex(
                name: "IX_Shift_Types_Organizational_Unit_Id",
                table: "Shift_Types");

            migrationBuilder.DropIndex(
                name: "IX_Rules_Organizational_Unit_Id",
                table: "Rules");

            migrationBuilder.DropIndex(
                name: "IX_Horizons_Organizational_Unit_Id",
                table: "Horizons");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Organizational_Unit_Id",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Demands_Organizational_Unit_Id",
                table: "Demands");

            migrationBuilder.DropIndex(
                name: "IX_Demand_Templates_Organizational_Unit_Id",
                table: "Demand_Templates");

            migrationBuilder.DropColumn(
                name: "Organizational_Unit_Id",
                table: "Shift_Types");

            migrationBuilder.DropColumn(
                name: "Organizational_Unit_Id",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "Organizational_Unit_Id",
                table: "Horizons");

            migrationBuilder.DropColumn(
                name: "Organizational_Unit_Id",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Organizational_Unit_Id",
                table: "Demands");

            migrationBuilder.DropColumn(
                name: "Organizational_Unit_Id",
                table: "Demand_Templates");

            migrationBuilder.AlterColumn<Guid>(
                name: "Employee_Id",
                table: "Unavailabilities",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Unavailabilities",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Shift_Types",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "Department_Id",
                table: "Shift_Types",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Rules",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "Department_Id",
                table: "Rules",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Horizon_Id",
                table: "Roster_Versions",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Roster_Versions",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "Shift_Type_Id",
                table: "Requests",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Employee_Id",
                table: "Requests",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Requests",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Horizons",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "Department_Id",
                table: "Horizons",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Employees",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "Department_Id",
                table: "Employees",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Employee_Id",
                table: "Employee_Competencies",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "Competency_Id",
                table: "Employee_Competencies",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Employee_Competencies",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "Shift_Type_Id",
                table: "Demands",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "Competency_Id",
                table: "Demands",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Demands",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "Department_Id",
                table: "Demands",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Shift_Type_Id",
                table: "Demand_Templates",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "Competency_Id",
                table: "Demand_Templates",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Demand_Templates",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "Department_Id",
                table: "Demand_Templates",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Competencies",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "User_Id",
                table: "Audit_Entries",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "Entity_Id",
                table: "Audit_Entries",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Audit_Entries",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "Version_Id",
                table: "Assignments",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "Shift_Type_Id",
                table: "Assignments",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "Roster_VersionId",
                table: "Assignments",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Employee_Id",
                table: "Assignments",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Assignments",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Parent_Id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Department_Department_Parent_Id",
                        column: x => x.Parent_Id,
                        principalTable: "Department",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shift_Types_Department_Id",
                table: "Shift_Types",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_Department_Id",
                table: "Rules",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Horizons_Department_Id",
                table: "Horizons",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Department_Id",
                table: "Employees",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Demands_Department_Id",
                table: "Demands",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_Templates_Department_Id",
                table: "Demand_Templates",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Department_Parent_Id",
                table: "Department",
                column: "Parent_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Demand_Templates_Department_Department_Id",
                table: "Demand_Templates",
                column: "Department_Id",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Demands_Department_Department_Id",
                table: "Demands",
                column: "Department_Id",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Department_Department_Id",
                table: "Employees",
                column: "Department_Id",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Horizons_Department_Department_Id",
                table: "Horizons",
                column: "Department_Id",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Department_Department_Id",
                table: "Rules",
                column: "Department_Id",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shift_Types_Department_Department_Id",
                table: "Shift_Types",
                column: "Department_Id",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Demand_Templates_Department_Department_Id",
                table: "Demand_Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_Demands_Department_Department_Id",
                table: "Demands");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Department_Department_Id",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Horizons_Department_Department_Id",
                table: "Horizons");

            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Department_Department_Id",
                table: "Rules");

            migrationBuilder.DropForeignKey(
                name: "FK_Shift_Types_Department_Department_Id",
                table: "Shift_Types");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropIndex(
                name: "IX_Shift_Types_Department_Id",
                table: "Shift_Types");

            migrationBuilder.DropIndex(
                name: "IX_Rules_Department_Id",
                table: "Rules");

            migrationBuilder.DropIndex(
                name: "IX_Horizons_Department_Id",
                table: "Horizons");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Department_Id",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Demands_Department_Id",
                table: "Demands");

            migrationBuilder.DropIndex(
                name: "IX_Demand_Templates_Department_Id",
                table: "Demand_Templates");

            migrationBuilder.DropColumn(
                name: "Department_Id",
                table: "Shift_Types");

            migrationBuilder.DropColumn(
                name: "Department_Id",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "Department_Id",
                table: "Horizons");

            migrationBuilder.DropColumn(
                name: "Department_Id",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Department_Id",
                table: "Demands");

            migrationBuilder.DropColumn(
                name: "Department_Id",
                table: "Demand_Templates");

            migrationBuilder.AlterColumn<int>(
                name: "Employee_Id",
                table: "Unavailabilities",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Unavailabilities",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Shift_Types",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Organizational_Unit_Id",
                table: "Shift_Types",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Rules",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Organizational_Unit_Id",
                table: "Rules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Horizon_Id",
                table: "Roster_Versions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Roster_Versions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Shift_Type_Id",
                table: "Requests",
                type: "integer",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Employee_Id",
                table: "Requests",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Requests",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Horizons",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Organizational_Unit_Id",
                table: "Horizons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Employees",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Organizational_Unit_Id",
                table: "Employees",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Employee_Id",
                table: "Employee_Competencies",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "Competency_Id",
                table: "Employee_Competencies",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Employee_Competencies",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Shift_Type_Id",
                table: "Demands",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "Competency_Id",
                table: "Demands",
                type: "integer",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Demands",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Organizational_Unit_Id",
                table: "Demands",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Shift_Type_Id",
                table: "Demand_Templates",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "Competency_Id",
                table: "Demand_Templates",
                type: "integer",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Demand_Templates",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Organizational_Unit_Id",
                table: "Demand_Templates",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Competencies",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "User_Id",
                table: "Audit_Entries",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "Entity_Id",
                table: "Audit_Entries",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Audit_Entries",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Version_Id",
                table: "Assignments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "Shift_Type_Id",
                table: "Assignments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "Roster_VersionId",
                table: "Assignments",
                type: "integer",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Employee_Id",
                table: "Assignments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Assignments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateTable(
                name: "Organizational_Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Parent_Id = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizational_Units", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizational_Units_Organizational_Units_Parent_Id",
                        column: x => x.Parent_Id,
                        principalTable: "Organizational_Units",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shift_Types_Organizational_Unit_Id",
                table: "Shift_Types",
                column: "Organizational_Unit_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_Organizational_Unit_Id",
                table: "Rules",
                column: "Organizational_Unit_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Horizons_Organizational_Unit_Id",
                table: "Horizons",
                column: "Organizational_Unit_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Organizational_Unit_Id",
                table: "Employees",
                column: "Organizational_Unit_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Demands_Organizational_Unit_Id",
                table: "Demands",
                column: "Organizational_Unit_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_Templates_Organizational_Unit_Id",
                table: "Demand_Templates",
                column: "Organizational_Unit_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Organizational_Units_Parent_Id",
                table: "Organizational_Units",
                column: "Parent_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Demand_Templates_Organizational_Units_Organizational_Unit_Id",
                table: "Demand_Templates",
                column: "Organizational_Unit_Id",
                principalTable: "Organizational_Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Demands_Organizational_Units_Organizational_Unit_Id",
                table: "Demands",
                column: "Organizational_Unit_Id",
                principalTable: "Organizational_Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Organizational_Units_Organizational_Unit_Id",
                table: "Employees",
                column: "Organizational_Unit_Id",
                principalTable: "Organizational_Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Horizons_Organizational_Units_Organizational_Unit_Id",
                table: "Horizons",
                column: "Organizational_Unit_Id",
                principalTable: "Organizational_Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Organizational_Units_Organizational_Unit_Id",
                table: "Rules",
                column: "Organizational_Unit_Id",
                principalTable: "Organizational_Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shift_Types_Organizational_Units_Organizational_Unit_Id",
                table: "Shift_Types",
                column: "Organizational_Unit_Id",
                principalTable: "Organizational_Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

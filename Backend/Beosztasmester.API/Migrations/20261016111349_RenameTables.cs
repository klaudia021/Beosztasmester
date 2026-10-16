using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Beosztasmester.Migrations
{
    /// <inheritdoc />
    public partial class RenameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Employees_Employee_Id",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Roster_Versions_Roster_Version_Id",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Shift_Types_Shift_Type_Id",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Demands_Competencies_Competency_Id",
                table: "Demands");

            migrationBuilder.DropForeignKey(
                name: "FK_Demands_Department_Department_Id",
                table: "Demands");

            migrationBuilder.DropForeignKey(
                name: "FK_Demands_Shift_Types_Shift_Type_Id",
                table: "Demands");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_Department_Parent_Id",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Department_Department_Id",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Horizons_Department_Department_Id",
                table: "Horizons");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Employees_Employee_Id",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Shift_Types_Shift_Type_Id",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Department_Department_Id",
                table: "Rules");

            migrationBuilder.DropForeignKey(
                name: "FK_Unavailabilities_Employees_Employee_Id",
                table: "Unavailabilities");

            migrationBuilder.DropTable(
                name: "Audit_Entries");

            migrationBuilder.DropTable(
                name: "Demand_Templates");

            migrationBuilder.DropTable(
                name: "Employee_Competencies");

            migrationBuilder.DropTable(
                name: "Roster_Versions");

            migrationBuilder.DropTable(
                name: "Shift_Types");

            migrationBuilder.RenameColumn(
                name: "Start_Date",
                table: "Unavailabilities",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "End_Date",
                table: "Unavailabilities",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "Employee_Id",
                table: "Unavailabilities",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Unavailabilities_Employee_Id",
                table: "Unavailabilities",
                newName: "IX_Unavailabilities_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "Horizon_Id",
                table: "SolverRuns",
                newName: "HorizonId");

            migrationBuilder.RenameColumn(
                name: "Type_Code",
                table: "Rules",
                newName: "TypeCode");

            migrationBuilder.RenameColumn(
                name: "Scope_Ref_Id",
                table: "Rules",
                newName: "ScopeRefId");

            migrationBuilder.RenameColumn(
                name: "Parameters_Json",
                table: "Rules",
                newName: "ParametersJson");

            migrationBuilder.RenameColumn(
                name: "Is_Hard",
                table: "Rules",
                newName: "IsHard");

            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "Rules",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Department_Id",
                table: "Rules",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Rules_Department_Id",
                table: "Rules",
                newName: "IX_Rules_DepartmentId");

            migrationBuilder.RenameColumn(
                name: "Shift_Type_Id",
                table: "Requests",
                newName: "ShiftTypeId");

            migrationBuilder.RenameColumn(
                name: "Employee_Id",
                table: "Requests",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_Shift_Type_Id",
                table: "Requests",
                newName: "IX_Requests_ShiftTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_Employee_Id",
                table: "Requests",
                newName: "IX_Requests_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "Start_Date",
                table: "Horizons",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "End_Date",
                table: "Horizons",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "Department_Id",
                table: "Horizons",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Horizons_Department_Id",
                table: "Horizons",
                newName: "IX_Horizons_DepartmentId");

            migrationBuilder.RenameColumn(
                name: "Exit_Date",
                table: "Employees",
                newName: "ExitDate");

            migrationBuilder.RenameColumn(
                name: "Entry_Date",
                table: "Employees",
                newName: "EntryDate");

            migrationBuilder.RenameColumn(
                name: "Department_Id",
                table: "Employees",
                newName: "DepartmentId");

            migrationBuilder.RenameColumn(
                name: "Contract_Hours_Per_Week",
                table: "Employees",
                newName: "ContractHoursPerWeek");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_Department_Id",
                table: "Employees",
                newName: "IX_Employees_DepartmentId");

            migrationBuilder.RenameColumn(
                name: "Parent_Id",
                table: "Department",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Department_Parent_Id",
                table: "Department",
                newName: "IX_Department_ParentId");

            migrationBuilder.RenameColumn(
                name: "Shift_Type_Id",
                table: "Demands",
                newName: "ShiftTypeId");

            migrationBuilder.RenameColumn(
                name: "Min_Headcount",
                table: "Demands",
                newName: "MinHeadcount");

            migrationBuilder.RenameColumn(
                name: "Max_Headcount",
                table: "Demands",
                newName: "MaxHeadcount");

            migrationBuilder.RenameColumn(
                name: "Department_Id",
                table: "Demands",
                newName: "DepartmentId");

            migrationBuilder.RenameColumn(
                name: "Competency_Id",
                table: "Demands",
                newName: "CompetencyId");

            migrationBuilder.RenameIndex(
                name: "IX_Demands_Shift_Type_Id",
                table: "Demands",
                newName: "IX_Demands_ShiftTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Demands_Department_Id",
                table: "Demands",
                newName: "IX_Demands_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Demands_Competency_Id",
                table: "Demands",
                newName: "IX_Demands_CompetencyId");

            migrationBuilder.RenameColumn(
                name: "Shift_Type_Id",
                table: "Assignments",
                newName: "ShiftTypeId");

            migrationBuilder.RenameColumn(
                name: "Roster_Version_Id",
                table: "Assignments",
                newName: "RosterVersionId");

            migrationBuilder.RenameColumn(
                name: "Employee_Id",
                table: "Assignments",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_Shift_Type_Id",
                table: "Assignments",
                newName: "IX_Assignments_ShiftTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_Roster_Version_Id",
                table: "Assignments",
                newName: "IX_Assignments_RosterVersionId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_Employee_Id",
                table: "Assignments",
                newName: "IX_Assignments_EmployeeId");

            migrationBuilder.CreateTable(
                name: "AuditEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Entity = table.Column<string>(type: "text", nullable: false),
                    EntityId = table.Column<int>(type: "integer", nullable: false),
                    Operation = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangesJson = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeCompetencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    CompetencyId = table.Column<int>(type: "integer", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCompetencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeCompetencies_Competencies_CompetencyId",
                        column: x => x.CompetencyId,
                        principalTable: "Competencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeCompetencies_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RosterVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HorizonId = table.Column<int>(type: "integer", nullable: false),
                    VersionNumber = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ObjectiveValue = table.Column<double>(type: "double precision", nullable: true),
                    SolutionMetadataJson = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RosterVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RosterVersions_Horizons_HorizonId",
                        column: x => x.HorizonId,
                        principalTable: "Horizons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShiftTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    IsNightShift = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShiftTypes_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DemandTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    ShiftTypeId = table.Column<int>(type: "integer", nullable: false),
                    CompetencyId = table.Column<int>(type: "integer", nullable: true),
                    MinHeadcount = table.Column<int>(type: "integer", nullable: false),
                    MaxHeadcount = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemandTemplates_Competencies_CompetencyId",
                        column: x => x.CompetencyId,
                        principalTable: "Competencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DemandTemplates_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DemandTemplates_ShiftTypes_ShiftTypeId",
                        column: x => x.ShiftTypeId,
                        principalTable: "ShiftTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemandTemplates_CompetencyId",
                table: "DemandTemplates",
                column: "CompetencyId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandTemplates_DepartmentId",
                table: "DemandTemplates",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandTemplates_ShiftTypeId",
                table: "DemandTemplates",
                column: "ShiftTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCompetencies_CompetencyId",
                table: "EmployeeCompetencies",
                column: "CompetencyId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCompetencies_EmployeeId",
                table: "EmployeeCompetencies",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RosterVersions_HorizonId",
                table: "RosterVersions",
                column: "HorizonId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftTypes_DepartmentId",
                table: "ShiftTypes",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Employees_EmployeeId",
                table: "Assignments",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_RosterVersions_RosterVersionId",
                table: "Assignments",
                column: "RosterVersionId",
                principalTable: "RosterVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_ShiftTypes_ShiftTypeId",
                table: "Assignments",
                column: "ShiftTypeId",
                principalTable: "ShiftTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Demands_Competencies_CompetencyId",
                table: "Demands",
                column: "CompetencyId",
                principalTable: "Competencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Demands_Department_DepartmentId",
                table: "Demands",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Demands_ShiftTypes_ShiftTypeId",
                table: "Demands",
                column: "ShiftTypeId",
                principalTable: "ShiftTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Department_ParentId",
                table: "Department",
                column: "ParentId",
                principalTable: "Department",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Department_DepartmentId",
                table: "Employees",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Horizons_Department_DepartmentId",
                table: "Horizons",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Employees_EmployeeId",
                table: "Requests",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_ShiftTypes_ShiftTypeId",
                table: "Requests",
                column: "ShiftTypeId",
                principalTable: "ShiftTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Department_DepartmentId",
                table: "Rules",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Unavailabilities_Employees_EmployeeId",
                table: "Unavailabilities",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Employees_EmployeeId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_RosterVersions_RosterVersionId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_ShiftTypes_ShiftTypeId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Demands_Competencies_CompetencyId",
                table: "Demands");

            migrationBuilder.DropForeignKey(
                name: "FK_Demands_Department_DepartmentId",
                table: "Demands");

            migrationBuilder.DropForeignKey(
                name: "FK_Demands_ShiftTypes_ShiftTypeId",
                table: "Demands");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_Department_ParentId",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Department_DepartmentId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Horizons_Department_DepartmentId",
                table: "Horizons");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Employees_EmployeeId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_ShiftTypes_ShiftTypeId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Department_DepartmentId",
                table: "Rules");

            migrationBuilder.DropForeignKey(
                name: "FK_Unavailabilities_Employees_EmployeeId",
                table: "Unavailabilities");

            migrationBuilder.DropTable(
                name: "AuditEntries");

            migrationBuilder.DropTable(
                name: "DemandTemplates");

            migrationBuilder.DropTable(
                name: "EmployeeCompetencies");

            migrationBuilder.DropTable(
                name: "RosterVersions");

            migrationBuilder.DropTable(
                name: "ShiftTypes");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Unavailabilities",
                newName: "Start_Date");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Unavailabilities",
                newName: "End_Date");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Unavailabilities",
                newName: "Employee_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Unavailabilities_EmployeeId",
                table: "Unavailabilities",
                newName: "IX_Unavailabilities_Employee_Id");

            migrationBuilder.RenameColumn(
                name: "HorizonId",
                table: "SolverRuns",
                newName: "Horizon_Id");

            migrationBuilder.RenameColumn(
                name: "TypeCode",
                table: "Rules",
                newName: "Type_Code");

            migrationBuilder.RenameColumn(
                name: "ScopeRefId",
                table: "Rules",
                newName: "Scope_Ref_Id");

            migrationBuilder.RenameColumn(
                name: "ParametersJson",
                table: "Rules",
                newName: "Parameters_Json");

            migrationBuilder.RenameColumn(
                name: "IsHard",
                table: "Rules",
                newName: "Is_Hard");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Rules",
                newName: "Is_Active");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Rules",
                newName: "Department_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Rules_DepartmentId",
                table: "Rules",
                newName: "IX_Rules_Department_Id");

            migrationBuilder.RenameColumn(
                name: "ShiftTypeId",
                table: "Requests",
                newName: "Shift_Type_Id");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Requests",
                newName: "Employee_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_ShiftTypeId",
                table: "Requests",
                newName: "IX_Requests_Shift_Type_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_EmployeeId",
                table: "Requests",
                newName: "IX_Requests_Employee_Id");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Horizons",
                newName: "Start_Date");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Horizons",
                newName: "End_Date");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Horizons",
                newName: "Department_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Horizons_DepartmentId",
                table: "Horizons",
                newName: "IX_Horizons_Department_Id");

            migrationBuilder.RenameColumn(
                name: "ExitDate",
                table: "Employees",
                newName: "Exit_Date");

            migrationBuilder.RenameColumn(
                name: "EntryDate",
                table: "Employees",
                newName: "Entry_Date");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Employees",
                newName: "Department_Id");

            migrationBuilder.RenameColumn(
                name: "ContractHoursPerWeek",
                table: "Employees",
                newName: "Contract_Hours_Per_Week");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                newName: "IX_Employees_Department_Id");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Department",
                newName: "Parent_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Department_ParentId",
                table: "Department",
                newName: "IX_Department_Parent_Id");

            migrationBuilder.RenameColumn(
                name: "ShiftTypeId",
                table: "Demands",
                newName: "Shift_Type_Id");

            migrationBuilder.RenameColumn(
                name: "MinHeadcount",
                table: "Demands",
                newName: "Min_Headcount");

            migrationBuilder.RenameColumn(
                name: "MaxHeadcount",
                table: "Demands",
                newName: "Max_Headcount");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Demands",
                newName: "Department_Id");

            migrationBuilder.RenameColumn(
                name: "CompetencyId",
                table: "Demands",
                newName: "Competency_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Demands_ShiftTypeId",
                table: "Demands",
                newName: "IX_Demands_Shift_Type_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Demands_DepartmentId",
                table: "Demands",
                newName: "IX_Demands_Department_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Demands_CompetencyId",
                table: "Demands",
                newName: "IX_Demands_Competency_Id");

            migrationBuilder.RenameColumn(
                name: "ShiftTypeId",
                table: "Assignments",
                newName: "Shift_Type_Id");

            migrationBuilder.RenameColumn(
                name: "RosterVersionId",
                table: "Assignments",
                newName: "Roster_Version_Id");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Assignments",
                newName: "Employee_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_ShiftTypeId",
                table: "Assignments",
                newName: "IX_Assignments_Shift_Type_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_RosterVersionId",
                table: "Assignments",
                newName: "IX_Assignments_Roster_Version_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_EmployeeId",
                table: "Assignments",
                newName: "IX_Assignments_Employee_Id");

            migrationBuilder.CreateTable(
                name: "Audit_Entries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChangesJson = table.Column<string>(type: "jsonb", nullable: false),
                    Entity = table.Column<string>(type: "text", nullable: false),
                    Entity_Id = table.Column<int>(type: "integer", nullable: false),
                    Operation = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    User_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit_Entries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee_Competencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Competency_Id = table.Column<int>(type: "integer", nullable: false),
                    Employee_Id = table.Column<int>(type: "integer", nullable: false),
                    Valid_From = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Valid_To = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee_Competencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Competencies_Competencies_Competency_Id",
                        column: x => x.Competency_Id,
                        principalTable: "Competencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_Competencies_Employees_Employee_Id",
                        column: x => x.Employee_Id,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roster_Versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Horizon_Id = table.Column<int>(type: "integer", nullable: false),
                    Created_By = table.Column<string>(type: "text", nullable: false),
                    Created_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Objective_Value = table.Column<double>(type: "double precision", nullable: true),
                    Solution_Metadata_Json = table.Column<string>(type: "jsonb", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Version_Number = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roster_Versions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roster_Versions_Horizons_Horizon_Id",
                        column: x => x.Horizon_Id,
                        principalTable: "Horizons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shift_Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Department_Id = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Duration_Minutes = table.Column<int>(type: "integer", nullable: false),
                    End_Time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Is_Night_Shift = table.Column<bool>(type: "boolean", nullable: false),
                    Start_Time = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shift_Types", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shift_Types_Department_Department_Id",
                        column: x => x.Department_Id,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Demand_Templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Competency_Id = table.Column<int>(type: "integer", nullable: true),
                    Department_Id = table.Column<int>(type: "integer", nullable: false),
                    Shift_Type_Id = table.Column<int>(type: "integer", nullable: false),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    Max_Headcount = table.Column<int>(type: "integer", nullable: true),
                    Min_Headcount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demand_Templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Demand_Templates_Competencies_Competency_Id",
                        column: x => x.Competency_Id,
                        principalTable: "Competencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Demand_Templates_Department_Department_Id",
                        column: x => x.Department_Id,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Demand_Templates_Shift_Types_Shift_Type_Id",
                        column: x => x.Shift_Type_Id,
                        principalTable: "Shift_Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Demand_Templates_Competency_Id",
                table: "Demand_Templates",
                column: "Competency_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_Templates_Department_Id",
                table: "Demand_Templates",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_Templates_Shift_Type_Id",
                table: "Demand_Templates",
                column: "Shift_Type_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Competencies_Competency_Id",
                table: "Employee_Competencies",
                column: "Competency_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Competencies_Employee_Id",
                table: "Employee_Competencies",
                column: "Employee_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Roster_Versions_Horizon_Id",
                table: "Roster_Versions",
                column: "Horizon_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Shift_Types_Department_Id",
                table: "Shift_Types",
                column: "Department_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Employees_Employee_Id",
                table: "Assignments",
                column: "Employee_Id",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Roster_Versions_Roster_Version_Id",
                table: "Assignments",
                column: "Roster_Version_Id",
                principalTable: "Roster_Versions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Shift_Types_Shift_Type_Id",
                table: "Assignments",
                column: "Shift_Type_Id",
                principalTable: "Shift_Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Demands_Competencies_Competency_Id",
                table: "Demands",
                column: "Competency_Id",
                principalTable: "Competencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Demands_Department_Department_Id",
                table: "Demands",
                column: "Department_Id",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Demands_Shift_Types_Shift_Type_Id",
                table: "Demands",
                column: "Shift_Type_Id",
                principalTable: "Shift_Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Department_Parent_Id",
                table: "Department",
                column: "Parent_Id",
                principalTable: "Department",
                principalColumn: "Id");

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
                name: "FK_Requests_Employees_Employee_Id",
                table: "Requests",
                column: "Employee_Id",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Shift_Types_Shift_Type_Id",
                table: "Requests",
                column: "Shift_Type_Id",
                principalTable: "Shift_Types",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Department_Department_Id",
                table: "Rules",
                column: "Department_Id",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Unavailabilities_Employees_Employee_Id",
                table: "Unavailabilities",
                column: "Employee_Id",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beosztasmester.Migrations;

public partial class ConvertIdsToGuids : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            ALTER TABLE "Assignments" DROP CONSTRAINT IF EXISTS "FK_Assignments_Employees_EmployeeId";
            ALTER TABLE "Assignments" DROP CONSTRAINT IF EXISTS "FK_Assignments_RosterVersions_RosterVersionId";
            ALTER TABLE "Assignments" DROP CONSTRAINT IF EXISTS "FK_Assignments_ShiftTypes_ShiftTypeId";
            ALTER TABLE "Demands" DROP CONSTRAINT IF EXISTS "FK_Demands_Competencies_CompetencyId";
            ALTER TABLE "Demands" DROP CONSTRAINT IF EXISTS "FK_Demands_Department_DepartmentId";
            ALTER TABLE "Demands" DROP CONSTRAINT IF EXISTS "FK_Demands_ShiftTypes_ShiftTypeId";
            ALTER TABLE "Department" DROP CONSTRAINT IF EXISTS "FK_Department_Department_ParentId";
            ALTER TABLE "DemandTemplates" DROP CONSTRAINT IF EXISTS "FK_DemandTemplates_Competencies_CompetencyId";
            ALTER TABLE "DemandTemplates" DROP CONSTRAINT IF EXISTS "FK_DemandTemplates_Department_DepartmentId";
            ALTER TABLE "DemandTemplates" DROP CONSTRAINT IF EXISTS "FK_DemandTemplates_ShiftTypes_ShiftTypeId";
            ALTER TABLE "Employees" DROP CONSTRAINT IF EXISTS "FK_Employees_Department_DepartmentId";
            ALTER TABLE "EmployeeCompetencies" DROP CONSTRAINT IF EXISTS "FK_EmployeeCompetencies_Competencies_CompetencyId";
            ALTER TABLE "EmployeeCompetencies" DROP CONSTRAINT IF EXISTS "FK_EmployeeCompetencies_Employees_EmployeeId";
            ALTER TABLE "Horizons" DROP CONSTRAINT IF EXISTS "FK_Horizons_Department_DepartmentId";
            ALTER TABLE "Requests" DROP CONSTRAINT IF EXISTS "FK_Requests_Employees_EmployeeId";
            ALTER TABLE "Requests" DROP CONSTRAINT IF EXISTS "FK_Requests_ShiftTypes_ShiftTypeId";
            ALTER TABLE "RosterVersions" DROP CONSTRAINT IF EXISTS "FK_RosterVersions_Horizons_HorizonId";
            ALTER TABLE "Rules" DROP CONSTRAINT IF EXISTS "FK_Rules_Department_DepartmentId";
            ALTER TABLE "ShiftTypes" DROP CONSTRAINT IF EXISTS "FK_ShiftTypes_Department_DepartmentId";
            ALTER TABLE "Unavailabilities" DROP CONSTRAINT IF EXISTS "FK_Unavailabilities_Employees_EmployeeId";

            ALTER TABLE "Assignments" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
            ALTER TABLE "AuditEntries" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
            ALTER TABLE "Competencies" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
            ALTER TABLE "DemandTemplates" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
            ALTER TABLE "Demands" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
            ALTER TABLE "Department" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
            ALTER TABLE "Employees" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
            ALTER TABLE "EmployeeCompetencies" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
            ALTER TABLE "Horizons" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
            ALTER TABLE "Requests" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
            ALTER TABLE "RosterVersions" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
            ALTER TABLE "Rules" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
            ALTER TABLE "ShiftTypes" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
            ALTER TABLE "SolverRuns" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
            ALTER TABLE "Unavailabilities" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;

            ALTER TABLE "Assignments" ALTER COLUMN "Id" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("Id"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "Assignments" ALTER COLUMN "RosterVersionId" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("RosterVersionId"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "Assignments" ALTER COLUMN "EmployeeId" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("EmployeeId"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "Assignments" ALTER COLUMN "ShiftTypeId" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("ShiftTypeId"::bigint & 4294967295), 12, '0'))::uuid;

            ALTER TABLE "AuditEntries" ALTER COLUMN "Id" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("Id"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "AuditEntries" ALTER COLUMN "EntityId" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("EntityId"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "AuditEntries" ALTER COLUMN "UserId" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("UserId"::bigint & 4294967295), 12, '0'))::uuid;

            ALTER TABLE "Competencies" ALTER COLUMN "Id" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("Id"::bigint & 4294967295), 12, '0'))::uuid;

            ALTER TABLE "DemandTemplates" ALTER COLUMN "Id" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("Id"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "DemandTemplates" ALTER COLUMN "DepartmentId" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("DepartmentId"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "DemandTemplates" ALTER COLUMN "ShiftTypeId" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("ShiftTypeId"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "DemandTemplates" ALTER COLUMN "CompetencyId" TYPE uuid USING CASE WHEN "CompetencyId" IS NULL THEN NULL ELSE ('00000000-0000-0000-0000-' || lpad(to_hex("CompetencyId"::bigint & 4294967295), 12, '0'))::uuid END;

            ALTER TABLE "Demands" ALTER COLUMN "Id" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("Id"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "Demands" ALTER COLUMN "DepartmentId" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("DepartmentId"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "Demands" ALTER COLUMN "ShiftTypeId" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("ShiftTypeId"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "Demands" ALTER COLUMN "CompetencyId" TYPE uuid USING CASE WHEN "CompetencyId" IS NULL THEN NULL ELSE ('00000000-0000-0000-0000-' || lpad(to_hex("CompetencyId"::bigint & 4294967295), 12, '0'))::uuid END;

            ALTER TABLE "Department" ALTER COLUMN "Id" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("Id"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "Department" ALTER COLUMN "ParentId" TYPE uuid USING CASE WHEN "ParentId" IS NULL THEN NULL ELSE ('00000000-0000-0000-0000-' || lpad(to_hex("ParentId"::bigint & 4294967295), 12, '0'))::uuid END;

            ALTER TABLE "Employees" ALTER COLUMN "Id" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("Id"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "Employees" ALTER COLUMN "DepartmentId" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("DepartmentId"::bigint & 4294967295), 12, '0'))::uuid;

            ALTER TABLE "EmployeeCompetencies" ALTER COLUMN "Id" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("Id"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "EmployeeCompetencies" ALTER COLUMN "EmployeeId" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("EmployeeId"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "EmployeeCompetencies" ALTER COLUMN "CompetencyId" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("CompetencyId"::bigint & 4294967295), 12, '0'))::uuid;

            ALTER TABLE "Horizons" ALTER COLUMN "Id" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("Id"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "Horizons" ALTER COLUMN "DepartmentId" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("DepartmentId"::bigint & 4294967295), 12, '0'))::uuid;

            ALTER TABLE "Requests" ALTER COLUMN "Id" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("Id"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "Requests" ALTER COLUMN "EmployeeId" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("EmployeeId"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "Requests" ALTER COLUMN "ShiftTypeId" TYPE uuid USING CASE WHEN "ShiftTypeId" IS NULL THEN NULL ELSE ('00000000-0000-0000-0000-' || lpad(to_hex("ShiftTypeId"::bigint & 4294967295), 12, '0'))::uuid END;

            ALTER TABLE "RosterVersions" ALTER COLUMN "Id" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("Id"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "RosterVersions" ALTER COLUMN "HorizonId" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("HorizonId"::bigint & 4294967295), 12, '0'))::uuid;

            ALTER TABLE "Rules" ALTER COLUMN "Id" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("Id"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "Rules" ALTER COLUMN "DepartmentId" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("DepartmentId"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "Rules" ALTER COLUMN "ScopeRefId" TYPE uuid USING CASE WHEN "ScopeRefId" IS NULL THEN NULL ELSE ('00000000-0000-0000-0000-' || lpad(to_hex("ScopeRefId"::bigint & 4294967295), 12, '0'))::uuid END;

            ALTER TABLE "ShiftTypes" ALTER COLUMN "Id" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("Id"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "ShiftTypes" ALTER COLUMN "DepartmentId" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("DepartmentId"::bigint & 4294967295), 12, '0'))::uuid;

            ALTER TABLE "SolverRuns" ALTER COLUMN "Id" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("Id"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "SolverRuns" ALTER COLUMN "HorizonId" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("HorizonId"::bigint & 4294967295), 12, '0'))::uuid;

            ALTER TABLE "Unavailabilities" ALTER COLUMN "Id" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("Id"::bigint & 4294967295), 12, '0'))::uuid;
            ALTER TABLE "Unavailabilities" ALTER COLUMN "EmployeeId" TYPE uuid USING ('00000000-0000-0000-0000-' || lpad(to_hex("EmployeeId"::bigint & 4294967295), 12, '0'))::uuid;
            """);

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
            name: "FK_DemandTemplates_Competencies_CompetencyId",
            table: "DemandTemplates",
            column: "CompetencyId",
            principalTable: "Competencies",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_DemandTemplates_Department_DepartmentId",
            table: "DemandTemplates",
            column: "DepartmentId",
            principalTable: "Department",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_DemandTemplates_ShiftTypes_ShiftTypeId",
            table: "DemandTemplates",
            column: "ShiftTypeId",
            principalTable: "ShiftTypes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Employees_Department_DepartmentId",
            table: "Employees",
            column: "DepartmentId",
            principalTable: "Department",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_EmployeeCompetencies_Competencies_CompetencyId",
            table: "EmployeeCompetencies",
            column: "CompetencyId",
            principalTable: "Competencies",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_EmployeeCompetencies_Employees_EmployeeId",
            table: "EmployeeCompetencies",
            column: "EmployeeId",
            principalTable: "Employees",
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
            name: "FK_RosterVersions_Horizons_HorizonId",
            table: "RosterVersions",
            column: "HorizonId",
            principalTable: "Horizons",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Rules_Department_DepartmentId",
            table: "Rules",
            column: "DepartmentId",
            principalTable: "Department",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ShiftTypes_Department_DepartmentId",
            table: "ShiftTypes",
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

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        throw new NotSupportedException(
            "GUID IDs cannot be converted back to the original integer IDs without losing identity.");
    }
}

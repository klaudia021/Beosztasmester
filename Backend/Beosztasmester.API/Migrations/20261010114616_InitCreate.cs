using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beosztasmester.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Audit_Entries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Entity = table.Column<string>(type: "text", nullable: false),
                    Entity_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Operation = table.Column<string>(type: "text", nullable: false),
                    User_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangesJson = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit_Entries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Competencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competencies", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Department_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Contract_Hours_Per_Week = table.Column<int>(type: "integer", nullable: false),
                    Entry_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Exit_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Department_Department_Id",
                        column: x => x.Department_Id,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Horizons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Department_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Start_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    End_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horizons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Horizons_Department_Department_Id",
                        column: x => x.Department_Id,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Department_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type_Code = table.Column<string>(type: "text", nullable: false),
                    Scope = table.Column<string>(type: "text", nullable: false),
                    Scope_Ref_Id = table.Column<Guid>(type: "uuid", nullable: true),
                    Is_Hard = table.Column<bool>(type: "boolean", nullable: false),
                    Weight = table.Column<int>(type: "integer", nullable: false),
                    Parameters_Json = table.Column<string>(type: "jsonb", nullable: false),
                    Is_Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rules_Department_Department_Id",
                        column: x => x.Department_Id,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shift_Types",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Department_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Start_Time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    End_Time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Duration_Minutes = table.Column<int>(type: "integer", nullable: false),
                    Is_Night_Shift = table.Column<bool>(type: "boolean", nullable: false)
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
                name: "Employee_Competencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Employee_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Competency_Id = table.Column<Guid>(type: "uuid", nullable: false),
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
                name: "Unavailabilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Employee_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Start_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    End_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unavailabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Unavailabilities_Employees_Employee_Id",
                        column: x => x.Employee_Id,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roster_Versions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Horizon_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Version_Number = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Created_By = table.Column<string>(type: "text", nullable: false),
                    Created_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Objective_Value = table.Column<double>(type: "double precision", nullable: true),
                    Solution_Metadata_Json = table.Column<string>(type: "jsonb", nullable: true)
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
                name: "Demand_Templates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Department_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    Shift_Type_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Competency_Id = table.Column<Guid>(type: "uuid", nullable: true),
                    Min_Headcount = table.Column<int>(type: "integer", nullable: false),
                    Max_Headcount = table.Column<int>(type: "integer", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Demands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Department_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Shift_Type_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Competency_Id = table.Column<Guid>(type: "uuid", nullable: true),
                    Min_Headcount = table.Column<int>(type: "integer", nullable: false),
                    Max_Headcount = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Demands_Competencies_Competency_Id",
                        column: x => x.Competency_Id,
                        principalTable: "Competencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Demands_Department_Department_Id",
                        column: x => x.Department_Id,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Demands_Shift_Types_Shift_Type_Id",
                        column: x => x.Shift_Type_Id,
                        principalTable: "Shift_Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Employee_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Shift_Type_Id = table.Column<Guid>(type: "uuid", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_Employees_Employee_Id",
                        column: x => x.Employee_Id,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_Shift_Types_Shift_Type_Id",
                        column: x => x.Shift_Type_Id,
                        principalTable: "Shift_Types",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Roster_Version_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Employee_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Shift_Type_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsLocked = table.Column<bool>(type: "boolean", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_Employees_Employee_Id",
                        column: x => x.Employee_Id,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_Roster_Versions_Roster_Version_Id",
                        column: x => x.Roster_Version_Id,
                        principalTable: "Roster_Versions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_Shift_Types_Shift_Type_Id",
                        column: x => x.Shift_Type_Id,
                        principalTable: "Shift_Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_Employee_Id",
                table: "Assignments",
                column: "Employee_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_Roster_Version_Id",
                table: "Assignments",
                column: "Roster_Version_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_Shift_Type_Id",
                table: "Assignments",
                column: "Shift_Type_Id");

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
                name: "IX_Demands_Competency_Id",
                table: "Demands",
                column: "Competency_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Demands_Department_Id",
                table: "Demands",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Demands_Shift_Type_Id",
                table: "Demands",
                column: "Shift_Type_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Department_Parent_Id",
                table: "Department",
                column: "Parent_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Competencies_Competency_Id",
                table: "Employee_Competencies",
                column: "Competency_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Competencies_Employee_Id",
                table: "Employee_Competencies",
                column: "Employee_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Department_Id",
                table: "Employees",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Horizons_Department_Id",
                table: "Horizons",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_Employee_Id",
                table: "Requests",
                column: "Employee_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_Shift_Type_Id",
                table: "Requests",
                column: "Shift_Type_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Roster_Versions_Horizon_Id",
                table: "Roster_Versions",
                column: "Horizon_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_Department_Id",
                table: "Rules",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Shift_Types_Department_Id",
                table: "Shift_Types",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Unavailabilities_Employee_Id",
                table: "Unavailabilities",
                column: "Employee_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "Audit_Entries");

            migrationBuilder.DropTable(
                name: "Demand_Templates");

            migrationBuilder.DropTable(
                name: "Demands");

            migrationBuilder.DropTable(
                name: "Employee_Competencies");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropTable(
                name: "Unavailabilities");

            migrationBuilder.DropTable(
                name: "Roster_Versions");

            migrationBuilder.DropTable(
                name: "Competencies");

            migrationBuilder.DropTable(
                name: "Shift_Types");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Horizons");

            migrationBuilder.DropTable(
                name: "Department");
        }
    }
}

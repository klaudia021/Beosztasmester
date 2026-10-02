using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beosztasmester.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRuleScopeRefToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Scope_Ref",
                table: "Rules");

            migrationBuilder.AddColumn<Guid>(
                name: "Scope_Ref_Id",
                table: "Rules",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Scope_Ref_Id",
                table: "Rules");

            migrationBuilder.AddColumn<string>(
                name: "Scope_Ref",
                table: "Rules",
                type: "text",
                nullable: true);
        }
    }
}

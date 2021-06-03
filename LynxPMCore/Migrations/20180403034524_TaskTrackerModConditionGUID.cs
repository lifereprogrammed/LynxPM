using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LynxPMCore.Migrations
{
    public partial class TaskTrackerModConditionGUID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTrackers_Conditions_ConditionID",
                table: "TaskTrackers");

            migrationBuilder.AlterColumn<Guid>(
                name: "ConditionID",
                table: "TaskTrackers",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTrackers_Conditions_ConditionID",
                table: "TaskTrackers",
                column: "ConditionID",
                principalTable: "Conditions",
                principalColumn: "ConditionID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTrackers_Conditions_ConditionID",
                table: "TaskTrackers");

            migrationBuilder.AlterColumn<Guid>(
                name: "ConditionID",
                table: "TaskTrackers",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTrackers_Conditions_ConditionID",
                table: "TaskTrackers",
                column: "ConditionID",
                principalTable: "Conditions",
                principalColumn: "ConditionID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

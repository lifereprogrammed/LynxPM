using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LynxPMCore.Migrations
{
    public partial class InitializeDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    AreaID = table.Column<Guid>(nullable: false),
                    AreaActive = table.Column<bool>(nullable: false),
                    AreaAppearanceOrder = table.Column<int>(nullable: false),
                    AreaDescription = table.Column<string>(nullable: true),
                    AreaKMLFileURL = table.Column<string>(nullable: true),
                    AreaName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.AreaID);
                });

            migrationBuilder.CreateTable(
                name: "Conditions",
                columns: table => new
                {
                    ConditionID = table.Column<Guid>(nullable: false),
                    ConditionDescription = table.Column<string>(nullable: true),
                    ConditionDisplayLetter = table.Column<string>(nullable: true),
                    ConditionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conditions", x => x.ConditionID);
                });

            migrationBuilder.CreateTable(
                name: "DueStatuses",
                columns: table => new
                {
                    DueStatusID = table.Column<Guid>(nullable: false),
                    DueStatusName = table.Column<string>(nullable: false),
                    DustStatusDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DueStatuses", x => x.DueStatusID);
                });

            migrationBuilder.CreateTable(
                name: "TaskTrackerStages",
                columns: table => new
                {
                    TaskTrackerStageID = table.Column<Guid>(nullable: false),
                    TaskTrackerStageDescription = table.Column<string>(nullable: true),
                    TaskTrackerStageName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTrackerStages", x => x.TaskTrackerStageID);
                });

            migrationBuilder.CreateTable(
                name: "TaskTypes",
                columns: table => new
                {
                    TaskTypeID = table.Column<Guid>(nullable: false),
                    TaskTypeDescription = table.Column<string>(nullable: true),
                    TaskTypeDisplayLetter = table.Column<string>(maxLength: 1, nullable: false),
                    TaskTypeName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypes", x => x.TaskTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Terms",
                columns: table => new
                {
                    TermID = table.Column<Guid>(nullable: false),
                    TermDays = table.Column<int>(nullable: false),
                    TermDescription = table.Column<string>(nullable: true),
                    TermLeadTime = table.Column<int>(nullable: false),
                    TermName = table.Column<string>(nullable: false),
                    TermShortName = table.Column<string>(maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terms", x => x.TermID);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentAreas",
                columns: table => new
                {
                    EquipmentAreaID = table.Column<Guid>(nullable: false),
                    AreaID = table.Column<Guid>(nullable: false),
                    EquipmentAreaActive = table.Column<bool>(nullable: false),
                    EquipmentAreaAppearanceOrder = table.Column<string>(nullable: true),
                    EquipmentAreaDescription = table.Column<string>(nullable: true),
                    EquipmentAreaName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentAreas", x => x.EquipmentAreaID);
                    table.ForeignKey(
                        name: "FK_EquipmentAreas_Areas_AreaID",
                        column: x => x.AreaID,
                        principalTable: "Areas",
                        principalColumn: "AreaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    EquipmentID = table.Column<Guid>(nullable: false),
                    AreaID = table.Column<Guid>(nullable: true),
                    EquipmentActive = table.Column<bool>(nullable: false),
                    EquipmentAppearance = table.Column<int>(nullable: false),
                    EquipmentAreaID = table.Column<Guid>(nullable: false),
                    EquipmentDescription = table.Column<string>(nullable: true),
                    EquipmentName = table.Column<string>(nullable: true),
                    EquipmentPictureID = table.Column<string>(nullable: true),
                    EquipmentPictureURL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.EquipmentID);
                    table.ForeignKey(
                        name: "FK_Equipments_Areas_AreaID",
                        column: x => x.AreaID,
                        principalTable: "Areas",
                        principalColumn: "AreaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equipments_EquipmentAreas_EquipmentAreaID",
                        column: x => x.EquipmentAreaID,
                        principalTable: "EquipmentAreas",
                        principalColumn: "EquipmentAreaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LTasks",
                columns: table => new
                {
                    LTaskID = table.Column<Guid>(nullable: false),
                    EquipmentID = table.Column<Guid>(nullable: false),
                    LTaskDescription = table.Column<string>(nullable: true),
                    LTaskName = table.Column<string>(nullable: false),
                    TaskTypeID = table.Column<Guid>(nullable: false),
                    TermID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LTasks", x => x.LTaskID);
                    table.ForeignKey(
                        name: "FK_LTasks_Equipments_EquipmentID",
                        column: x => x.EquipmentID,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LTasks_TaskTypes_TaskTypeID",
                        column: x => x.TaskTypeID,
                        principalTable: "TaskTypes",
                        principalColumn: "TaskTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LTasks_Terms_TermID",
                        column: x => x.TermID,
                        principalTable: "Terms",
                        principalColumn: "TermID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskTrackers",
                columns: table => new
                {
                    TaskTrackerID = table.Column<Guid>(nullable: false),
                    ConditionID = table.Column<Guid>(nullable: false),
                    LTaskID = table.Column<Guid>(nullable: false),
                    TaskRecordUserIP = table.Column<string>(nullable: true),
                    TaskTrackerComments = table.Column<string>(nullable: true),
                    TaskTrackerCompletionDate = table.Column<string>(nullable: true),
                    TaskTrackerDateStamp = table.Column<string>(nullable: true),
                    TaskTrackerDaystoComplete = table.Column<int>(nullable: false),
                    TaskTrackerExpectedCompletionDate = table.Column<string>(nullable: true),
                    TaskTrackerPreviousCompletionDate = table.Column<string>(nullable: true),
                    TaskTrackerRecordUser = table.Column<string>(nullable: true),
                    TaskTrackerSessionID = table.Column<Guid>(nullable: false),
                    TaskTrackerStageID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTrackers", x => x.TaskTrackerID);
                    table.ForeignKey(
                        name: "FK_TaskTrackers_Conditions_ConditionID",
                        column: x => x.ConditionID,
                        principalTable: "Conditions",
                        principalColumn: "ConditionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskTrackers_LTasks_LTaskID",
                        column: x => x.LTaskID,
                        principalTable: "LTasks",
                        principalColumn: "LTaskID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskTrackers_TaskTrackerStages_TaskTrackerStageID",
                        column: x => x.TaskTrackerStageID,
                        principalTable: "TaskTrackerStages",
                        principalColumn: "TaskTrackerStageID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentAreas_AreaID",
                table: "EquipmentAreas",
                column: "AreaID");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_AreaID",
                table: "Equipments",
                column: "AreaID");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_EquipmentAreaID",
                table: "Equipments",
                column: "EquipmentAreaID");

            migrationBuilder.CreateIndex(
                name: "IX_LTasks_EquipmentID",
                table: "LTasks",
                column: "EquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_LTasks_TaskTypeID",
                table: "LTasks",
                column: "TaskTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LTasks_TermID",
                table: "LTasks",
                column: "TermID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTrackers_ConditionID",
                table: "TaskTrackers",
                column: "ConditionID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTrackers_LTaskID",
                table: "TaskTrackers",
                column: "LTaskID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTrackers_TaskTrackerStageID",
                table: "TaskTrackers",
                column: "TaskTrackerStageID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DueStatuses");

            migrationBuilder.DropTable(
                name: "TaskTrackers");

            migrationBuilder.DropTable(
                name: "Conditions");

            migrationBuilder.DropTable(
                name: "LTasks");

            migrationBuilder.DropTable(
                name: "TaskTrackerStages");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "TaskTypes");

            migrationBuilder.DropTable(
                name: "Terms");

            migrationBuilder.DropTable(
                name: "EquipmentAreas");

            migrationBuilder.DropTable(
                name: "Areas");
        }
    }
}

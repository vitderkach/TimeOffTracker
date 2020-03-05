using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TOT.Data.Migrations
{
    // This code has been generated automatically using 'add-migration  AddTablesAndFieldsRelatedWithUserInformation' command in PM manager
    /* A short explanation: this migration adds new tables Location and Team, new IsFired, RecruitmentDate fields for UserInformation table add Year for VacationPolicy, 
       default value for UsedDays column  from VacationType table, changes relation between UserInformation and VacationPolicy as one-to-many instead one-to-one
       adds some cosmetics changes, which connected with names of tables and fileds(VacationPolicyInfo instead VacationPolicy, UsedDays instead WastedDays etc.)*/
    public partial class AddTablesAndFieldsRelatedWithUserInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacationTypes_VacationPolicies_VacationPolicyInfoId",
                table: "VacationTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VacationTypes",
                table: "VacationTypes");

            migrationBuilder.DropIndex(
                name: "IX_VacationTypes_VacationPolicyInfoId",
                table: "VacationTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VacationPolicies",
                table: "VacationPolicies");

            migrationBuilder.DropIndex(
                name: "IX_VacationPolicies_UserInformationId",
                table: "VacationPolicies");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "VacationTypes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "VacationPolicies");

            migrationBuilder.RenameColumn(
                name: "TimeOffType",
                table: "VacationTypes",
                newName: "Name");

            // This part of code has been changed manually: the value of name is UsedDays instead StatutoryDays
            migrationBuilder.RenameColumn(
                name: "WastedDays",
                table: "VacationTypes",
                newName: "UsedDays");

            // This part of code has been added manually: add a default value is '0' for column UsedDays 
            migrationBuilder.Sql(
                @"ALTER TABLE VacationTypes
                ADD CONSTRAINT DF_UsedDays
                DEFAULT 0 FOR UsedDays;"
                );

            migrationBuilder.RenameColumn(
                name: "VacationPolicyInfoId",
                table: "VacationTypes",
                newName: "VacationPolicyId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "VacationRequests",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_VacationRequests_UserId",
                table: "VacationRequests",
                newName: "IX_VacationRequests_ApplicationUserId");

            migrationBuilder.RenameColumn(
                name: "UserInformationId",
                table: "UserInformations",
                newName: "ApplicationUserId");

            // This part of code has been changed manually: the value of name StatutoryDays instead UsedDays
            migrationBuilder.AddColumn<int>(
                name: "StatutoryDays",
                table: "VacationTypes",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "VacationPolicyUserInformationId",
                table: "VacationTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VacationPolicyYear",
                table: "VacationTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "VacationPolicies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsFired",
                table: "UserInformations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "UserInformations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RecruitmentDate",
                table: "UserInformations",
                type: "date",
                nullable: true,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "UserInformations",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ VacationType",
                table: "VacationTypes",
                columns: new[] { "VacationPolicyId", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_VacationPolicy",
                table: "VacationPolicies",
                columns: new[] { "UserInformationId", "Year" });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VacationTypes_VacationPolicyUserInformationId_VacationPolicyYear",
                table: "VacationTypes",
                columns: new[] { "VacationPolicyUserInformationId", "VacationPolicyYear" });

            migrationBuilder.CreateIndex(
                name: "IX_UserInformations_LocationId",
                table: "UserInformations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInformations_TeamId",
                table: "UserInformations",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInformations_Location_LocationId",
                table: "UserInformations",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInformations_Team_TeamId",
                table: "UserInformations",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationTypes_VacationPolicies_VacationPolicyUserInformationId_VacationPolicyYear",
                table: "VacationTypes",
                columns: new[] { "VacationPolicyUserInformationId", "VacationPolicyYear" },
                principalTable: "VacationPolicies",
                principalColumns: new[] { "UserInformationId", "Year" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInformations_Location_LocationId",
                table: "UserInformations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInformations_Team_TeamId",
                table: "UserInformations");

            migrationBuilder.DropForeignKey(
                name: "FK_VacationTypes_VacationPolicies_VacationPolicyUserInformationId_VacationPolicyYear",
                table: "VacationTypes");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ VacationType",
                table: "VacationTypes");

            migrationBuilder.DropIndex(
                name: "IX_VacationTypes_VacationPolicyUserInformationId_VacationPolicyYear",
                table: "VacationTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VacationPolicy",
                table: "VacationPolicies");

            migrationBuilder.DropIndex(
                name: "IX_UserInformations_LocationId",
                table: "UserInformations");

            migrationBuilder.DropIndex(
                name: "IX_UserInformations_TeamId",
                table: "UserInformations");

            // This part of code has been changed manually: the value of name is StatutoryDays instead UsedDays
            migrationBuilder.DropColumn(
                name: "StatutoryDays",
                table: "VacationTypes");

            migrationBuilder.DropColumn(
                name: "VacationPolicyUserInformationId",
                table: "VacationTypes");

            migrationBuilder.DropColumn(
                name: "VacationPolicyYear",
                table: "VacationTypes");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "VacationPolicies");

            migrationBuilder.DropColumn(
                name: "IsFired",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "RecruitmentDate",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "UserInformations");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "VacationTypes",
                newName: "TimeOffType");

            // This part of code has been added manually: delete the default constraint from column UsedDays
            migrationBuilder.Sql(
                @"ALTER TABLE VacationTypes
                DROP CONSTRAINT DF_UsedDays;"
                );

            // This part of code has been changed manually: the value of name is UsedDays instead StatutoryDays
            migrationBuilder.RenameColumn(
                name: "UsedDays",
                table: "VacationTypes",
                newName: "WastedDays");

            migrationBuilder.RenameColumn(
                name: "VacationPolicyId",
                table: "VacationTypes",
                newName: "VacationPolicyInfoId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "VacationRequests",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_VacationRequests_ApplicationUserId",
                table: "VacationRequests",
                newName: "IX_VacationRequests_UserId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "UserInformations",
                newName: "UserInformationId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "VacationTypes",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "VacationPolicies",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VacationTypes",
                table: "VacationTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VacationPolicies",
                table: "VacationPolicies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_VacationTypes_VacationPolicyInfoId",
                table: "VacationTypes",
                column: "VacationPolicyInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationPolicies_UserInformationId",
                table: "VacationPolicies",
                column: "UserInformationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationTypes_VacationPolicies_VacationPolicyInfoId",
                table: "VacationTypes",
                column: "VacationPolicyInfoId",
                principalTable: "VacationPolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

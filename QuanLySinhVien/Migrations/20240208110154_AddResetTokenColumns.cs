using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLySinhVien.Migrations
{
    /// <inheritdoc />
    public partial class AddResetTokenColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
        name: "Students",
        columns: table => new
        {
            StudentId = table.Column<int>(type: "int", nullable: false)
                .Annotation("SqlServer:Identity", "1, 1"),
            FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_Students", x => x.StudentId);
        });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    idUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResetToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResetTokenExpiration = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.idUser);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    GradeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    MaxScore = table.Column<int>(type: "int", nullable: false, defaultValue: 10)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.GradeId);
                    table.ForeignKey(
                        name: "FK_Grades_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                       onDelete: ReferentialAction.NoAction);
                });
            
            migrationBuilder.CreateTable(
                name: "individuals",
                columns: table => new
                {
                    IndividualId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    ParticipationInStudy = table.Column<bool>(type: "bit", nullable: false),
                    ComplianceWithSchoolRules = table.Column<bool>(type: "bit", nullable: false),
                    ParticipationInSocialActivities = table.Column<bool>(type: "bit", nullable: false),
                    CitizenshipInCommunity = table.Column<bool>(type: "bit", nullable: false),
                    ParticipationInOrganizations = table.Column<bool>(type: "bit", nullable: false),
                    Achievements = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GradeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_individuals", x => x.IndividualId);
                    table.ForeignKey(
                        name: "FK_individuals_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "GradeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_individuals_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                    table.UniqueConstraint("UC_Individuals_StudentId_GradeId", x => new { x.StudentId, x.GradeId });
                });
            
            migrationBuilder.CreateTable(
                name: "TeacherEvaluations",
                columns: table => new
                {
                    TeacherEvaluationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    EvaluationComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GradeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherEvaluations", x => x.TeacherEvaluationId);
                    table.ForeignKey(
                        name: "FK_TeacherEvaluations_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "GradeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherEvaluations_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });
            
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    TeamEvaluation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParticipationInStudy = table.Column<bool>(type: "bit", nullable: false),
                    ComplianceWithSchoolRules = table.Column<bool>(type: "bit", nullable: false),
                    ParticipationInSocialActivities = table.Column<bool>(type: "bit", nullable: false),
                    CitizenshipInCommunity = table.Column<bool>(type: "bit", nullable: false),
                    ParticipationInOrganizations = table.Column<bool>(type: "bit", nullable: false),
                    Achievements = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GradeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Teams_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "GradeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teams_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });
            
            migrationBuilder.CreateIndex(
                name: "IX_Grades_StudentId",
                table: "Grades",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_individuals_GradeId",
                table: "individuals",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_individuals_StudentId",
                table: "individuals",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherEvaluations_GradeId",
                table: "TeacherEvaluations",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherEvaluations_StudentId",
                table: "TeacherEvaluations",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_GradeId",
                table: "Teams",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_StudentId",
                table: "Teams",
                column: "StudentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "individuals");

            migrationBuilder.DropTable(
                name: "TeacherEvaluations");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}

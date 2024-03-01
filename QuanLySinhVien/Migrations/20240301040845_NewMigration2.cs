using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLySinhVien.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayDanhGia",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "XepLoai",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "NoiDungDanhGia",
                table: "Teachers",
                newName: "XepLoai");

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayDanhGia",
                table: "Teachers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Teachers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ManagerAdminId",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Manager",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MSSV = table.Column<int>(type: "int", nullable: false),
                    MSGV = table.Column<int>(type: "int", nullable: false),
                    TeacherMSGV = table.Column<int>(type: "int", nullable: false),
                    StudentMSSV = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.AdminId);
                    table.ForeignKey(
                        name: "FK_Manager_Students_StudentMSSV",
                        column: x => x.StudentMSSV,
                        principalTable: "Students",
                        principalColumn: "MSSV",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Manager_Teachers_TeacherMSGV",
                        column: x => x.TeacherMSGV,
                        principalTable: "Teachers",
                        principalColumn: "MSGV",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ManagerAdminId",
                table: "Reviews",
                column: "ManagerAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Manager_StudentMSSV",
                table: "Manager",
                column: "StudentMSSV");

            migrationBuilder.CreateIndex(
                name: "IX_Manager_TeacherMSGV",
                table: "Manager",
                column: "TeacherMSGV");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Manager_idUser",
                table: "AspNetUsers",
                column: "idUser",
                principalTable: "Manager",
                principalColumn: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Manager_ManagerAdminId",
                table: "Reviews",
                column: "ManagerAdminId",
                principalTable: "Manager",
                principalColumn: "AdminId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Manager_idUser",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Manager_ManagerAdminId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "Manager");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ManagerAdminId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "NgayDanhGia",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "ManagerAdminId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "XepLoai",
                table: "Teachers",
                newName: "NoiDungDanhGia");

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayDanhGia",
                table: "Reviews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "XepLoai",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLySinhVien.Migrations
{
    /// <inheritdoc />
    public partial class newDatabaseAgain2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MSGV",
                table: "review");

            migrationBuilder.DropColumn(
                name: "MSSV",
                table: "review");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "review");

            migrationBuilder.DropColumn(
                name: "XepLoai",
                table: "review");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MSGV",
                table: "review",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MSSV",
                table: "review",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "review",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "XepLoai",
                table: "review",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

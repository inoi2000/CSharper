using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSharper.Migrations
{
    /// <inheritdoc />
    public partial class ThirdDebaging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentUser_Assignments_AssignmentId",
                table: "AssignmentUser");

            migrationBuilder.RenameColumn(
                name: "AssignmentId",
                table: "AssignmentUser",
                newName: "AssignmentsId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Subjects",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentUser_Assignments_AssignmentsId",
                table: "AssignmentUser",
                column: "AssignmentsId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentUser_Assignments_AssignmentsId",
                table: "AssignmentUser");

            migrationBuilder.RenameColumn(
                name: "AssignmentsId",
                table: "AssignmentUser",
                newName: "AssignmentId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Subjects",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentUser_Assignments_AssignmentId",
                table: "AssignmentUser",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

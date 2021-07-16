using Microsoft.EntityFrameworkCore.Migrations;

namespace ContosoUniversity.Migrations
{
    public partial class EnrollmentIndexCS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Enrollment_CourseID",
                table: "Enrollment");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_CourseID_StudentID",
                table: "Enrollment",
                columns: new[] { "CourseID", "StudentID" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Enrollment_CourseID_StudentID",
                table: "Enrollment");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_CourseID",
                table: "Enrollment",
                column: "CourseID");
        }
    }
}

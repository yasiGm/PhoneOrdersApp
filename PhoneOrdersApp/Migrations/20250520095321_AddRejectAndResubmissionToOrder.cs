using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneOrdersApp.Migrations
{
    /// <inheritdoc />
    public partial class AddRejectAndResubmissionToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RejectReason",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResubmissionNote",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectReason",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ResubmissionNote",
                table: "Orders");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PracticeWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddingNumberOfGroupsInGroupModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NoOfGroups",
                table: "Groups",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoOfGroups",
                table: "Groups");
        }
    }
}

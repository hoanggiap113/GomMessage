using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GomMessage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_User_Tele : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "telephone",
                table: "users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "telephone",
                table: "users");
        }
    }
}

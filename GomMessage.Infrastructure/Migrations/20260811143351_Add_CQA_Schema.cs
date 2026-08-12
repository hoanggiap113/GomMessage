using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GomMessage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_CQA_Schema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cqa");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "users",
                newSchema: "cqa");

            migrationBuilder.RenameTable(
                name: "user_tenants",
                newName: "user_tenants",
                newSchema: "cqa");

            migrationBuilder.RenameTable(
                name: "tenants",
                newName: "tenants",
                newSchema: "cqa");

            migrationBuilder.RenameTable(
                name: "refresh_tokens",
                newName: "refresh_tokens",
                newSchema: "cqa");

            migrationBuilder.RenameTable(
                name: "notification_logs",
                newName: "notification_logs",
                newSchema: "cqa");

            migrationBuilder.RenameTable(
                name: "messages",
                newName: "messages",
                newSchema: "cqa");

            migrationBuilder.RenameTable(
                name: "jobs",
                newName: "jobs",
                newSchema: "cqa");

            migrationBuilder.RenameTable(
                name: "job_runs",
                newName: "job_runs",
                newSchema: "cqa");

            migrationBuilder.RenameTable(
                name: "job_results",
                newName: "job_results",
                newSchema: "cqa");

            migrationBuilder.RenameTable(
                name: "invitations",
                newName: "invitations",
                newSchema: "cqa");

            migrationBuilder.RenameTable(
                name: "conversations",
                newName: "conversations",
                newSchema: "cqa");

            migrationBuilder.RenameTable(
                name: "channels",
                newName: "channels",
                newSchema: "cqa");

            migrationBuilder.RenameTable(
                name: "app_settings",
                newName: "app_settings",
                newSchema: "cqa");

            migrationBuilder.RenameTable(
                name: "agents",
                newName: "agents",
                newSchema: "cqa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "users",
                schema: "cqa",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "user_tenants",
                schema: "cqa",
                newName: "user_tenants");

            migrationBuilder.RenameTable(
                name: "tenants",
                schema: "cqa",
                newName: "tenants");

            migrationBuilder.RenameTable(
                name: "refresh_tokens",
                schema: "cqa",
                newName: "refresh_tokens");

            migrationBuilder.RenameTable(
                name: "notification_logs",
                schema: "cqa",
                newName: "notification_logs");

            migrationBuilder.RenameTable(
                name: "messages",
                schema: "cqa",
                newName: "messages");

            migrationBuilder.RenameTable(
                name: "jobs",
                schema: "cqa",
                newName: "jobs");

            migrationBuilder.RenameTable(
                name: "job_runs",
                schema: "cqa",
                newName: "job_runs");

            migrationBuilder.RenameTable(
                name: "job_results",
                schema: "cqa",
                newName: "job_results");

            migrationBuilder.RenameTable(
                name: "invitations",
                schema: "cqa",
                newName: "invitations");

            migrationBuilder.RenameTable(
                name: "conversations",
                schema: "cqa",
                newName: "conversations");

            migrationBuilder.RenameTable(
                name: "channels",
                schema: "cqa",
                newName: "channels");

            migrationBuilder.RenameTable(
                name: "app_settings",
                schema: "cqa",
                newName: "app_settings");

            migrationBuilder.RenameTable(
                name: "agents",
                schema: "cqa",
                newName: "agents");
        }
    }
}

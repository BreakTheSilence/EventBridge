using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventBridge.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIdFromEventParticipants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "EventParticipants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "EventParticipants",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

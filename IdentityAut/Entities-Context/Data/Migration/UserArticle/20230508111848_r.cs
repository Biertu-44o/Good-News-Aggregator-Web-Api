#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities_Context.Data.Migration.UserArticle
{
    /// <inheritdoc />
    public partial class r : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HashUrlId",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashUrlId",
                table: "Articles");
        }
    }
}

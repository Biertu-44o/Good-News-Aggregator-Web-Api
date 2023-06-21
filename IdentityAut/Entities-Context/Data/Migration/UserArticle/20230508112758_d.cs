#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities_Context.Data.Migration.UserArticle
{
    /// <inheritdoc />
    public partial class d : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Articles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}

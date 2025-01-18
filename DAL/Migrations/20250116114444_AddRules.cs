using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
        "INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp) VALUES " +
        "('1', 'Admin', 'ADMIN', '1b5e4d5b-44b1-42d0-a28a-2f81f99e401a'), " +
        "('2', 'User', 'USER', '4c4ff13e-8a3d-4749-82fd-7d9a56a6e0f4')");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
        "DELETE FROM AspNetRoles WHERE Name IN ('Admin', 'User')");


        }
    }
}

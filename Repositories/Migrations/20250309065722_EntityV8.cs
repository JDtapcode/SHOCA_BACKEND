using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class EntityV8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Feature",
                table: "ProPackages");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "PortfolioImages");

            migrationBuilder.CreateTable(
                name: "ProPackageFeature",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProPackageFeature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProPackageFeature_ProPackages_ProPackageId",
                        column: x => x.ProPackageId,
                        principalTable: "ProPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProPackageFeature_ProPackageId",
                table: "ProPackageFeature",
                column: "ProPackageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProPackageFeature");

            migrationBuilder.AddColumn<string>(
                name: "Feature",
                table: "ProPackages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "PortfolioImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class EntityV7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "PortfolioImages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "PortfolioImages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "PortfolioImages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionDate",
                table: "PortfolioImages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "PortfolioImages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PortfolioImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDate",
                table: "PortfolioImages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "PortfolioImages",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PortfolioImages");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "PortfolioImages");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "PortfolioImages");

            migrationBuilder.DropColumn(
                name: "DeletionDate",
                table: "PortfolioImages");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PortfolioImages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PortfolioImages");

            migrationBuilder.DropColumn(
                name: "ModificationDate",
                table: "PortfolioImages");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "PortfolioImages");
        }
    }
}

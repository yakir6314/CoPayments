using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoPayApi.Migrations
{
    public partial class addCompanyToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_companies_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "AspNetUsers",
                newName: "companyId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_companyId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApproveDate",
                table: "payments",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_companies_companyId",
                table: "AspNetUsers",
                column: "companyId",
                principalTable: "companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_companies_companyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApproveDate",
                table: "payments");

            migrationBuilder.RenameColumn(
                name: "companyId",
                table: "AspNetUsers",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_companyId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_companies_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId",
                principalTable: "companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

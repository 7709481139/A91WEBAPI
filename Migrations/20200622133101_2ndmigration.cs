using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace A91WEBAPI.Migrations
{
    public partial class _2ndmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardCode",
                table: "AspNetUsers",
                nullable: true);

           
        }

       
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using WebApplication1.ViewModel;

#nullable disable

namespace WebApplication1.Database.Migrations
{
    /// <inheritdoc />
    public partial class addAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileAttach",
                table: "Messages",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatRoomViewModel");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Users");

           
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class ActorType_Removed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actors_ActorTypes_ActorTypeId",
                table: "Actors");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionTypes_TransactionTypeId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "ActorTypes");

            migrationBuilder.DropIndex(
                name: "IX_Actors_ActorTypeId",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "ActorTypeId",
                table: "Actors");

            migrationBuilder.AddColumn<Guid>(
                name: "SiteId",
                table: "User",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TransactionTypeId",
                table: "Transactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_SiteId",
                table: "User",
                column: "SiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionTypes_TransactionTypeId",
                table: "Transactions",
                column: "TransactionTypeId",
                principalTable: "TransactionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Sites_SiteId",
                table: "User",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionTypes_TransactionTypeId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Sites_SiteId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_SiteId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "User");

            migrationBuilder.AlterColumn<Guid>(
                name: "TransactionTypeId",
                table: "Transactions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "ActorTypeId",
                table: "Actors",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ActorTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actors_ActorTypeId",
                table: "Actors",
                column: "ActorTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actors_ActorTypes_ActorTypeId",
                table: "Actors",
                column: "ActorTypeId",
                principalTable: "ActorTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionTypes_TransactionTypeId",
                table: "Transactions",
                column: "TransactionTypeId",
                principalTable: "TransactionTypes",
                principalColumn: "Id");
        }
    }
}

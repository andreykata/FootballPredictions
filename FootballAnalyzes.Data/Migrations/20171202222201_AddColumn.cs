using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FootballAnalyzes.Data.Migrations
{
    public partial class AddColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FootballGames_GameStatistics_GameStatisticId",
                table: "FootballGames");

            migrationBuilder.DropForeignKey(
                name: "FK_FootballGames_Leagues_LeagueId",
                table: "FootballGames");

            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_FootballGames_GameId",
                table: "Predictions");

            migrationBuilder.AddColumn<int>(
                name: "FirstHalfResultId",
                table: "FootballGames",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GameResults_GameId",
                table: "GameResults",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_FootballGames_FirstHalfResultId",
                table: "FootballGames",
                column: "FirstHalfResultId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FootballGames_GameResults_FirstHalfResultId",
                table: "FootballGames",
                column: "FirstHalfResultId",
                principalTable: "GameResults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FootballGames_GameStatistics_GameStatisticId",
                table: "FootballGames",
                column: "GameStatisticId",
                principalTable: "GameStatistics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FootballGames_Leagues_LeagueId",
                table: "FootballGames",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameResults_FootballGames_GameId",
                table: "GameResults",
                column: "GameId",
                principalTable: "FootballGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_FootballGames_GameId",
                table: "Predictions",
                column: "GameId",
                principalTable: "FootballGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FootballGames_GameResults_FirstHalfResultId",
                table: "FootballGames");

            migrationBuilder.DropForeignKey(
                name: "FK_FootballGames_GameStatistics_GameStatisticId",
                table: "FootballGames");

            migrationBuilder.DropForeignKey(
                name: "FK_FootballGames_Leagues_LeagueId",
                table: "FootballGames");

            migrationBuilder.DropForeignKey(
                name: "FK_GameResults_FootballGames_GameId",
                table: "GameResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_FootballGames_GameId",
                table: "Predictions");

            migrationBuilder.DropIndex(
                name: "IX_GameResults_GameId",
                table: "GameResults");

            migrationBuilder.DropIndex(
                name: "IX_FootballGames_FirstHalfResultId",
                table: "FootballGames");

            migrationBuilder.DropColumn(
                name: "FirstHalfResultId",
                table: "FootballGames");

            migrationBuilder.AddForeignKey(
                name: "FK_FootballGames_GameStatistics_GameStatisticId",
                table: "FootballGames",
                column: "GameStatisticId",
                principalTable: "GameStatistics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FootballGames_Leagues_LeagueId",
                table: "FootballGames",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_FootballGames_GameId",
                table: "Predictions",
                column: "GameId",
                principalTable: "FootballGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FootballAnalyzes.Data.Migrations
{
    public partial class AllModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AwayTeamGoals = table.Column<int>(nullable: false),
                    HomeTeamGoals = table.Column<int>(nullable: false),
                    Result = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AwayTeamCorners = table.Column<int>(nullable: false),
                    AwayTeamFouls = table.Column<int>(nullable: false),
                    AwayTeamOffsides = table.Column<int>(nullable: false),
                    AwayTeamShotsOnTarget = table.Column<int>(nullable: false),
                    AwayTeamShotsWide = table.Column<int>(nullable: false),
                    HomeTeamCorners = table.Column<int>(nullable: false),
                    HomeTeamFouls = table.Column<int>(nullable: false),
                    HomeTeamOffsides = table.Column<int>(nullable: false),
                    HomeTeamShotsOnTarget = table.Column<int>(nullable: false),
                    HomeTeamShotsWide = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Country = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Stage = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    UniqueName = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    UniqueName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FootballGames",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AwayTeamId = table.Column<int>(nullable: false),
                    FirstHalfResultId = table.Column<int>(nullable: true),
                    FullTimeResultId = table.Column<int>(nullable: true),
                    GameStatisticId = table.Column<int>(nullable: true),
                    HomeTeamId = table.Column<int>(nullable: false),
                    LeagueId = table.Column<int>(nullable: false),
                    MatchDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FootballGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FootballGames_Teams_AwayTeamId",
                        column: x => x.AwayTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FootballGames_GameResults_FirstHalfResultId",
                        column: x => x.FirstHalfResultId,
                        principalTable: "GameResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FootballGames_GameResults_FullTimeResultId",
                        column: x => x.FullTimeResultId,
                        principalTable: "GameResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FootballGames_GameStatistics_GameStatisticId",
                        column: x => x.GameStatisticId,
                        principalTable: "GameStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FootballGames_Teams_HomeTeamId",
                        column: x => x.HomeTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FootballGames_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Predictions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GameId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Procent = table.Column<double>(nullable: false),
                    Result = table.Column<string>(nullable: true),
                    Selected = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Predictions_FootballGames_GameId",
                        column: x => x.GameId,
                        principalTable: "FootballGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FootballGames_AwayTeamId",
                table: "FootballGames",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_FootballGames_FirstHalfResultId",
                table: "FootballGames",
                column: "FirstHalfResultId");

            migrationBuilder.CreateIndex(
                name: "IX_FootballGames_FullTimeResultId",
                table: "FootballGames",
                column: "FullTimeResultId");

            migrationBuilder.CreateIndex(
                name: "IX_FootballGames_GameStatisticId",
                table: "FootballGames",
                column: "GameStatisticId");

            migrationBuilder.CreateIndex(
                name: "IX_FootballGames_HomeTeamId",
                table: "FootballGames",
                column: "HomeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_FootballGames_LeagueId",
                table: "FootballGames",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_GameId",
                table: "Predictions",
                column: "GameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Predictions");

            migrationBuilder.DropTable(
                name: "FootballGames");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "GameResults");

            migrationBuilder.DropTable(
                name: "GameStatistics");

            migrationBuilder.DropTable(
                name: "Leagues");
        }
    }
}

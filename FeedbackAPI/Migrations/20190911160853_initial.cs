using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FeedbackAPI.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GameName = table.Column<string>(maxLength: 50, nullable: false),
                    ReleaseYear = table.Column<int>(nullable: false),
                    Publisher = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameID);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ScreenName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerID);
                });

            migrationBuilder.CreateTable(
                name: "GameSessions",
                columns: table => new
                {
                    GameSessionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GameID = table.Column<int>(nullable: false),
                    SessionStartTime = table.Column<DateTime>(nullable: false),
                    SessionEndTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSessions", x => x.GameSessionID);
                    table.ForeignKey(
                        name: "FK_GameSessions_Games_GameID",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "GameID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    FeedbackID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlayerID = table.Column<int>(nullable: false),
                    GameSessionID = table.Column<int>(nullable: false),
                    FeedbackScore = table.Column<int>(nullable: false),
                    FeedbackComment = table.Column<string>(maxLength: 500, nullable: true),
                    SubmissionDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.FeedbackID);
                    table.ForeignKey(
                        name: "FK_Feedback_GameSessions_GameSessionID",
                        column: x => x.GameSessionID,
                        principalTable: "GameSessions",
                        principalColumn: "GameSessionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedback_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Players",
                        principalColumn: "PlayerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "GameID", "GameName", "Publisher", "ReleaseYear" },
                values: new object[,]
                {
                    { 1, "The Witcher", "CD Project", 2017 },
                    { 2, "Gears Of War", "Microsoft", 2009 },
                    { 3, "Pillars Of Eternity", "Paradox", 2015 },
                    { 4, "XCOM2", "2K", 2017 }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "PlayerID", "ScreenName" },
                values: new object[,]
                {
                    { 1, "Geralt" },
                    { 2, "Marcus" },
                    { 3, "Watcher" },
                    { 4, "Bradford" }
                });

            migrationBuilder.InsertData(
                table: "GameSessions",
                columns: new[] { "GameSessionID", "GameID", "SessionEndTime", "SessionStartTime" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2019, 9, 1, 12, 8, 52, 468, DateTimeKind.Local), new DateTime(2019, 9, 1, 7, 8, 52, 468, DateTimeKind.Local) },
                    { 2, 1, new DateTime(2019, 8, 23, 12, 8, 52, 468, DateTimeKind.Local), new DateTime(2019, 8, 22, 12, 8, 52, 468, DateTimeKind.Local) },
                    { 3, 1, new DateTime(2019, 8, 14, 13, 8, 52, 468, DateTimeKind.Local), new DateTime(2019, 8, 12, 11, 8, 52, 468, DateTimeKind.Local) },
                    { 4, 2, new DateTime(2019, 7, 8, 22, 28, 52, 468, DateTimeKind.Local), new DateTime(2019, 7, 8, 11, 8, 52, 468, DateTimeKind.Local) },
                    { 5, 2, new DateTime(2019, 7, 13, 13, 27, 52, 468, DateTimeKind.Local), new DateTime(2019, 7, 13, 7, 28, 52, 468, DateTimeKind.Local) },
                    { 6, 3, new DateTime(2019, 9, 8, 15, 8, 52, 468, DateTimeKind.Local), new DateTime(2019, 9, 8, 2, 13, 52, 468, DateTimeKind.Local) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_GameSessionID",
                table: "Feedback",
                column: "GameSessionID");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_PlayerID",
                table: "Feedback",
                column: "PlayerID");

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_GameID",
                table: "GameSessions",
                column: "GameID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "GameSessions");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}

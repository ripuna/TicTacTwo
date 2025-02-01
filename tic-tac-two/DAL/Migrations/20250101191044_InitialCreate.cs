using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConfigName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    BoardSizeWidth = table.Column<int>(type: "INTEGER", nullable: false),
                    BoardSizeHeight = table.Column<int>(type: "INTEGER", nullable: false),
                    GridSizeWidth = table.Column<int>(type: "INTEGER", nullable: false),
                    GridSizeHeight = table.Column<int>(type: "INTEGER", nullable: false),
                    WinCondition = table.Column<int>(type: "INTEGER", nullable: false),
                    MoveGridAfterNMoves = table.Column<int>(type: "INTEGER", nullable: false),
                    PieceCount = table.Column<int>(type: "INTEGER", nullable: false),
                    MoveablePieces = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    GameState = table.Column<string>(type: "TEXT", maxLength: 10240, nullable: false),
                    ConfigId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Configs_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "Configs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_ConfigId",
                table: "Games",
                column: "ConfigId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Configs");
        }
    }
}

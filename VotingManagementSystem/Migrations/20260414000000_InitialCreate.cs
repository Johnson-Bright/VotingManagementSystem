using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingManagementSystem.Migrations
{
    [DbContext(typeof(VotingManagementSystem.Data.VotingContext))]
    [Migration("20260414000000_InitialCreate")]
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Users", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Positions", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "Elections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Elections", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "Voters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    NationalId = table.Column<string>(type: "TEXT", nullable: false),
                    DateOfBirth = table.Column<string>(type: "TEXT", nullable: true),
                    Gender = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Voters", x => x.Id); table.ForeignKey("FK_Voters_Users_UserId", x => x.UserId, "Users", "Id", onDelete: ReferentialAction.Cascade); });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ElectionId = table.Column<int>(type: "INTEGER", nullable: true),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Party = table.Column<string>(type: "TEXT", nullable: true),
                    Bio = table.Column<string>(type: "TEXT", nullable: true),
                    Manifesto = table.Column<string>(type: "TEXT", nullable: true),
                    PhotoPath = table.Column<string>(type: "TEXT", nullable: true),
                    Contact = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Candidates", x => x.Id); table.ForeignKey("FK_Candidates_Elections_ElectionId", x => x.ElectionId, "Elections", "Id", onDelete: ReferentialAction.Cascade); table.ForeignKey("FK_Candidates_Positions_PositionId", x => x.PositionId, "Positions", "Id", onDelete: ReferentialAction.SetNull); table.ForeignKey("FK_Candidates_Users_UserId", x => x.UserId, "Users", "Id", onDelete: ReferentialAction.SetNull); });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VoterId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    CandidateId = table.Column<int>(type: "INTEGER", nullable: false),
                    ElectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: true),
                    CastAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Votes", x => x.Id); table.ForeignKey("FK_Votes_Candidates_CandidateId", x => x.CandidateId, "Candidates", "Id", onDelete: ReferentialAction.Cascade); table.ForeignKey("FK_Votes_Elections_ElectionId", x => x.ElectionId, "Elections", "Id", onDelete: ReferentialAction.Cascade); table.ForeignKey("FK_Votes_Users_UserId", x => x.UserId, "Users", "Id", onDelete: ReferentialAction.SetNull); table.ForeignKey("FK_Votes_Voters_VoterId", x => x.VoterId, "Voters", "Id", onDelete: ReferentialAction.Cascade); });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Role = table.Column<string>(type: "TEXT", nullable: true),
                    Action = table.Column<string>(type: "TEXT", nullable: false),
                    Details = table.Column<string>(type: "TEXT", nullable: true),
                    IpAddress = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_AuditLogs", x => x.Id); table.ForeignKey("FK_AuditLogs_Users_UserId", x => x.UserId, "Users", "Id", onDelete: ReferentialAction.SetNull); });

            migrationBuilder.CreateIndex(name: "IX_Users_Email", table: "Users", column: "Email", unique: true);
            migrationBuilder.CreateIndex(name: "IX_Users_Username", table: "Users", column: "Username", unique: true);
            migrationBuilder.CreateIndex(name: "IX_Voters_NationalId", table: "Voters", column: "NationalId", unique: true);
            migrationBuilder.CreateIndex(name: "IX_Candidates_ElectionId", table: "Candidates", column: "ElectionId");
            migrationBuilder.CreateIndex(name: "IX_Candidates_PositionId", table: "Candidates", column: "PositionId");
            migrationBuilder.CreateIndex(name: "IX_Candidates_UserId", table: "Candidates", column: "UserId");
            migrationBuilder.CreateIndex(name: "IX_Votes_VoterId", table: "Votes", column: "VoterId");
            migrationBuilder.CreateIndex(name: "IX_Votes_CandidateId", table: "Votes", column: "CandidateId");
            migrationBuilder.CreateIndex(name: "IX_Votes_ElectionId", table: "Votes", column: "ElectionId");
            migrationBuilder.CreateIndex(name: "IX_Votes_UserId", table: "Votes", column: "UserId");
            migrationBuilder.CreateIndex(name: "UX_Votes_Voter_Election_Position", table: "Votes", columns: new[] { "VoterId", "ElectionId", "PositionId" }, unique: true);

            // Create view
            migrationBuilder.Sql(@"CREATE VIEW IF NOT EXISTS ResultsView AS
SELECT v.ElectionId AS ElectionId, v.CandidateId AS CandidateId, c.Name AS CandidateName, COUNT(*) AS VotesCount
FROM Votes v
JOIN Candidates c ON c.Id = v.CandidateId
GROUP BY v.ElectionId, v.CandidateId;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS ResultsView");
            migrationBuilder.DropTable(name: "AuditLogs");
            migrationBuilder.DropTable(name: "Votes");
            migrationBuilder.DropTable(name: "Candidates");
            migrationBuilder.DropTable(name: "Voters");
            migrationBuilder.DropTable(name: "Elections");
            migrationBuilder.DropTable(name: "Positions");
            migrationBuilder.DropTable(name: "Users");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FundTrack.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ChangedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schemes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Amc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RiskLevel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CurrentNav = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    NavDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ModifiedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schemes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ModifiedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NavHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NavDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Nav = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NavHistory_Schemes_SchemeId",
                        column: x => x.SchemeId,
                        principalTable: "Schemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sips",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstallmentAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DayOfMonth = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    NextInstallmentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ModifiedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sips_Schemes_SchemeId",
                        column: x => x.SchemeId,
                        principalTable: "Schemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sips_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SipId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Nav = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Units = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    TransactionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ModifiedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Schemes_SchemeId",
                        column: x => x.SchemeId,
                        principalTable: "Schemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Sips_SipId",
                        column: x => x.SipId,
                        principalTable: "Sips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Schemes",
                columns: new[] { "Id", "Amc", "Category", "Code", "CreatedAtUtc", "CreatedBy", "CurrentNav", "DeletedAtUtc", "DeletedBy", "IsDeleted", "ModifiedAtUtc", "ModifiedBy", "Name", "NavDate", "RiskLevel" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "Aarav Mutual Fund", "Equity", "FT100001", new DateTimeOffset(new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "seed", 78.4521m, null, null, false, null, null, "Aarav Flexi Cap Fund", new DateOnly(2026, 10, 30), "VeryHigh" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "Aarav Mutual Fund", "Equity", "FT100002", new DateTimeOffset(new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "seed", 52.1187m, null, null, false, null, null, "Aarav Large Cap Fund", new DateOnly(2026, 10, 30), "High" },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "Aarav Mutual Fund", "Debt", "FT100003", new DateTimeOffset(new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "seed", 31.6642m, null, null, false, null, null, "Aarav Short Duration Fund", new DateOnly(2026, 10, 30), "LowToModerate" },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "Nirmal Mutual Fund", "Equity", "FT100004", new DateTimeOffset(new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "seed", 112.9035m, null, null, false, null, null, "Nirmal Small Cap Fund", new DateOnly(2026, 10, 30), "VeryHigh" },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "Nirmal Mutual Fund", "Hybrid", "FT100005", new DateTimeOffset(new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "seed", 44.2876m, null, null, false, null, null, "Nirmal Balanced Advantage Fund", new DateOnly(2026, 10, 30), "ModeratelyHigh" },
                    { new Guid("10000000-0000-0000-0000-000000000006"), "Nirmal Mutual Fund", "Liquid", "FT100006", new DateTimeOffset(new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "seed", 2543.1098m, null, null, false, null, null, "Nirmal Liquid Fund", new DateOnly(2026, 10, 30), "Low" },
                    { new Guid("10000000-0000-0000-0000-000000000007"), "Sahyadri Mutual Fund", "Index", "FT100007", new DateTimeOffset(new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "seed", 26.7314m, null, null, false, null, null, "Sahyadri Nifty 50 Index Fund", new DateOnly(2026, 10, 30), "High" },
                    { new Guid("10000000-0000-0000-0000-000000000008"), "Sahyadri Mutual Fund", "Debt", "FT100008", new DateTimeOffset(new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "seed", 28.0459m, null, null, false, null, null, "Sahyadri Corporate Bond Fund", new DateOnly(2026, 10, 30), "Moderate" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_ChangedAtUtc",
                table: "AuditLogs",
                column: "ChangedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityName_EntityId",
                table: "AuditLogs",
                columns: new[] { "EntityName", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_NavHistory_SchemeId_NavDate",
                table: "NavHistory",
                columns: new[] { "SchemeId", "NavDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schemes_Category_RiskLevel",
                table: "Schemes",
                columns: new[] { "Category", "RiskLevel" });

            migrationBuilder.CreateIndex(
                name: "IX_Schemes_Code",
                table: "Schemes",
                column: "Code",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Schemes_Name",
                table: "Schemes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Sips_SchemeId",
                table: "Sips",
                column: "SchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sips_Status_NextInstallmentDate",
                table: "Sips",
                columns: new[] { "Status", "NextInstallmentDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Sips_UserId_Status",
                table: "Sips",
                columns: new[] { "UserId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SchemeId",
                table: "Transactions",
                column: "SchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SipId",
                table: "Transactions",
                column: "SipId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId_TransactionDate",
                table: "Transactions",
                columns: new[] { "UserId", "TransactionDate" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "NavHistory");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Sips");

            migrationBuilder.DropTable(
                name: "Schemes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

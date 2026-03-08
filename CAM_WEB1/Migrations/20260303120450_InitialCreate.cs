using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAM_WEB1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "t_Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Branch = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_Users", x => x.UserId);
                    table.CheckConstraint("CK_User_Email_Format", "Email LIKE '%@gmail.com'");
                    table.CheckConstraint("CK_User_UserId_Format", "UserId LIKE 'USR%'");
                    table.ForeignKey(
                        name: "FK_t_Users_t_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "t_Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t_RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UserId = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_t_RefreshTokens_t_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "t_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_RefreshTokens_UserId",
                table: "t_RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_t_Roles_RoleName",
                table: "t_Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_Users_Email",
                table: "t_Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_Users_RoleId",
                table: "t_Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_RefreshTokens");

            migrationBuilder.DropTable(
                name: "t_Users");

            migrationBuilder.DropTable(
                name: "t_Roles");
        }
    }
}

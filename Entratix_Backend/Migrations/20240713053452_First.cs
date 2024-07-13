using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Entratix_Backend.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roleid = table.Column<int>(type: "integer", nullable: false),
                    firstname = table.Column<string>(type: "text", nullable: false),
                    lastname = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    passwordsalt = table.Column<byte[]>(type: "bytea", nullable: false),
                    passwordhash = table.Column<byte[]>(type: "bytea", nullable: false),
                    tokenexpires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    tokencreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    token = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    location = table.Column<string>(type: "text", nullable: false),
                    cost = table.Column<decimal>(type: "numeric", nullable: false),
                    photo = table.Column<string>(type: "text", nullable: false),
                    department = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.id);
                    table.ForeignKey(
                        name: "FK_events_users_userid",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "eventtickets",
                columns: table => new
                {
                    eventid = table.Column<int>(type: "integer", nullable: false),
                    entry = table.Column<string>(type: "text", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eventtickets", x => new { x.eventid, x.entry });
                    table.ForeignKey(
                        name: "FK_eventtickets_events_eventid",
                        column: x => x.eventid,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ticketpurchases",
                columns: table => new
                {
                    userid = table.Column<int>(type: "integer", nullable: false),
                    eventid = table.Column<int>(type: "integer", nullable: false),
                    entry = table.Column<string>(type: "text", nullable: false),
                    quantity_purchased = table.Column<int>(type: "integer", nullable: false),
                    used = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticketpurchases", x => new { x.userid, x.eventid, x.entry });
                    table.ForeignKey(
                        name: "FK_ticketpurchases_events_eventid",
                        column: x => x.eventid,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ticketpurchases_users_userid",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_events_userid",
                table: "events",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_ticketpurchases_eventid",
                table: "ticketpurchases",
                column: "eventid");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "eventtickets");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "ticketpurchases");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NexusBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Date", "Name" },
                values: new object[] { 1, new DateTimeOffset(new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Tech Conference 2025" });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "CurrentUserId", "EventId", "HoldExpiry", "SeatNumber", "Status" },
                values: new object[,]
                {
                    { 1, null, 1, null, "1A", 0 },
                    { 2, null, 1, null, "1B", 0 },
                    { 3, null, 1, null, "1C", 0 },
                    { 4, null, 1, null, "1D", 0 },
                    { 5, null, 1, null, "1E", 0 }
                });


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

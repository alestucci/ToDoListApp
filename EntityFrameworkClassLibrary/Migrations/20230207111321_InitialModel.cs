using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkClassLibrary.Migrations
{
    public partial class InitialModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaskDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "CreatedTime", "IsCompleted", "TaskDescription" },
                values: new object[] { "00fc28d7-67e5-44a4-91d1-dbc5d2253a04", new DateTime(2023, 2, 7, 12, 13, 21, 51, DateTimeKind.Local).AddTicks(5904), false, "Pagare le bollette" });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "CreatedTime", "IsCompleted", "TaskDescription" },
                values: new object[] { "4ba1dc49-a574-436d-8b5f-8bcf28f1745d", new DateTime(2023, 2, 7, 12, 13, 21, 51, DateTimeKind.Local).AddTicks(5911), false, "Andare in banca" });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "CreatedTime", "IsCompleted", "TaskDescription" },
                values: new object[] { "780423e0-3d45-4efd-bc41-8970892ebeb7", new DateTime(2023, 2, 7, 12, 13, 21, 51, DateTimeKind.Local).AddTicks(5897), false, "Comprare il pane" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkClassLibrary.Migrations
{
    public partial class InitialCreate : Migration
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
                values: new object[] { "42e50217-216e-4e5e-a985-e0ce4006026d", new DateTime(2023, 2, 10, 15, 39, 36, 507, DateTimeKind.Local).AddTicks(9315), false, "Pagare le bollette" });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "CreatedTime", "IsCompleted", "TaskDescription" },
                values: new object[] { "5dba68c0-1f75-461c-9ba6-01b01d816090", new DateTime(2023, 2, 10, 15, 39, 36, 507, DateTimeKind.Local).AddTicks(9298), false, "Comprare il pane" });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "CreatedTime", "IsCompleted", "TaskDescription" },
                values: new object[] { "7c3d82f5-95e1-4d5a-b361-871199308f21", new DateTime(2023, 2, 10, 15, 39, 36, 507, DateTimeKind.Local).AddTicks(9325), false, "Andare in banca" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos");
        }
    }
}

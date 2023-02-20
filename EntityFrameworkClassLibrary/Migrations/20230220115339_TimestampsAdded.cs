using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkClassLibrary.Migrations
{
    public partial class TimestampsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: "42e50217-216e-4e5e-a985-e0ce4006026d");

            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: "5dba68c0-1f75-461c-9ba6-01b01d816090");

            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: "7c3d82f5-95e1-4d5a-b361-871199308f21");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "Todos",
                newName: "Updated");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Todos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "Created", "IsCompleted", "TaskDescription", "Updated" },
                values: new object[] { "3b38df86-6098-4ea7-9133-0db0b2709ca6", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Andare in banca", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "Created", "IsCompleted", "TaskDescription", "Updated" },
                values: new object[] { "5ad0df73-4ef9-4312-a65e-cf04157984a1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Comprare il pane", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "Created", "IsCompleted", "TaskDescription", "Updated" },
                values: new object[] { "6a82af6c-5dc7-48a5-9790-7d790bd8484d", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Pagare le bollette", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: "3b38df86-6098-4ea7-9133-0db0b2709ca6");

            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: "5ad0df73-4ef9-4312-a65e-cf04157984a1");

            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: "6a82af6c-5dc7-48a5-9790-7d790bd8484d");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Todos");

            migrationBuilder.RenameColumn(
                name: "Updated",
                table: "Todos",
                newName: "CreatedTime");

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
    }
}

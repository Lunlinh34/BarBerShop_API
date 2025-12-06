using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberShop.Migrations
{
    public partial class fix_booking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Payment_payID",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "BookingsService");

            migrationBuilder.RenameColumn(
                name: "payID",
                table: "Bookings",
                newName: "storeID");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_payID",
                table: "Bookings",
                newName: "IX_Bookings_storeID");

            migrationBuilder.AddColumn<int>(
                name: "employeID",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "serID",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_employeID",
                table: "Bookings",
                column: "employeID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_serID",
                table: "Bookings",
                column: "serID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Employees_employeID",
                table: "Bookings",
                column: "employeID",
                principalTable: "Employees",
                principalColumn: "employeID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Service_serID",
                table: "Bookings",
                column: "serID",
                principalTable: "Service",
                principalColumn: "serID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Stores_storeID",
                table: "Bookings",
                column: "storeID",
                principalTable: "Stores",
                principalColumn: "storeID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Employees_employeID",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Service_serID",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Stores_storeID",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_employeID",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_serID",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "employeID",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "note",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "serID",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "storeID",
                table: "Bookings",
                newName: "payID");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_storeID",
                table: "Bookings",
                newName: "IX_Bookings_payID");

            migrationBuilder.CreateTable(
                name: "BookingsService",
                columns: table => new
                {
                    bookingServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bookingID = table.Column<int>(type: "int", nullable: false),
                    serviceID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingsService", x => x.bookingServiceID);
                    table.ForeignKey(
                        name: "FK_BookingsService_Bookings_bookingID",
                        column: x => x.bookingID,
                        principalTable: "Bookings",
                        principalColumn: "bookingID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingsService_Service_serviceID",
                        column: x => x.serviceID,
                        principalTable: "Service",
                        principalColumn: "serID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingsService_bookingID",
                table: "BookingsService",
                column: "bookingID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingsService_serviceID",
                table: "BookingsService",
                column: "serviceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Payment_payID",
                table: "Bookings",
                column: "payID",
                principalTable: "Payment",
                principalColumn: "payID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

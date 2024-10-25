using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace handshakes_carpark.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarParks",
                columns: table => new
                {
                    CarParkNo = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    XCoord = table.Column<double>(type: "REAL", nullable: false),
                    YCoord = table.Column<double>(type: "REAL", nullable: false),
                    CarParkType = table.Column<string>(type: "TEXT", nullable: true),
                    ParkingSystemType = table.Column<string>(type: "TEXT", nullable: true),
                    ShortTermParking = table.Column<string>(type: "TEXT", nullable: true),
                    FreeParking = table.Column<string>(type: "TEXT", nullable: true),
                    NightParking = table.Column<bool>(type: "INTEGER", nullable: false),
                    CarParkDecks = table.Column<int>(type: "INTEGER", nullable: false),
                    GantryHeight = table.Column<double>(type: "REAL", nullable: false),
                    CarParkBasement = table.Column<bool>(type: "INTEGER", nullable: false),
                    FavoriteCarPark = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarParks", x => x.CarParkNo);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarParks");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace BreastCancerAPI.Migrations
{
    public partial class InitialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CellFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Radius = table.Column<double>(type: "float", nullable: false),
                    Texture = table.Column<double>(type: "float", nullable: false),
                    Perimeter = table.Column<double>(type: "float", nullable: false),
                    Area = table.Column<double>(type: "float", nullable: false),
                    Smoothness = table.Column<double>(type: "float", nullable: false),
                    Compactness = table.Column<double>(type: "float", nullable: false),
                    Concavity = table.Column<double>(type: "float", nullable: false),
                    ConcavePoints = table.Column<double>(type: "float", nullable: false),
                    Symmetry = table.Column<double>(type: "float", nullable: false),
                    FractalDimension = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CellFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrognosticInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Outcome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<int>(type: "int", nullable: false),
                    TumorSize = table.Column<float>(type: "real", nullable: false),
                    LymphNodeStatus = table.Column<int>(type: "int", nullable: false),
                    CellFeaturesId = table.Column<int>(type: "int", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrognosticInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrognosticInfos_CellFeatures_CellFeaturesId",
                        column: x => x.CellFeaturesId,
                        principalTable: "CellFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrognosticInfos_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrognosticInfos_CellFeaturesId",
                table: "PrognosticInfos",
                column: "CellFeaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_PrognosticInfos_PatientId",
                table: "PrognosticInfos",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrognosticInfos");

            migrationBuilder.DropTable(
                name: "CellFeatures");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}

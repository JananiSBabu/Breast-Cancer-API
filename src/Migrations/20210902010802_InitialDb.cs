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
                    FractalDimension = table.Column<double>(type: "float", nullable: false),
                    MitosisCount = table.Column<int>(type: "int", nullable: false),
                    NucleiSize = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CellFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CellFeaturesModel",
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
                    table.PrimaryKey("PK_CellFeaturesModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MRN = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MRN = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    PrimaryPhysician = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrognosticInfoModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Outcome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<int>(type: "int", nullable: false),
                    TumorSize = table.Column<double>(type: "float", nullable: false),
                    LymphNodeStatus = table.Column<int>(type: "int", nullable: true),
                    CellFeaturesId = table.Column<int>(type: "int", nullable: true),
                    PatientModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrognosticInfoModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrognosticInfoModel_CellFeaturesModel_CellFeaturesId",
                        column: x => x.CellFeaturesId,
                        principalTable: "CellFeaturesModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrognosticInfoModel_PatientModel_PatientModelId",
                        column: x => x.PatientModelId,
                        principalTable: "PatientModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrognosticInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Outcome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<int>(type: "int", nullable: false),
                    TumorSize = table.Column<double>(type: "float", nullable: false),
                    LymphNodeStatus = table.Column<int>(type: "int", nullable: true),
                    CellFeaturesId = table.Column<int>(type: "int", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    TumorGrade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TumorType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HER2Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrognosticInfoModel_CellFeaturesId",
                table: "PrognosticInfoModel",
                column: "CellFeaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_PrognosticInfoModel_PatientModelId",
                table: "PrognosticInfoModel",
                column: "PatientModelId");

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
                name: "PrognosticInfoModel");

            migrationBuilder.DropTable(
                name: "PrognosticInfos");

            migrationBuilder.DropTable(
                name: "CellFeaturesModel");

            migrationBuilder.DropTable(
                name: "PatientModel");

            migrationBuilder.DropTable(
                name: "CellFeatures");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}

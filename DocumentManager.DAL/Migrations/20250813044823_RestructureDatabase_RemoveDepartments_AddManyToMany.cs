using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RestructureDatabase_RemoveDepartments_AddManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "IssuingUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssuingUnitName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuingUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutgoingDocumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutgoingDocumentTypeName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingDocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipientGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientGroupName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipientGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RelatedProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RelatedProjectName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedProjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutgoingDocumentFormats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutgoingDocumentFormatName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    OutgoingDocumentTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingDocumentFormats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutgoingDocumentFormats_OutgoingDocumentTypes_OutgoingDocumentTypeId",
                        column: x => x.OutgoingDocumentTypeId,
                        principalTable: "OutgoingDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecipientGroupEmployees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    RecipientGroupID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipientGroupEmployees", x => new { x.EmployeeID, x.RecipientGroupID });
                    table.ForeignKey(
                        name: "FK_RecipientGroupEmployees_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipientGroupEmployees_RecipientGroups_RecipientGroupID",
                        column: x => x.RecipientGroupID,
                        principalTable: "RecipientGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncomingDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncomingDocumentNumber = table.Column<int>(type: "int", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentCodeFromIssuer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseDateFromIssuer = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocumentContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuingUnitID = table.Column<int>(type: "int", nullable: false),
                    RelatedProjectID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomingDocuments_IssuingUnits_IssuingUnitID",
                        column: x => x.IssuingUnitID,
                        principalTable: "IssuingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomingDocuments_RelatedProjects_RelatedProjectID",
                        column: x => x.RelatedProjectID,
                        principalTable: "RelatedProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OutgoingDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutgoingDocumentNumber = table.Column<int>(type: "int", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuingUnitID = table.Column<int>(type: "int", nullable: false),
                    RelatedProjectID = table.Column<int>(type: "int", nullable: false),
                    OutgoingDocumentFormatID = table.Column<int>(type: "int", nullable: false),
                    OutgoingDocumentTypeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutgoingDocuments_IssuingUnits_IssuingUnitID",
                        column: x => x.IssuingUnitID,
                        principalTable: "IssuingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutgoingDocuments_OutgoingDocumentFormats_OutgoingDocumentFormatID",
                        column: x => x.OutgoingDocumentFormatID,
                        principalTable: "OutgoingDocumentFormats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutgoingDocuments_OutgoingDocumentTypes_OutgoingDocumentTypeID",
                        column: x => x.OutgoingDocumentTypeID,
                        principalTable: "OutgoingDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutgoingDocuments_RelatedProjects_RelatedProjectID",
                        column: x => x.RelatedProjectID,
                        principalTable: "RelatedProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncomingDocumentRecipientGroups",
                columns: table => new
                {
                    IncomingDocumentID = table.Column<int>(type: "int", nullable: false),
                    RecipientGroupID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingDocumentRecipientGroups", x => new { x.IncomingDocumentID, x.RecipientGroupID });
                    table.ForeignKey(
                        name: "FK_IncomingDocumentRecipientGroups_IncomingDocuments_IncomingDocumentID",
                        column: x => x.IncomingDocumentID,
                        principalTable: "IncomingDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomingDocumentRecipientGroups_RecipientGroups_RecipientGroupID",
                        column: x => x.RecipientGroupID,
                        principalTable: "RecipientGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OutgoingDocumentRecipientGroups",
                columns: table => new
                {
                    OutgoingDocumentID = table.Column<int>(type: "int", nullable: false),
                    RecipientGroupID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingDocumentRecipientGroups", x => new { x.OutgoingDocumentID, x.RecipientGroupID });
                    table.ForeignKey(
                        name: "FK_OutgoingDocumentRecipientGroups_OutgoingDocuments_OutgoingDocumentID",
                        column: x => x.OutgoingDocumentID,
                        principalTable: "OutgoingDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutgoingDocumentRecipientGroups_RecipientGroups_RecipientGroupID",
                        column: x => x.RecipientGroupID,
                        principalTable: "RecipientGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomingDocumentRecipientGroups_RecipientGroupID",
                table: "IncomingDocumentRecipientGroups",
                column: "RecipientGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingDocuments_IssuingUnitID",
                table: "IncomingDocuments",
                column: "IssuingUnitID");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingDocuments_RelatedProjectID",
                table: "IncomingDocuments",
                column: "RelatedProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocumentFormats_OutgoingDocumentTypeId",
                table: "OutgoingDocumentFormats",
                column: "OutgoingDocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocumentRecipientGroups_RecipientGroupID",
                table: "OutgoingDocumentRecipientGroups",
                column: "RecipientGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocuments_IssuingUnitID",
                table: "OutgoingDocuments",
                column: "IssuingUnitID");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocuments_OutgoingDocumentFormatID",
                table: "OutgoingDocuments",
                column: "OutgoingDocumentFormatID");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocuments_OutgoingDocumentTypeID",
                table: "OutgoingDocuments",
                column: "OutgoingDocumentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocuments_RelatedProjectID",
                table: "OutgoingDocuments",
                column: "RelatedProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_RecipientGroupEmployees_RecipientGroupID",
                table: "RecipientGroupEmployees",
                column: "RecipientGroupID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomingDocumentRecipientGroups");

            migrationBuilder.DropTable(
                name: "OutgoingDocumentRecipientGroups");

            migrationBuilder.DropTable(
                name: "RecipientGroupEmployees");

            migrationBuilder.DropTable(
                name: "IncomingDocuments");

            migrationBuilder.DropTable(
                name: "OutgoingDocuments");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "RecipientGroups");

            migrationBuilder.DropTable(
                name: "IssuingUnits");

            migrationBuilder.DropTable(
                name: "OutgoingDocumentFormats");

            migrationBuilder.DropTable(
                name: "RelatedProjects");

            migrationBuilder.DropTable(
                name: "OutgoingDocumentTypes");
        }
    }
}

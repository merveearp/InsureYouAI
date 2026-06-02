using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsureYouAI.Migrations
{
    /// <inheritdoc />
    public partial class AddInsuranceLeadTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InsuranceLeads",
                columns: table => new
                {
                    InsuranceLeadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsuranceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSentToZoho = table.Column<bool>(type: "bit", nullable: false),
                    ZohoLeadId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZohoSyncStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZohoErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceLeads", x => x.InsuranceLeadId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsuranceLeads");
        }
    }
}

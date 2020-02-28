using Microsoft.EntityFrameworkCore.Migrations;
using TOT.Data.Extensions;
namespace TOT.Data.Migrations
{
    public partial class AddTemporalTables : Migration
    {
        //Creates History schema and adds temporal tables to the database
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE SCHEMA History");
            migrationBuilder.AddTemporalTableSupport("VacationRequests", "dbo", "History");
            migrationBuilder.AddTemporalTableSupport("ManagerResponses", "dbo", "History");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

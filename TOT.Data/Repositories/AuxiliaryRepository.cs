using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TOT.Interfaces.Repositories;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.Extensions.Configuration;

namespace TOT.Data.Repositories
{
    public class AuxiliaryRepository : IAuxiliaryRepository
    {
        private readonly string _connectionString;
        public AuxiliaryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public ICollection<int> GetHistoryManagerIdentificators(int historyVacationRequestId, DateTime systemStart)
        {
            List<int> HistoryManagerIdentificators = new List<int>();
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();
                string sql =
                    $@"SELECT DISTINCT MR.ManagerId
                    FROM dbo.VacationRequests
                    FOR SYSTEM_TIME AS OF '{systemStart.ToString("yyyy-MM-dd HH:mm:ss")}' AS VR
                    INNER JOIN dbo.ManagerResponses
                    FOR SYSTEM_TIME AS OF '{systemStart.ToString("yyyy-MM-dd HH:mm:ss")}' AS MR
                    ON VR.VacationRequestId = MR.VacationRequestId
                    WHERE VR.VacationRequestId = {historyVacationRequestId}
                    AND MR.ForStageOfApproving = 2;";
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                SqlCommand command = new SqlCommand(sql, connection);
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        HistoryManagerIdentificators.Add((int)reader["ManagerId"]);
                    }
                }
            }
            return HistoryManagerIdentificators;
        }
    }
}

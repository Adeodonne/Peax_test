using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Domain.Constants;
using EmployeeService.Dtos;
using Infrastructure.Contract;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings[ConfiguratorConstants.EmployeeDb].ConnectionString;

        public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
            WITH EmployeeHierarchy AS (
                SELECT Id, Name, ManagerID, 0 AS Level
                FROM dbo.Employee
                WHERE Id = @id AND Enable = 1

                UNION ALL

                SELECT e.Id, e.Name, e.ManagerID, eh.Level + 1
                FROM dbo.Employee e
                INNER JOIN EmployeeHierarchy eh ON e.ManagerID = eh.Id
                WHERE e.Enable = 1
            )
            SELECT Id, Name, ManagerID, Level
            FROM EmployeeHierarchy
            ORDER BY Level, Name";

                command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    EmployeeDto result = null;

                    var employeeById = new Dictionary<int, EmployeeDto>();

                    var ordId = reader.GetOrdinal("Id");
                    var ordName = reader.GetOrdinal("Name");
                    var ordManagerId = reader.GetOrdinal("ManagerID");
                    var ordLevel = reader.GetOrdinal("Level");

                    while (await reader.ReadAsync())
                    {
                        var employeeId = reader.GetInt32(ordId);
                        var level = reader.GetInt32(ordLevel);

                        var employee = new EmployeeDto
                        {
                            Id = employeeId,
                            Name = reader.IsDBNull(ordName) ? null : reader.GetString(ordName),
                            ManagerID = reader.IsDBNull(ordManagerId)
                                ? (int?)null
                                : reader.GetInt32(ordManagerId),
                            Employees = new List<EmployeeDto>()
                        };

                        employeeById[employeeId] = employee;

                        if (level == 0 || employee.ManagerID == null)
                        {
                            result = employee;
                            continue;
                        }

                        if (employee.ManagerID.HasValue &&
                            employeeById.TryGetValue(employee.ManagerID.Value, out var parent))
                        {
                            parent.Employees.Add(employee);
                        }
                    }

                    return result;
                }
            }
        }

        public async Task<bool> EnableEmployeeAsync(int id, int enable)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                UPDATE dbo.Employee 
                SET Enable = @enable 
                WHERE ID = @id";

                    command.Parameters.Add(new SqlParameter("@enable", enable));
                    command.Parameters.Add(new SqlParameter("@id", id));

                    var rows = await command.ExecuteNonQueryAsync();
                    return rows > 0;
                }
            }
        }
    }
}

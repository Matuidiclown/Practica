using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HubLap.Data.Core
{
    public class SqlServerDataAccess : IDataAccess
    {
        private readonly IConfiguration _config;

        // Inyección de Dependencias: Pedimos la configuración al iniciar
        public SqlServerDataAccess(IConfiguration config)
        {
            _config = config;
        }

        // Implementación de LEER
        public async Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName = "DefaultConnection")
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                // Dapper hace su magia aquí: QueryAsync
                return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        // Implementación de GUARDAR
        public async Task SaveData<T>(string storedProcedure, T parameters, string connectionStringName = "DefaultConnection")
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                // Dapper hace su magia aquí: ExecuteAsync
                await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}

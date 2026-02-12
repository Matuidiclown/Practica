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

        private readonly string _conectionstring;

        // Inyección de Dependencias: Pedimos la configuración al iniciar
        public SqlServerDataAccess(IConfiguration config)
        {
            _config = config;
            _conectionstring = config.GetConnectionString("DefaultConnection");
        }
        

        // Implementación de LEER
        public async Task<IEnumerable<T>> LoadData<T>(string storedProcedure, object parameters = null)
        {
            using (var connection = new SqlConnection(_conectionstring))
            {
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

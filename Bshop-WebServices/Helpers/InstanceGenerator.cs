using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Bshop_WebServices.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace Bshop_WebServices.Helpers
{
    public class InstanceGenerator
    {

        private static MySqlConnection _Connection { get; set; }
        ILogger<InstanceGenerator> _logger;
        public MySqlConnection Instance(ServicesConfig config)
        {
            try
            {

                string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", config.Host,
                                                                                                     config.DataBaseName,
                                                                                                     config.DataBaseUser,
                                                                                                     config.DataBasePwd);
                _Connection = new MySqlConnection(connstring);
                _Connection.Open();
                return _Connection;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al conectar con la base de datos {ex.Message} seguimiento {ex.StackTrace}");
            }
            return null;
        }

        public async Task<DbDataReader> ExecutePetition(string query, MySqlConnection connection)
        {
            try
            {
                var cmd = new MySqlCommand(query, connection);
                var reader = await cmd.ExecuteReaderAsync();
                return reader;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error al realizar la consulta con la base de datos {ex.Message} seguimiento {ex.StackTrace}");
            }

            return null;
        }
    }
}

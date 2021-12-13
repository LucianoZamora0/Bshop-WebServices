using Bshop_WebServices.Configuration;
using Bshop_WebServices.Helpers;
using Bshop_WebServices.Models;
using Bshop_WebServices.Repository.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bshop_WebServices.Repository.Implementation
{
    /*Clase encargada de realizar las operaciones definidas por la intefaz*/
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ServicesConfig _config;
        InstanceGenerator _instance = new InstanceGenerator();
        private const string _TableName = "category";
        ILogger<ProductRepository> _logger;

        public CategoryRepository(IOptions<ServicesConfig> config, ILogger<ProductRepository> logger)
        {
            _logger = logger;
            _config = config.Value;
        }

        /*Accion para consultar por las categorias almacenadas*/
        public async Task<ICollection<Category>> GetAll()
        {
            try
            {

                ICollection<Category> categories = new List<Category>();
                MySqlConnection connection = _instance.Instance(_config);
                string query = $"SELECT id, name " +
                               $"FROM {_TableName} ";
                var reader = await _instance.ExecutePetition(query, connection);

                while (reader.Read())
                {
                    Category category = new Category();
                    category.Id = int.Parse(reader["id"].ToString());
                    category.Name = reader["name"].ToString();
                    categories.Add(category);
                }
                connection.Close();
                var categoriesResult = await Task<ICollection<Category>>.FromResult(categories);

                return categoriesResult;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Un error ocurrio al buscar las categorias, error: {ex.Message}, en: {ex.StackTrace}");
            }
            return null;
        }
    }
}

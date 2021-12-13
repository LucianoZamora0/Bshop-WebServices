using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bshop_WebServices.Configuration;
using Bshop_WebServices.Repository.Contracts;
using Microsoft.Extensions.Logging;
using MySql.Data;
using MySql.Data.MySqlClient;
using Bshop_WebServices.Helpers;
using Bshop_WebServices.Models;
using Microsoft.Extensions.Options;

namespace Bshop_WebServices.Repository.Implementation
{
    /*Clase encargada de realizar las operaciones definidas por la intefaz*/
    public class ProductRepository : IProductRepository
    {
        private  readonly ServicesConfig _config;
        InstanceGenerator _instance = new InstanceGenerator();
        private const string _TableName = "product";
        ILogger<ProductRepository> _logger;

        public ProductRepository(IOptions<ServicesConfig> config, ILogger<ProductRepository> logger)
        {
            _logger = logger;
            _config = config.Value;
        }

        /*Accion para consultar por un producto dado una id*/
        public async  Task<Product> Get(int id)
        {
            try
            {
                Product product = new Product();
                MySqlConnection connection = _instance.Instance(_config);
                string query = $"SELECT id, name, url_image, price, discount, category " +
                               $"FROM {_TableName} " +
                               $"WHERE id = {id} ";
                var reader = await _instance.ExecutePetition(query, connection);
                
                while (reader.Read())
                {
                    product.Id = int.Parse(reader["id"].ToString());
                    product.Name = reader["name"].ToString();
                    product.UrlImage = reader["url_image"].ToString();
                    product.Discount = int.Parse(reader["discount"].ToString());
                    product.Price = float.Parse(reader["price"].ToString());
                    product.Category = int.Parse(reader["category"].ToString());
                }
                connection.Close();
                var p = await Task<Product>.FromResult(product);

                return p;

            }
            catch(Exception ex)
            {
                _logger.LogError($"Un error ocurrio al buscar el producto id {id}, error: {ex.Message}, en: {ex.StackTrace}");
            }
            return null;
        }

        /*Accion para consultar por productos dado un rango de busqueda*/
        public async Task<ICollection<Product>> GetAll(int start, int end)
        {
            try
            {
                
                ICollection<Product> products = new List<Product>();
                MySqlConnection connection = _instance.Instance(_config);
                string query = $"SELECT id, name, url_image, price, discount, category " +
                               $"FROM {_TableName} " + 
                               $"LIMIT {end - start} OFFSET {start}";
                var reader = await _instance.ExecutePetition(query, connection);

                while (reader.Read())
                {
                    Product product = new Product();
                    product.Id = int.Parse(reader["id"].ToString());
                    product.Name = reader["name"].ToString();
                    product.UrlImage = reader["url_image"].ToString();
                    product.Discount = int.Parse(reader["discount"].ToString());
                    product.Price = float.Parse(reader["price"].ToString());
                    product.Category = int.Parse(reader["category"].ToString());
                    products.Add(product);
                }
                connection.Close();
                var productsResult = await Task<ICollection<Product>>.FromResult(products);

                return productsResult;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Un error ocurrio al buscar los productos, error: {ex.Message}, en: {ex.StackTrace}");
            }
            return null;
        }

        /*Accion para consultar total de productos almacenados*/
        public async Task<int> GetAllCount()
        {
            try
            {

                MySqlConnection connection = _instance.Instance(_config);
                string query = $"SELECT Count(*) as total " +
                               $"FROM {_TableName} ";
                var reader = await _instance.ExecutePetition(query, connection);
                int total = 0;

                while (reader.Read())
                {
                    total = int.Parse(reader["total"].ToString());
                }
                connection.Close();
                return total;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Un error ocurrio al contar los productos, error: {ex.Message}, en: {ex.StackTrace}");
            }
            return -1;
        }

        /*Accion para consultar por productos dado una lista de categorias y rango de busqueda*/
        public async Task<ICollection<Product>> GetByCategory(List<int> categories, int start, int end)
        {
            try
            {
                string categoriesQuery = "(";
                foreach(var x in categories)
                {
                    categoriesQuery += x + ",";
                }

                string categoriesQuery2 = categoriesQuery.Remove(categoriesQuery.Length - 1) + ")";
                ICollection<Product> products = new List<Product>();
                MySqlConnection connection = _instance.Instance(_config);
                string query = $"SELECT id, name, url_image, price, discount, category " +
                               $"FROM {_TableName} " +
                               $"WHERE category in {categoriesQuery2} " +
                               $"LIMIT {end - start} OFFSET {start}";
                var reader = await _instance.ExecutePetition(query, connection);

                while (reader.Read())
                {
                    Product product = new Product();
                    product.Id = int.Parse(reader["id"].ToString());
                    product.Name = reader["name"].ToString();
                    product.UrlImage = reader["url_image"].ToString();
                    product.Discount = int.Parse(reader["discount"].ToString());
                    product.Price = float.Parse(reader["price"].ToString());
                    product.Category = int.Parse(reader["category"].ToString());
                    products.Add(product);
                }
                connection.Close();
                var productsResult = await Task<ICollection<Product>>.FromResult(products);

                return productsResult;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Un error ocurrio al buscar los productos, error: {ex.Message}, en: {ex.StackTrace}");
            }
            return null;
        }

        /*Accion para consultar por total de productos dado una lista de categorias y rango de busqueda*/
        public async Task<int> GetByCategoryCount(List<int> categories)
        {
            try
            {
                string categoriesQuery = "(";
                foreach (var x in categories)
                {
                    categoriesQuery += x + ",";
                }

                string categoriesQuery2 = categoriesQuery.Remove(categoriesQuery.Length -1) + ")";
                ICollection<Product> products = new List<Product>();
                MySqlConnection connection = _instance.Instance(_config);
                string query = $"SELECT COUNT(*)  as total " +
                               $"FROM {_TableName} " +
                               $"WHERE category in {categoriesQuery2} ";
                var reader = await _instance.ExecutePetition(query, connection);
                int total = 0;
                while (reader.Read())
                {
                    total = int.Parse(reader["total"].ToString());
                }
                connection.Close();

                return total;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Un error ocurrio al contar los productos por categoria, error: {ex.Message}, en: {ex.StackTrace}");
            }
            return -1;
        }

        /*Accion para consultar por productos dado una lista de categorias, rango de precio y rango de busqueda*/
        public async Task<ICollection<Product>> GetByFilters(List<int> categories, int minPrice, int maxPrice, int start, int end)
        {
            try
            {
                ICollection<Product> products = new List<Product>();
                MySqlConnection connection = _instance.Instance(_config);
                string query = $"SELECT id, name, url_image, price, discount, category " +
                               $"FROM {_TableName} " +
                               $"WHERE price between {minPrice} AND {maxPrice} ";

                if(categories.Count != 0)
                {
                    string categoriesQuery = "(";
                    foreach (var x in categories)
                    {
                        categoriesQuery += x + ",";
                    }

                    string categoriesQuery2 = categoriesQuery.Remove(categoriesQuery.Length - 1) + ")";
                    query += $"AND category in {categoriesQuery2} ";
                }
                query += $"LIMIT {end - start} OFFSET {start}";


                var reader = await _instance.ExecutePetition(query, connection);

                while (reader.Read())
                {
                    Product product = new Product();
                    product.Id = int.Parse(reader["id"].ToString());
                    product.Name = reader["name"].ToString();
                    product.UrlImage = reader["url_image"].ToString();
                    product.Discount = int.Parse(reader["discount"].ToString());
                    product.Price = float.Parse(reader["price"].ToString());
                    product.Category = int.Parse(reader["category"].ToString());
                    products.Add(product);
                }
                connection.Close();
                var productsResult = await Task<ICollection<Product>>.FromResult(products);

                return productsResult;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Un error ocurrio al buscar los productos, error: {ex.Message}, en: {ex.StackTrace}");
            }
            return null;
        }

        /*Accion para consultar por el totla de productos dado una lista de categorias, rango de precio y rango de busqueda*/
        public async Task<int> GetByFiltersCount(List<int> categories, int minPrice, int maxPrice)
        {
            try
            {
                MySqlConnection connection = _instance.Instance(_config);
                string query = $"SELECT COUNT(*) as total " +
                               $"FROM {_TableName} " +
                               $"WHERE price between {minPrice} AND {maxPrice} ";

                if (categories.Count != 0)
                {
                    string categoriesQuery = "(";
                    foreach (var x in categories)
                    {
                        categoriesQuery += x + ",";
                    }

                    string categoriesQuery2 = categoriesQuery.Remove(categoriesQuery.Length - 1) + ")";
                    query += $"AND category in {categoriesQuery2} ";
                }


                var reader = await _instance.ExecutePetition(query, connection);
                int total = 0;

                while (reader.Read())
                {
                    
                    total = int.Parse(reader["total"].ToString());

                }
                connection.Close();

                return total;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Un error ocurrio al contar los productos por filtro, error: {ex.Message}, en: {ex.StackTrace}");
            }
            return -1;
        }

        /*Accion para consultar por productos dado un texto plano y rango de busqueda*/
        public async Task<ICollection<Product>> GetBySearch(string text, int start, int end)
        {
            try
            {

                ICollection<Product> products = new List<Product>();
                MySqlConnection connection = _instance.Instance(_config);
                string query = $"SELECT id, name, url_image, price, discount, category " +
                               $"FROM {_TableName} " +
                               $"WHERE name LIKE '%{text}%' " +
                               $"LIMIT {end - start} OFFSET {start}";
                var reader = await _instance.ExecutePetition(query, connection);

                while (reader.Read())
                {
                    Product product = new Product();
                    product.Id = int.Parse(reader["id"].ToString());
                    product.Name = reader["name"].ToString();
                    product.UrlImage = reader["url_image"].ToString();
                    product.Discount = int.Parse(reader["discount"].ToString());
                    product.Price = float.Parse(reader["price"].ToString());
                    product.Category = int.Parse(reader["category"].ToString());
                    products.Add(product);
                }
                connection.Close();
                var productsResult = await Task<ICollection<Product>>.FromResult(products);

                return productsResult;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Un error ocurrio al buscar los productos, error: {ex.Message}, en: {ex.StackTrace}");
            }
            return null;
        }

        /*Accion para consultar por el total de  productos dado un texto plano y rango de busqueda*/
        public async Task<int> GetBySearchCount(string text)
        {
            try
            {

                MySqlConnection connection = _instance.Instance(_config);
                string query = $"SELECT COUNT(*) as total " +
                               $"FROM {_TableName} " +
                               $"WHERE name LIKE '%{text}%' ";
                var reader = await _instance.ExecutePetition(query, connection);
                int total = 0;

                while (reader.Read())
                {
                    total = int.Parse(reader["total"].ToString());
                }

                connection.Close();
                return total;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Un error ocurrio al contar la busqueda de productos por nombre, error: {ex.Message}, en: {ex.StackTrace}");
            }
            return -1;
        }

    }


}

   


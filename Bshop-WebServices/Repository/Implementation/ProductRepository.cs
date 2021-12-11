﻿using System;
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

        public async Task<ICollection<Product>> GetByCategory(int category, int start, int end)
        {
            try
            {

                ICollection<Product> products = new List<Product>();
                MySqlConnection connection = _instance.Instance(_config);
                string query = $"SELECT id, name, url_image, price, discount, category " +
                               $"FROM {_TableName} " +
                               $"WHERE category = {category} " +
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

        public async Task<int> GetByCategoryCount(int category)
        {
            try
            {

                ICollection<Product> products = new List<Product>();
                MySqlConnection connection = _instance.Instance(_config);
                string query = $"SELECT COUNT(*)  as total " +
                               $"FROM {_TableName} " +
                               $"WHERE category = {category} ";
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

        public async Task<ICollection<Product>> GetByFilters(int category, int minPrice, int maxPrice, int start, int end)
        {
            try
            {

                ICollection<Product> products = new List<Product>();
                MySqlConnection connection = _instance.Instance(_config);
                string query = $"SELECT id, name, url_image, price, discount, category " +
                               $"FROM {_TableName} " +
                               $"WHERE price between {minPrice} AND {maxPrice} ";

                if(category != -1)
                {
                    query += $"AND category = {category} ";
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

        public async Task<int> GetByFiltersCount(int category, int minPrice, int maxPrice)
        {
            try
            {
                MySqlConnection connection = _instance.Instance(_config);
                string query = $"SELECT COUNT(*) as total " +
                               $"FROM {_TableName} " +
                               $"WHERE price between {minPrice} AND {maxPrice} ";

                if (category != -1)
                {
                    query += $"AND category = {category} ";
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
                    total = int.Parse(reader["id"].ToString());
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

   

using Bshop_WebServices.Models;
using Bshop_WebServices.Repository.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace Bshop_WebServices.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductController> _logger;


        public ProductController(IProductRepository repository, ILogger<ProductController> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        [HttpGet]
        [Route("Get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<Product> Get(int id)
        {
            return await _repository.Get(id);
        }

        [HttpGet]
        [Route("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<Product>))]
        public async Task<ICollection<Product>> GetAll(int start, int end)
        {
            return await _repository.GetAll(start, end);
        }

        [HttpGet]
        [Route("GetAllCount")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<int> GetAllCount()
        {
            return await _repository.GetAllCount();
        }

        [HttpGet]
        [Route("GetByCategory")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<Product>))]
        public async Task<ICollection<Product>> GetByCategory(string categoriesJson, int start, int end)
        {
            var categories = JsonConvert.DeserializeObject<ICollection<int>>(categoriesJson);
            List<int> cat2 = categories.ToList();
            return await _repository.GetByCategory(cat2, start, end);
        }

        [HttpGet]
        [Route("GetByCategoryCount")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<int> GetByCategoryCount(string categoriesJson)
        {
            var categories = JsonConvert.DeserializeObject<ICollection<int>>(categoriesJson);
            List<int> cat2 = categories.ToList();
            return await _repository.GetByCategoryCount(cat2);
        }

        [HttpGet]
        [Route("GetByFilters")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<Product>))]
        public async Task<ICollection<Product>> GetByFilters(string categoriesJson, int minPrice, int maxPrice, int start, int end)
        {
            var categories = JsonConvert.DeserializeObject<ICollection<int>>(categoriesJson);
            List<int> cat2 = categories.ToList();
            return await _repository.GetByFilters(cat2,minPrice, maxPrice, start, end);
        }

        [HttpGet]
        [Route("GetByFiltersCount")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<int> GetByFiltersCount(string categoriesJson, int minPrice, int maxPrice)
        {
            var categories = JsonConvert.DeserializeObject<ICollection<int>>(categoriesJson);
            List<int> cat2 = categories.ToList();
            return await _repository.GetByFiltersCount(cat2, minPrice, maxPrice);
        }

        [HttpGet]
        [Route("GetBySearch")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<Product>))]
        public async Task<ICollection<Product>> GetBySearch(string text, int start, int end)
        {
            return await _repository.GetBySearch(text, start, end);
        }

        [HttpGet]
        [Route("GetBySearchCount")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<int> GetBySearchCount(string text)
        {
            return await _repository.GetBySearchCount(text);
        }
    }
}

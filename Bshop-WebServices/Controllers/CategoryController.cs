using Bshop_WebServices.Models;
using Bshop_WebServices.Repository.Contracts;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _repository;
        private readonly ILogger<CategoryController> _logger;


        public CategoryController(ICategoryRepository repository, ILogger<CategoryController> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        [HttpGet]
        [Route("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<Category>))]
        public async Task<ICollection<Category>> GetAll()
        {
            return await _repository.GetAll();
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Helpers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Api.Business.Interface;
using Api.Service.CustomTags;
using Api.Data.ServiceModels;

namespace Api.Service.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    public class GenericController : ControllerBase
    {
        
        
        private readonly IGenericRepository _repository;

        public GenericController(IGenericRepository repository) : base()
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            ApiVersion apiVersion = HttpContext.GetRequestedApiVersion();

            return new string[] { "Api call with Auth success" };
        }


        [HttpPost("getForex")]
        [ValidateModel]
        public IActionResult GetForex([FromBody] GetForexRequest request)
        {
            object result = _repository.GetForex(request);
            return Helper.CheckResult(result);
        }

    }
}
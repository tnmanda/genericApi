using Api.Service.CustomTags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Service.BaseController
{
    [Authorize]
    [ValidateModel]
    [Produces("application/json")]
    //[ApiVersion("1.0")]
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
    public class ControllerBase<T> : Controller where T : ControllerBase<T>
    {
        private ILogger<T> _logger; // log4net implement here
        protected ILogger<T> Logger => _logger ?? (_logger = HttpContext.RequestServices.GetService<ILogger<T>>());
    }
}

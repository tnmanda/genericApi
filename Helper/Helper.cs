using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Helpers
{
    public static class Helper
    {
        public static IActionResult CheckResult(object data)
        {
            try
            {
                if (data != null )
                {
                    return Ok(data);
                }
                else if (data == null)
                {
                    return NotFound();
                }
                else
                {
                    return (NotOk());
                }
            }
            catch
            {
                return (NotOk());
            }

        }

        private static IActionResult NotOk()
        {
            BadRequestResult badResult = new BadRequestResult();
            return badResult;
        }

        private static IActionResult NotFound()
        {
            NotFoundResult notFoundResult = new NotFoundResult();
            return notFoundResult;
        }

        private static IActionResult Ok(object data)
        {
            OkObjectResult okResult = new OkObjectResult(data);
            return okResult;
        }
        


    }
}

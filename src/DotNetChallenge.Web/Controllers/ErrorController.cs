using Microsoft.AspNetCore.Mvc;

namespace DotNetChallenge.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/GenericError")]
        public IActionResult GenericError()
        {
            return View("GenericError");
        }

        [Route("Error/NotFound")]
        public IActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }

        [Route("Error/ServerError")]
        public IActionResult ServerError()
        {
            Response.StatusCode = 500;
            return View("ServerError");
        }
    }
}

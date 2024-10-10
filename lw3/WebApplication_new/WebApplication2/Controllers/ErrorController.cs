using System.Collections.Generic;
using System.Web.Http;

namespace lw3.Controllers
{
    [RoutePrefix("api/error")]
    public class ErrorController : ApiController
    {
        [Route("{code:int}/{id:int}/{format}")]
        [HttpGet]
        public IHttpActionResult Error(int code, int id, string format)
        {
            var links = new List<dynamic>
            {
                new
                {
                    rel = "self",
                    href = $"/api/error/{code}/{id}/{format}",
                    method = "GET"
                },
                new
                {
                    rel = "help",
                    href = "/api/error/help",
                    method = "GET",
                    description = "Get detailed error information"
                }
            };

            if (format == "json")
            {
                switch (id)
                {
                    default:
                    case 1:
                        return Json(new
                        {
                            message = $"{code}/{id}. There's no entity by this id",
                            links
                        });
                    case 2:
                        return Json(new
                        {
                            message = $"{code}/{id}. Unsupported format",
                            links
                        });
                    case 3:
                        return Json(new
                        {
                            message = $"{code}/{id}. Enter correct data",
                            links
                        });
                    case 4:
                        return Json(new
                        {
                            message = $"{code}/{id}. Поля limit, offset, minId, и maxId must be not negative",
                            links
                        });
                    case 5:
                        return Json(new
                        {
                            message = $"{code}/{id}. minId must be less than maxId.",
                            links
                        });
                }
            }
            else
            {
                switch (id)
                {
                    default:
                    case 1:
                        return Ok(new
                        {
                            message = $"{code}/{id}. There's no entity by this id",
                            links
                        });
                    case 2:
                        return Ok(new
                        {
                            message = $"{code}/{id}. Unsupported format",
                            links
                        });
                    case 3:
                        return Ok(new
                        {
                            message = $"{code}/{id}. Enter correct data",
                            links
                        });
                    case 4:
                        return Ok(new
                        {
                            message = $"{code}/{id}. Поля limit, offset, minId, и maxId must be not negative",
                            links
                        });
                    case 5:
                        return Ok(new
                        {
                            message = $"{code}/{id}. minId must be less than maxId.",
                            links
                        });
                }
            }
        }

        [Route("help")]
        [HttpGet]
        public IHttpActionResult Help()
        {
            var helpMessage = new
            {
                info = "This API provides detailed error information.",
                errors = new Dictionary<int, string>
                {
                    { 1, "No entity by this ID" },
                    { 2, "Unsupported format" },
                    { 3, "Incorrect data entry" },
                    { 4, "Limit, offset, minId, and maxId must be non-negative" },
                    { 5, "minId must be less than maxId" }
                }
            };

            return Json(helpMessage);
        }
    }
}

using System;
using System.Web.Http;

namespace WebApplication2.Controllers
{
    [RoutePrefix("api/error")]
    public class ErrorController : ApiController
    {
        [Route("{code:int}/{id:int}.{format}")]
        [HttpGet]
        public IHttpActionResult Error(int code, int id, string format)
        {
            if (format == "json")
            {
                switch (id)
                {
                    default:
                    case 1:
                        return Json(new { message = $"{code}/{id}. There's no entity by this id" });
                    case 2:
                        return Json(new { message = $"{code}/{id}. Unsupported format" });
                    case 3:
                        return Json(new { message = $"{code}/{id}. Enter correct data" });
                    case 4:
                        return Json(new { message = $"{code}/{id}. Поля limit, offset, minId, и maxId must be not negative" });
                    case 5:
                        return Json(new { message = $"{code}/{id}. minId must be less than maxId." });
                }
            }
            else
            {
                switch (id)
                {
                    default:
                    case 1:
                        return Ok($"{code}/{id}. There's no entity by this id");
                    case 2:
                        return Ok($"{code}/{id}. Unsupported format");
                    case 3:
                        return Ok($"{code}/{id}. Enter correct data");
                    case 4:
                        return Ok($"{code}/{id}. Поля limit, offset, minId, и maxId must be not negative");
                    case 5:
                        return Ok($"{code}/{id}. minId must be less than maxId.");
                }
            }
        }
    }
}

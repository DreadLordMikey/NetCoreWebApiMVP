using Microsoft.AspNetCore.Mvc;

using System.Net;

namespace NetCoreWebApiMVP.Controllers
{
   [ApiVersion("1")]
   [Route("api/v1/foo")]
   [ApiController]
   public class FooController : ControllerBase
   {
      [HttpGet("status")]
      [MapToApiVersion("1")]
      public JsonResult GetStatus()
      {
         return new JsonResult("Version 1") { StatusCode = (int)HttpStatusCode.OK };
      }
   }

   [ApiVersion("2")]
   [Route("api/v2/foo")]
   [ApiController]
   public class Foo2Controller : ControllerBase
   {
      [HttpGet("status")]
      [MapToApiVersion("2")]
      public JsonResult GetStatus()
      {
         return new JsonResult("Version 2") { StatusCode = (int)HttpStatusCode.OK };
      }
   }
}

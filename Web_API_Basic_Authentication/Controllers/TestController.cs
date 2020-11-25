using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using Web_API_Basic_Authentication.Model;

namespace Web_API_Basic_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Route("GetData")]
        [HttpPost]
        public IActionResult GetData([FromBody] LoginModel model)
        {
            return StatusCode(StatusCodes.Status200OK, new { userName = model.UserName, password = model.Password});
        }
    }
}
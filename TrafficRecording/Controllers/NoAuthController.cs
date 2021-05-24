using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TrafficRecording.Controllers
{
    [Route("[controller]")]
    public class NoAuthController : Controller
    {
        [HttpGet]
        [HttpPost]
        [Route("")]
        [Route("/{**article}")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}

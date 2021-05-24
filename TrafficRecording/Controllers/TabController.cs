using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficRecording.Controllers
{
    public class TabController : Controller
    {
        [HttpGet]
        [Route("tab/oauth/token")] //http://auditlog.sportcastlive.com/tab/oauth/token
        public IActionResult GetTabToken()
        {
            return Ok("6a1377b4-dc24-4288-9264-fc106c5543e3");
        }

        [Authorize]
        [HttpPost]
        public IActionResult SendRequest()
        {
            return Ok(true);
        }
    }
}

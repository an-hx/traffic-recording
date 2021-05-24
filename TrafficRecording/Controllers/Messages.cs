using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficRecording.Controllers
{
    public class Messages : Controller
    {
        [Authorize]
        [HttpPost]
        public IActionResult SendRequest()
        {
            return Ok(true);
        }
    }
}

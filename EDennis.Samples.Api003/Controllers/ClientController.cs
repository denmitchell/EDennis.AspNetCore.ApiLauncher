using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using EDennis.Samples.Api003.Models;

namespace EDennis.Samples.Api003.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ClientController(IConfiguration config) {
            _config = config;
        }

        [HttpGet]
        public ActionResult<Client> Get() {
            var client = new Client { ClientId = _config["MockClient"] };
            return client;
        }

    }
}
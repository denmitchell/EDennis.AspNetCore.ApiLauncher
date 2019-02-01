﻿using EDennis.Samples.Api001.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EDennis.Samples.Api001.Controllers {
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
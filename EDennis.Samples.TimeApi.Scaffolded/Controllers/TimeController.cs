using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDennis.Samples.SharedModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.TimeApi.Scaffolded.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class TimeController : ControllerBase {

        private readonly ILogger<TimeController> _logger;
        private readonly CurrentTime _time;

        public TimeController(ILogger<TimeController> logger, CurrentTime time) {
            _logger = logger;
            _time = time;
        }

        [HttpGet]
        public Time Get() {
            return _time;
        }
    }
}

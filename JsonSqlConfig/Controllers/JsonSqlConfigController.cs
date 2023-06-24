using Microsoft.AspNetCore.Mvc;

namespace JsonSqlConfig.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]

    public class JsonSqlConfigController : ControllerBase
    {
        private readonly ILogger<JsonSqlConfigController> _logger;

        public JsonSqlConfigController(ILogger<JsonSqlConfigController> logger)
        {
            _logger = logger;
        }

        [HttpPost("{path}")]
        public IActionResult PostConfig([FromBody]string json, string path)
        {
            return Ok();
        }
    }
}
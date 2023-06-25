using JsonSqlConfig.Experiments;
using Microsoft.AspNetCore.Mvc;

namespace JsonSqlConfig.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]

    public class JsonSqlConfigController : ControllerBase
    {
        private readonly ILogger<JsonSqlConfigController> _logger;
        private readonly IJsonParser _jsonParser;

        public JsonSqlConfigController(
            ILogger<JsonSqlConfigController> logger,
            IJsonParser jsonParser)
        {
            _logger = logger;
            _jsonParser = jsonParser;
        }

        [HttpPost("{json}")]
        public ActionResult<string> PostConfig(string json)
        {
            var parseResult = _jsonParser.Store(json);
            return Ok(parseResult);
        }
    }
}
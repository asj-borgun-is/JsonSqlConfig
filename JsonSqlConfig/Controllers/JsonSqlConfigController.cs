using JsonSqlConfig.Experiments;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

        [HttpPost]
        public ActionResult<string> PostConfig([FromBody]object jsonElement) 
        {
            var element = (JsonElement)jsonElement;

            var unit = _jsonParser.Store(element);

            return Ok(_jsonParser.GetJsonString(unit));
        }

        [HttpPost("{group}")]
        public ActionResult<string> PostConfigInGroup([FromBody]object jsonElement, string group) 
        {
            var element = (JsonElement)jsonElement;

            var unit = _jsonParser.Store(element, group);

            return Ok(_jsonParser.GetJsonString(unit));
        }
    }
}
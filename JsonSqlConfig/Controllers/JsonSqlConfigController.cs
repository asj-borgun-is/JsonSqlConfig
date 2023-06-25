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
            var jdoc = (JsonElement)jsonElement;
            var parseResult = _jsonParser.Store(jdoc);
            return parseResult;
        }
    }
}
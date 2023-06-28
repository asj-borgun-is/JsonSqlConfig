using JsonSqlConfig.Experiments;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace JsonSqlConfig.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]

    public class JsonSqlConfigController : ControllerBase
    {
        private readonly ILogger<JsonSqlConfigController> _logger;
        private readonly IJsonParser _jsonParser;
        private readonly IWebHostEnvironment _environment;

        public JsonSqlConfigController(
            ILogger<JsonSqlConfigController> logger,
            IJsonParser jsonParser,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _jsonParser = jsonParser;
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> PostConfig([FromBody]object jsonElement) 
        {
            return await ActionWrapper(() => PostConfigAction(jsonElement));
        }

        [HttpPost("{group}")]
        public async Task<IActionResult> PostConfigInGroup([FromBody]object jsonElement, string group) 
        {
            return await ActionWrapper(() => PostConfigAction(jsonElement, group));
        }

        [HttpGet("{group}")]
        public async Task<ActionResult<string>> GetConfig(string group)
        {
            return await ActionWrapper(() => GetConfigAction(group));
        }

        private async Task<IActionResult> PostConfigAction(object jsonElement, string group = "")
        {
            group ??= string.Empty;
            if (await _jsonParser.GroupExists(group)) return Conflict($"Group ({group}) already exists.");

            var element = (JsonElement)jsonElement;
            await _jsonParser.Store(element, group);
            return NoContent();
        }

        private async Task<ActionResult<string>> GetConfigAction(string group)
        {
            var jsonString = await _jsonParser.GetJsonString(group);
            if (jsonString == null) return NotFound();
            return Ok(jsonString);
        }

        private async Task<ActionResult<TReturn>> ActionWrapper<TReturn>(Func<Task<ActionResult<TReturn>>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                return GetProblemDetails(ex);
            }
        }

        private async Task<IActionResult> ActionWrapper(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                return GetProblemDetails(ex);
            }
        }

        private ObjectResult GetProblemDetails(Exception ex)
        {
            var errorStatus = (int)HttpStatusCode.InternalServerError;
            if (_environment.IsDevelopment()) return Problem(title: ex.Message, detail: ex.StackTrace, statusCode: errorStatus);
            else return Problem(title: "Internal Error", statusCode: errorStatus);
        }
    }
}
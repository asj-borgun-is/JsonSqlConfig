using JsonSqlConfig.Service;
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
        private readonly IJsonService _jsonService;
        private readonly IWebHostEnvironment _environment;

        public JsonSqlConfigController(
            ILogger<JsonSqlConfigController> logger,
            IJsonService jsonService,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _jsonService = jsonService;
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> PostConfigDefault([FromBody]object jsonElement) 
        {
            return await ActionWrapper(() => PostConfigAction(jsonElement));
        }

        [HttpPost("{group}")]
        public async Task<IActionResult> PostConfig([FromBody]object jsonElement, string group) 
        {
            return await ActionWrapper(() => PostConfigAction(jsonElement, group));
        }

        [HttpGet("{group}")]
        public async Task<ActionResult<string>> GetConfig(string group)
        {
            return await ActionWrapper(() => GetConfigAction(group));
        }

        [HttpDelete("{group}")]
        public async Task<IActionResult> DeleteConfig(string group)
        {
            return await ActionWrapper(() => DeleteConfigAction(group));
        }

        private async Task<IActionResult> PostConfigAction(object jsonElement, string group = "")
        {
            group ??= string.Empty;
            if (await _jsonService.Exists(group)) return Conflict($"Group ({group}) already exists.");
            var element = (JsonElement)jsonElement;

            await _jsonService.Store(element, group);

            return NoContent();
        }

        private async Task<ActionResult<string>> GetConfigAction(string group)
        {
            group ??= string.Empty;

            var jsonString = await _jsonService.Get(group);

            if (jsonString == null) return NotFound();
            return Ok(jsonString);
        }

        private async Task<IActionResult> DeleteConfigAction(string group)
        {
            group ??= string.Empty;
            if (!await _jsonService.Exists(group)) return NotFound();

            await _jsonService.Delete(group);

            return NoContent();
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
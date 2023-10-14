using JsonSqlConfigDb;
using JsonSqlConfigDb.Service;
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
        private readonly IJsonSqlService _jsonService;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public JsonSqlConfigController(
            ILogger<JsonSqlConfigController> logger,
            IJsonSqlService jsonService,
            IWebHostEnvironment environment,
            IConfiguration configuration)
        {
            _logger = logger;
            _jsonService = jsonService;
            _environment = environment;
            _configuration = configuration;
        }

        [HttpPost("{group}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PostConfig([FromBody]object jsonElement, string group) 
        {
            return await ActionWrapper(() => PostConfigAction(jsonElement, group));
        }

        [HttpPut("{group}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PutConfig([FromBody]object jsonElement, string group) 
        {
            return await ActionWrapper(() => PutConfigAction(jsonElement, group));
        }

        [HttpGet("{group}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> GetConfig(string group)
        {
            return await ActionWrapper(() => GetConfigAction(group));
        }

        [HttpDelete("{group}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteConfig(string group)
        {
            return await ActionWrapper(() => DeleteConfigAction(group));
        }

        [HttpPost("{group}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ExistsConfig(string group)
        {
            return await ActionWrapper(() => ExistsConfigAction(group));
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult ReloadConfig()
        {
            return ActionWrapper(() => ReloadConfigAction());
        }

        private async Task<IActionResult> PostConfigAction(object jsonElement, string group = "")
        {
            group ??= string.Empty;
            if (await _jsonService.Exists(group)) return Conflict($"Group ({group}) already exists.");
            var element = (JsonElement)jsonElement;

            await _jsonService.Store(element, group);

            return NoContent();
        }

        private async Task<IActionResult> PutConfigAction(object jsonElement, string group = "")
        {
            group ??= string.Empty;
            if (!await _jsonService.Exists(group)) return NotFound();
            var element = (JsonElement)jsonElement;

            using var txn = _jsonService.Database.BeginTransaction();

            await _jsonService.Delete(group);
            await _jsonService.Store(element, group);

            await txn.CommitAsync();
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

        private async Task<IActionResult> ExistsConfigAction(string group)
        {
            group ??= string.Empty;
            if (!await _jsonService.Exists(group)) return NotFound();

            return NoContent();
        }

        private IActionResult ReloadConfigAction()
        {
            var propertyName = "Logging:LogLevel:Default";
            DebugProperty(propertyName, "before reload");

            _jsonService.LoadProvider();

            DebugProperty(propertyName, "after reload");

            return NoContent();
        }

        private async Task<ActionResult<TReturn>> ActionWrapper<TReturn>(Func<Task<ActionResult<TReturn>>> action)
        {
            try
            {
                return await action();
            }
            catch (JsonSqlException je)
            {
                return Conflict(je.Message);
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
            catch (JsonSqlException je)
            {
                return Conflict(je.Message);
            }
            catch (Exception ex)
            {
                return GetProblemDetails(ex);
            }
        }

        private IActionResult ActionWrapper(Func<IActionResult> action)
        {
            try
            {
                return action();
            }
            catch (JsonSqlException je)
            {
                return Conflict(je.Message);
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

        private void DebugProperty(string propertyName, string when)
        {
            var property = _configuration[propertyName];
            _logger.LogDebug("Property {propertyname} has value {property}, {when}", propertyName, property, when);
        }
    }
}
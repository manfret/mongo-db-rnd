using Microsoft.AspNetCore.Mvc;
using mongo_db_rnd.Api.Mongo;

namespace mongo_db_rnd.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly MongoRepository _mongoRepository;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            MongoRepository mongoRepository)
        {
            _logger = logger;
            _mongoRepository = mongoRepository;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCollection([FromQuery] string collectionName)
        {
            await _mongoRepository.CreateCollectionAsync(collectionName);
            var obj = new { collectionName };
            return Ok(obj);
        }

        [HttpPost]
        [Route("{collectionName}/add")]
        public async Task<IActionResult> AddValueCollection([FromRoute] string collectionName, [FromBody] RequestValue request)
        {
            var value = request.Value;
            var res = await _mongoRepository.PushValueAsync(collectionName, value);
            return Ok(res);
        }

        [HttpDelete]
        [Route("{collectionName}")]
        public async Task<IActionResult> DeleteValueFromCollection([FromRoute] string collectionName, [FromBody] RequestValue request)
        {
            var value = request.Value;
            var res = await _mongoRepository.DeleteValueAsync(collectionName, value);
            return Ok(res);
        }

        [HttpGet]
        [Route("{collectionName}")]
        public async Task<IActionResult> ReadFirstCollection([FromRoute] string collectionName)
        {
            var res = await _mongoRepository.ReadAsync(collectionName);
            return Ok(res);
        }
    }
}
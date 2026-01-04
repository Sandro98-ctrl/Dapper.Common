using Dapper.Common.UoW;
using Microsoft.AspNetCore.Mvc;

namespace Dapper.Common.Web.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(ILogger<WeatherForecastController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetAll(
        [FromServices] IDbSession dbSession)
    {
        logger.LogInformation("Obtendo todos...");

        var query =
            """
            SELECT 
                wf.id AS Id,
                wf.date_weather AS Date,
                temperature_c AS TemperatureC,
                summary AS Summary
            FROM weather_forecast wf
            """;

        var records = await dbSession.Connection.QueryAsync<WeatherForecast>(query);

        return Ok(records);
    }

    [HttpGet("{id}", Name = nameof(GetById))]
    public async Task<ActionResult<WeatherForecast>> GetById(
        int id,
        [FromServices] DapperContext dapperContext,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Obtendo todos...");

        var query =
            """
            SELECT 
                wf.id AS Id,
                wf.date_weather AS Date,
                temperature_c AS TemperatureC,
                summary AS Summary
            FROM weather_forecast wf
            WHERE wf.id = @id
            """;

        var record = await dapperContext.QueryFirstOrDefaultAsync<WeatherForecast>(query, new { id }, cancellationToken);

        return record switch
        {
            null => NotFound(),
            _ => Ok(record)
        };
    }

    [HttpPost]
    public async Task<ActionResult> CreateWeatherForecast(
        WeatherForecast request,
        [FromServices] DapperContext dapperContext,
        [FromServices] IUnitOfWork unitOfWork,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Criando registro...");

        var sql =
            """
            INSERT INTO weather_forecast 
                (date_weather, temperature_c, summary)
                VALUES 
                (@Date, @TemperatureC, @Summary);
            select last_insert_rowid()
            """;

        var parameters = new
        {
            request.Date,
            request.TemperatureC,
            request.Summary
        };

        var result = await unitOfWork.ExecuteInTransactionAsync(
            async ct => await dapperContext.ExecuteAsync(sql, parameters, ct),
            cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = result }, null);
    }
}

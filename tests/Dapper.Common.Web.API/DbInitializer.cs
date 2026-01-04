using Microsoft.Data.Sqlite;

namespace Dapper.Common.Web.API;

public static class DbInitializer
{
    public static bool CreateDataBaseTableWeatherForecast(this WebApplication app)
    {
        var connectionString = app.Configuration.GetConnectionString("Default");

        try
        {
            using var cnn = new SqliteConnection(connectionString);
            cnn.Open();
            cnn.Query(
                        @"
                                        PRAGMA foreign_keys = off;
                                        BEGIN TRANSACTION;
                                        -- Table: weather_forecast
                                        CREATE TABLE weather_forecast (
                                            id INTEGER       PRIMARY KEY UNIQUE NOT NULL,
                                            date_weather DATETIME,
                                            temperature_c INTEGER,
                                            summary VARCHAR (500)
                                        );
                                        COMMIT TRANSACTION;
                                        PRAGMA foreign_keys = on;
                                    ");

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}

using Dapper.Common;
using Dapper.Common.Sqlite;
using Dapper.Common.UoW;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDapperContext((options) =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default")!;
    //var configuration = sp.GetRequiredService<IConfiguration>();
    //var connectionString = configuration.GetConnectionString("Default")!;
    options.UseSqlite(connectionString);
    options.AddUnitOfWork();
});

//builder.Services.AddDapperCore((options) =>
//{
//    var connectionString = builder.Configuration.GetConnectionString("Default")!;
//    options.UseSqlite(connectionString);
//    options.AddUnitOfWork();
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //app.CreateDataBaseTableWeatherForecast();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using WeatherForecast.Helper;
using WeatherForecast.Mappings;
using WeatherForecast.Service;
using WeatherForecast.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container
builder.Services.AddControllers().AddJsonOptions(o => JsonHelper.SetOptions(o.JsonSerializerOptions));

// Register the weather services
builder.Services.AddSingleton<CountryService>();
builder.Services.AddHttpClient<CountryService>();
builder.Services.AddHttpClient<XxWeatherService>();
builder.Services.AddHttpClient<OpenMeteoService>();

builder.Services.AddTransient<IWeatherAggregatorService, XxWeatherService>();
builder.Services.AddTransient<IWeatherAggregatorService, OpenMeteoService>();
builder.Services.AddTransient<WeatherAggregatorService>();
builder.Services.RegisterMaps();


// Add memory cache
builder.Services.AddMemoryCache();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandlingMiddlewares>(); // Middleware is registered here
app.MapControllers();
app.Run();
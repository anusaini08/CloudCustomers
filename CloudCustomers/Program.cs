using CloudCustomers.Models;
using CloudCustomers.Services;

var builder = WebApplication.CreateBuilder(args);

// Load Redis configuration from appsettings.json
var redisSettings = builder.Configuration.GetSection("RedisCacheSettings").Get<RedisCacheSettings>();

// Add Redis distributed cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisSettings.ConnectionString;
});

// Add services to the container.

ConfigureServices(builder.Services);

builder.Services.AddControllers();

// Add in-memory caching
builder.Services.AddMemoryCache();

// Enable output caching middleware
builder.Services.AddOutputCache();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use middleware for output caching
app.UseOutputCache();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddTransient<IUsersService, UsersService>();
    services.AddHttpClient<IUsersService, UsersService>();
}

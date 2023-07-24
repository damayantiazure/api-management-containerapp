using System.Text.Json;
using NeptureWebAPI;
using NeptureWebAPI.AzureDevOps;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<AppConfig>();
builder.Services.AddLogging(logging =>
{
    logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
    logging.AddConsole();
    logging.AddDebug();
});
builder.Services.AddSingleton(new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
    AllowTrailingCommas = true,
    PropertyNameCaseInsensitive = true
});
builder.Services.AddHttpClient(
    AppConfig.AZUREDEVOPSCLIENT,
    (services, client) =>
    {
        client.BaseAddress = new Uri(AppConfig.AZDO_URI);
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<Client>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// We will run this on a docker container, so we need to allow any host
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

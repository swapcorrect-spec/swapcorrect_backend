using SwapShop.Api.Extension;
using SwapShop.Api.ResponsHandler;
using SwapShop.Infrastructure.OtherService.Implementation;
using SwapSwap.Api.MappingProfile;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// CreateBuilder already layers appsettings.json -> appsettings.{Environment}.json -> env vars,
// so any setting can be overridden at deploy time, e.g. EmailConfiguration__Password or
// ConnectionStrings__ProdDB (double underscore maps to the ':' section separator).
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.ConfigureLibrary(builder.Configuration);
builder.Services.ConfigureDb(builder.Configuration);
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.ConfigureEvent(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();
Seeder.SeedData(app).Wait();
app.UseCors();
// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{*/
app.UseSwagger();
    app.UseSwaggerUI();
/*}*/
app.MapHub<ChatHub>("/chathub");
app.MapHub<NotificationHub>("/notificationhub");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

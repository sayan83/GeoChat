using GeoChat.ChatAPI.Services;
using GeoChat.DataLayer.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .WriteTo.File("logs/chats_log.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddHostedService<NotificationWorker>();

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(options => {
//     options.CustomSchemaIds(type => type.ToString());
// });

builder.Services.AddScoped<IGeoChatRepository,GeoChatRepository>();
// builder.Services.AddScoped<INotificationService,NotificationService>();

var app = builder.Build();

app.UseWebSockets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

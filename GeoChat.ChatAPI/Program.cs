using GeoChat.ChatAPI;
using GeoChat.ChatAPI.Services;
using Microsoft.EntityFrameworkCore;
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

string? roomServiceURI = builder.Configuration.GetValue<string>("Services:RoomService");
builder.Services.AddHttpClient("RoomsService", httpClient => {
    httpClient.BaseAddress = new Uri(roomServiceURI);
    // httpClient.DefaultRequestHeaders.Add(
    //     HeaderNames.ContentType, "application/json"
    // );
});
builder.Services.AddDbContext<ChatDBContext>(dbContextOptions => {
    dbContextOptions.UseSqlite("Data Source=GeoChatMessages.db");
});
builder.Services.AddScoped<IChatRepository, ChatRepository>();
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

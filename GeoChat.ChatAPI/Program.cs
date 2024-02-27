using GeoChat.ChatAPI.Services;
using GeoChat.DataLayer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<NotificationWorker>();

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(options => {
//     options.CustomSchemaIds(type => type.ToString());
// });

builder.Services.AddScoped<IGeoChatRepository,GeoChatRepository>();
builder.Services.AddScoped<INotificationService,NotificationService>();

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

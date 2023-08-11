using Serilog;
using Serilog.Sinks.Fluentd;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var log = new LoggerConfiguration().WriteTo.Fluentd("localhost", 8021).CreateLogger();
#region Logs to Dynatrace 
/// These are the lines
//Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
//builder.Host.UseSerilog(((ctx, lc) => lc

//.ReadFrom.Configuration(ctx.Configuration)));
#endregion

var app = builder.Build();

#region Final Line Dynatrace
////
///Final lines to make ILogger as logger component of Dyntrace
//app.UseSerilogRequestLogging();

#endregion

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

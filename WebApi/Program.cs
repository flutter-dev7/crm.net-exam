using Infrastructure.Context;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.MSSqlServer;
using WebApi.MiddleWare;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<DataContext>();
builder.Services.AddScoped<GroupService>();
builder.Services.AddScoped<IProgressBookService, ProgressBookService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<IStudentGroupService, StudentGroupService>();
builder.Services.AddScoped<ITimeTableService, TimeTableService>();

Log.Logger = new LoggerConfiguration()
.WriteTo.Console()
.WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
.CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1");
    });
}
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
using TaskManagerDapper.Commons;
using TaskManagerDapper.Middlewares;
using TaskManagerDapper.Repositories;
using TaskManagerDapper.Services;
using Serilog;
using NLog.Web;



//serilog configuration
Log.Logger = new LoggerConfiguration()
    .WriteTo.Debug()
    .MinimumLevel.Information()
    .WriteTo.Seq("http://localhost:5432")
    .CreateLogger();
Log.Information("Hello, {Name}!", Environment.UserName);

// Important to call at exit so that batched events are flushed.
Log.CloseAndFlush();

var builder = WebApplication.CreateBuilder(args);

// in built logget api
//builder.Logging.ClearProviders(); //To clear all the default providers
//builder.Logging.AddDebug(); //Added Debug and Console Providers only
//builder.Logging.AddConsole();

//serilog logging provider
builder.Host.UseSerilog((context, LoggerConfiguration) =>
{
    LoggerConfiguration.WriteTo.Debug();
    LoggerConfiguration.ReadFrom.Configuration(context.Configuration)
    .WriteTo.File("logs/MyAppLog.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7, fileSizeLimitBytes: 10485760);
});

//Log4Net Log Provider
//builder.Logging.ClearProviders();
//builder.Logging.AddLog4Net();

//NLog Log Provider
//builder.Logging.ClearProviders();
//builder.Host.UseNLog();

// Add services to the container.
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskLogTimeEntriesRepository, TaskLogTimeEntriesRepository>();
builder.Services.AddScoped<ITaskLogTimeEntriesService, TaskLogTimeEntriesService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")  // Replace with your frontend URL or port
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();
app.UseCors("AllowSpecificOrigin");
app.UseLogging();
app.UseGlobalException();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

using Microsoft.EntityFrameworkCore;
using Tekton.Infrastructure.Interfaces;
using Tekton.Infrastructure;
using Tekton.Service;
using Tekton.Service.Interfaces;
using Tekton.Infrastructure.Repositories;
using System.ComponentModel;
using Tekton.Infrastructure.Repositories.Decorators;
using System.Reflection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
var logger = new LoggerConfiguration()
    //.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    //.MinimumLevel.Override("System", LogEventLevel.Information)
    .WriteTo.File("logs/devsu-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .CreateLogger();
builder.Logging.AddSerilog(logger);
Log.Logger = logger;
builder.Host.UseSerilog(logger);
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(x => x.GetCustomAttributes<DisplayNameAttribute>().SingleOrDefault().DisplayName);
});

var connectionString = builder.Configuration.GetConnectionString("sqlConnection");
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<TektonContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<Func<TektonContext>>((provider) => () => provider.GetService<TektonContext>());
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IStatusService, StatusService>();
builder.Services.AddTransient<IHttpRepository, HttpRepository>();
builder.Services.AddTransient<IDiscountService, DiscountService>();
builder.Services.AddScoped<DbFactory>();
builder.Services.AddScoped<IWorkUnit, WorkUnit>();
builder.Services.AddScoped(typeof(Repository<>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(RepositoryCacheDecorator<>));

builder.Services.AddMemoryCache();

//builder.Services.AddHttpLogging(o => { });
//var LogPath = builder.Configuration.GetSection("LogPath").Value;
//builder.Services.AddW3CLogging(logging =>
//{
//    logging.LoggingFields = W3CLoggingFields.All;
//    logging.FileSizeLimit = 5 * 1024 * 1024;
//    logging.RetainedFileCountLimit = 2;
//    logging.FileName = "TektonLogs";
//    logging.LogDirectory = LogPath;
//    logging.FlushInterval = TimeSpan.FromSeconds(2);
//});

var app = builder.Build();
//app.UseHttpLogging();
//app.UseW3CLogging();
//app.UseMiddleware<SerilogMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
}



app.UseStaticFiles();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
Log.CloseAndFlush();

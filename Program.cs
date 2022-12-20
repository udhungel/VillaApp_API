using Microsoft.EntityFrameworkCore;
using Serilog;
using VillaApp_WebAPI.AutoMapper;
using VillaApp_WebAPI.Data;
using VillaApp_WebAPI.Logging;
using VillaApp_WebAPI.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//logger configuration using SiriLog
Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("log/villaLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();


builder.Services.AddControllers(options =>{
                                             //options.ReturnHttpNotAcceptable = true; //for text/plain need to comment it
                                          }).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
builder.Services.AddDbContext<ApplicationDBContext>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
                                                               });
builder.Services.AddScoped<IVillaRepository, VillaRepository>();

//AutoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<Ilogging, Logging>();

var app = builder.Build();


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

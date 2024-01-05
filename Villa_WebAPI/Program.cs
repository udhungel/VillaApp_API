using MagicVilla_API.Repository;
using MagicVilla_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Serilog;
using VillaApp_WebAPI.AutoMapper;
using VillaApp_WebAPI.Data;
using VillaApp_WebAPI.Logging;
using VillaApp_WebAPI.Repository;
using VillaApp_WebAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

//AutoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x => {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }); 

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
app.UseAuthentication(); //need to Authentication before Authorization

app.UseAuthorization();

app.MapControllers();

app.Run();

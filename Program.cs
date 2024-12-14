using ContabilidaMarket.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;  // Se agregó esta línea para Swagger
using WorkerService0001;
using ContabilidaMarket.SignalR;  // Importa el namespace correcto para tu Worker
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.Json.Serialization;
//using StackExchange.Redis;
using ContabilidaMarket.interfaces01; // Asegúrate de usar Microsoft.AspNetCore.SignalR
using ContabilidaMarket.clasesUtiles;


var builder = WebApplication.CreateBuilder(args);

// Configuración de CORS
//builder.WithOrigins("http://127.0.0.1:5500")
//builder.WithOrigins("http://localhost:5173")
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
      builder =>
      {
    builder.WithOrigins("http://localhost:5173", "https://marcelo-sd.github.io")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
      });
});




builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSql")));

builder.Services.AddControllers();
//builder.Services.AddControllers().AddJsonOptions(options =>
//    { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; 
//    options.JsonSerializerOptions.WriteIndented = true; });
//options.Configuration = "localhost:6379";

builder.Services.AddEndpointsApiExplorer();
// Registrar la interfaz y su implementación 
builder.Services.AddScoped<IAltasMain, Ventas>();
///////////////////////////////////////////////////////
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//es de redis cloud , y la puedo usar por 30 dias  des 6/11
//options.Configuration = "redis-10700.c100.us-east-1-4.ec2.redns.redis-cloud.com:10700,password=oT8FRcBtyPPUakQaS9BU9r344MtFojAo";
//options.Configuration = "rediss://red-csl8tfe8ii6s73c1jqig:JeYjiAnGGEVDMlTbqv3AaWHBfraf0VBI@oregon-redis.render.com:6379";
// options.Configuration = "localhost:6379,password=1234";
//  options.InstanceName = "SampleInstance";


//});
///////////////////////////////////

////////////////////////////////////////////////////
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ContabilidaMarket API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT en el siguiente formato: Bearer {your token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
//////////////////////////////////////////////

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//////////////////////////////
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),
            RoleClaimType = ClaimTypes.Role
        };
    });

// Configurar la autorización basada en roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("1"));
});
/////////////////////////////////////////
builder.Services.AddSingleton<IColaDeTareasEnSEgundoPlano, ColaDeTareasEnSegundoPlano>();
builder.Services.AddSingleton<CallBackTareasCompletadas>(message
    => Console.WriteLine($"Callback: {message}"));
builder.Services.AddHostedService<Worker>();
builder.Services.AddSignalR(); // Agrega SignalR a los servicios

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{

//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins"); // Usar la política de CORS
app.UseAuthentication();  // Asegúrate de que esto esté antes de Authorization
app.UseAuthorization();


app.MapControllers();



app.MapHub<NotificacionSingIr>("/notificationHub"); // Mapeo del Hub fuera de UseEndpoints


app.Run();

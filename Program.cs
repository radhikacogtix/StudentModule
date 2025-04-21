using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Std.Data.Services;
using Std.Data;
using Microsoft.AspNetCore.Authentication;
using StudentCRUD.Middleware;
using StudentCRUD;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddXmlSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "StudentCRUD API",
        Version = "v1",
        Description = "API for managing students"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowPartners", policy =>
    {
        policy.WithOrigins("http://localhost:5500")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddScoped<IStudentService, StudentService>();


builder.Services.AddHttpClient("InternalAPI")
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });

builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthentication>("BasicAuthentication",null);


var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);
var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
var secretKey = jwtSettings.Secretkey;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secretkey)),

    };
});
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudentCRUD API v1");
    });
}


app.Use(async (context, next) =>
{
    var path = context.Request.Path;

   
    if (path.StartsWithSegments("/swagger") || path == "/")
    {
        await next();
        return;
    }

    if (path.StartsWithSegments("/api/internal"))
    {
        await next();
        return;
    }
    /*
    if (!context.Request.Headers.TryGetValue("X-API-KEY", out var extractedApiKey))
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("API Key is missing.");
        return;
    }

    var apiKey = builder.Configuration["ApiKey"];
    if (!apiKey.Equals(extractedApiKey))
    {
        context.Response.StatusCode = 403;
        await context.Response.WriteAsync("Unauthorized access.");
        return;
    }*/

    await next();
});

app.Use(async (context, next) =>
{
    var path = context.Request.Path;

    if (path.StartsWithSegments("/api/internal"))
    {
        var remoteIp = context.Connection.RemoteIpAddress?.ToString();
        var allowedIps = new[] { "127.0.0.1", "::1", "192.168.1.5" };

        if (!allowedIps.Contains(remoteIp))
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync($"Access Denied for IP: {remoteIp}");
            return;
        }
    }

    await next();
});

app.UseMiddleware<ApiKeyMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowPartners");
app.MapControllers();
app.Run();

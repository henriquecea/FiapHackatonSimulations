using FiapHackatonSimulations.Application.Service.RabbitMQ;
using FiapHackatonSimulations.Domain.Handler;
using FiapHackatonSimulations.Domain.Interface.RabbitMQ;
using FiapHackatonSimulations.Domain.Interface.Service;
using FiapHackatonSimulations.Domain.Settings;
using FiapHackatonSimulations.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System.Text;

namespace FiapHackatonSimulations.WebAPI.Extension;

public static class BuilderConfiguration
{
    public static void AddDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    }

    public static void AddRabbitMQ(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<RabbitMqSettings>(
            builder.Configuration.GetSection("RabbitMq"));

        builder.Services.AddSingleton<RabbitMQ.Client.IConnectionFactory>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

            return new ConnectionFactory
            {
                HostName = settings.Host,
                Port = settings.Port,
                UserName = settings.Username,
                Password = settings.Password
            };
        });

        builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();

        builder.Services.AddScoped<IRabbitMQReceiver, RabbitMQReceiver>();
        builder.Services.AddScoped<IRabbitMQMessageHandler, SimulationHandler>();

        builder.Services.AddHostedService<RabbitMQConsumer>();
    }

    public static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fiap Hackaton - Simulation", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
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
                    Array.Empty<string>()
                }
            });
        });
    }

    public static void AddAuthWithJWT(this WebApplicationBuilder builder)
    {
        var secretKey = builder.Configuration["JWT:SecretKey"]
                      ?? throw new InvalidOperationException("JWT Secret Key is not configured.");

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
    }
}
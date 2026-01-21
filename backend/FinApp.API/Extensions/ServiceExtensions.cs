using FinApp.Core.Interfaces;
using FinApp.Core.Services;
using FinApp.Core.Mappings;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using FinApp.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FinApp.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        // Repositories & Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IStpbService, StpbService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProgramService, ProgramService>();
        services.AddScoped<IKegiatanService, KegiatanService>();
        services.AddScoped<IOutputService, OutputService>();
        services.AddScoped<ISuboutputService, SuboutputService>();
        services.AddScoped<IKomponenService, KomponenService>();
        services.AddScoped<ISubkomponenService, SubkomponenService>();
        services.AddScoped<IAkunService, AkunService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IJwtService, JwtService>();

        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        // FluentValidation
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<FinApp.Core.Validators.LoginRequestValidator>();

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSecret = configuration["Jwt:Secret"] ?? throw new ArgumentNullException("JWT Secret not configured");
        var jwtIssuer = configuration["Jwt:Issuer"] ?? "FinAppAPI";
        var jwtAudience = configuration["Jwt:Audience"] ?? "FinAppClient";

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,
                ValidateAudience = true,
                ValidAudience = jwtAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        return services;
    }
}

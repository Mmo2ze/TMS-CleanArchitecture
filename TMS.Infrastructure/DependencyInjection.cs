﻿using System.Text;
using Coravel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Refit;
using TMS.Application.Common.Interfaces.Auth;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Repositories;
using TMS.Infrastructure.Auth;
using TMS.Infrastructure.Persistence;
using TMS.Infrastructure.Persistence.Interceptors;
using TMS.Infrastructure.Persistence.Repositories;
using TMS.Infrastructure.Services;
using TMS.Infrastructure.Services.BackGroundJobs;
using TMS.Infrastructure.Services.WhatsappSender;
using TMS.Infrastructure.Services.WhatsappSender.ApiDefinition;


namespace TMS.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceProvider services, IConfiguration configuration)
    {
        services.UseScheduler(sc =>
        {
            sc.Schedule<WhatsappCheckerJob>()
                .Cron("01 * * * *");
            sc.Schedule<AttendanceCheckerJob>()
                .EveryMinute()
                .PreventOverlapping("AttendanceCheckerJob")
                .RunOnceAtStart();
            sc.Schedule<PaymentCleanerJob>()
                .Cron("5 3 1 * *")
                .PreventOverlapping("PaymentCleanerJob")
                .RunOnceAtStart();
        });
        
    }

    public static void AddInfrastructure(this IServiceCollection services,
        ConfigurationManager builderConfiguration)
    {
        services.AddScoped<ICodeManger, CodeManger>();
        services.AddScoped<IClaimGenerator, ClaimGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<ICookieManger, CookieManger>();
        services.AddSingleton<IClaimsReader, ClaimsReader>();
        services.AddSingleton<ITeacherHelper, TeacherHelper>();
        services.AddPersistence(builderConfiguration);
        services.AddAuthentication(builderConfiguration);
        services.AddLogging();
        services.AddScheduler();
        AddBackGroundJobs(services);

        AddWhatsappService(services);
    }

    private static void AddBackGroundJobs(IServiceCollection services)
    {
        services.AddTransient<WhatsappCheckerJob>();
        services.AddTransient<AttendanceCheckerJob>();
        services.AddTransient<PaymentCleanerJob>();
    }

    private static void AddWhatsappService(IServiceCollection services)
    {
        services.AddOptions<WhatsappSenderSettings>()
            .BindConfiguration(WhatsappSenderSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IWhatsappSender, WhatsappSender>();
        services.AddRefitClient<IWhatsappApi>()
            .ConfigureHttpClient((sp, client) =>
                {
                    var settings = sp.GetRequiredService<IOptions<WhatsappSenderSettings>>().Value;
                    client.BaseAddress = new Uri(settings.BaseUrl);
                    client.DefaultRequestHeaders.Add("X-RapidAPI-Key", settings.RapidKey);
                    client.DefaultRequestHeaders.Add("X-RapidAPI-Host", settings.RapidHost);
                }
            );
    }

    private static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()!;
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = jwtSettings.Audience,
                ValidIssuer = jwtSettings.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
            });
    }

    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<PublishDomainEventsInterceptor>();
        services.AddDbContext<MainContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Postgres"))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        });
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "TMS";
        });
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<ITeacherRepository, TeacherRepository>();
        services.AddScoped<IAssistantRepository, AssistantRepository>();
        services.AddScoped<IParentRepository, ParentRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        services.AddScoped<ISchedulerRepository, SchedulerRepository>();
        services.AddScoped<IHolidayRepository, HolidayRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ICardOrderRepository, CardOrderRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
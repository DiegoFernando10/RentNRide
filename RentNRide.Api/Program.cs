using Microsoft.AspNetCore.Diagnostics;
using Minio;
using Newtonsoft.Json.Serialization;
using RentNRide.Common.Domain.Exceptions;
using RentNRide.Common.Domain.Interfaces;
using RentNRide.Common.Domain.Services;
using RentNRide.Data.Entities;
using RentNRide.Data.PostgreSql;
using RentNRide.Listener.MotorcycleConsumer;
using RentNRide.Providers.RabbitMqPublisher;
using RentNRide.Service.Driver;
using RentNRide.Service.Motorcycle;
using RentNRide.Service.Plan;
using RentNRide.Service.Rental;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// ===== Db =====
builder.Services.AddDbContext<RentNRideDbContext>((provider, options) =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    options.UsePostgresFromEnv(configuration);
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ===== Services =====
builder.Services.AddScoped<IMotorcycleService, MotorcycleService>();
builder.Services.AddScoped<IMotorcycleRegisteredService, MotorcycleRegisteredService>();
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IPlanService, PlanService>();

// ===== Controllers =====
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers()
       .AddNewtonsoftJson(options =>
       {
           options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
           options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
       });

// ===== Providers =====
builder.Services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
// ===== Listener =====
builder.Services.AddHostedService<MotorcycleConsumer>();

// ===== MinIO =====
builder.Services.AddSingleton<IMinioClient>(sp =>
{
    var host = builder.Configuration["MINIO_HOST"];
    var port = int.Parse(builder.Configuration["MINIO_PORT"]);
    var user = builder.Configuration["MINIO_USER"];
    var password = builder.Configuration["MINIO_PASSWORD"];

    return new MinioClient()
        .WithEndpoint(host, port)
        .WithCredentials(user, password)
        .Build();
});

// ===== API Versioning =====
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
});

var app = builder.Build();

app.UseCors("Default");

app.UseExceptionHandler(builder =>
{
    builder.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var ex = context.Features.Get<IExceptionHandlerFeature>();
        if (ex != null)
        {
            object error;

            if (ex.Error is ModelValidationException validationEx)
            {
                error = new
                {
                    message = validationEx.Message,
                    failures = validationEx.Failures,
                    trace = validationEx.StackTrace
                };
            }
            else
            {
                switch (ex.Error)
                {
                    case NotFoundException:
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case ApiException:
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                }

                error = new
                {
                    message = ex.Error.InnerException?.Message ?? ex.Error.Message,
                    trace = ex.Error.StackTrace
                };
            }

            await context.Response.WriteAsJsonAsync(error);
        }
    });
});


app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();

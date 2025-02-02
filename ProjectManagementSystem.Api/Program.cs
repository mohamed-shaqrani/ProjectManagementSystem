using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Api.Config;
using ProjectManagementSystem.Api.Data;
using ProjectManagementSystem.Api.Extensions;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Helpers;
using ProjectManagementSystem.Api.MappingProfiles;
using ProjectManagementSystem.Api.Middlewares;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var emailConfig = builder.Configuration
        .GetSection("EmailSettings")
        .Get<EmailConfiguration>();



builder.Services.AddSingleton(emailConfig);

builder.Services.AddMediatR(AssemblyReference.Assembly);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); ;
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new AutofacModule());
});


builder.Services.AddCompressionServices();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ProjectAdmin", policy =>
        policy.Requirements.Add(new ProjectAdminRequirement()));
});
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailSettings"));


builder.Services.AddMemoryCache();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();
MappingExtensions.Mapper = app.Services.GetRequiredService<IMapper>();


app.Use(async (context, next) =>
{
    try
    {
       
        
        Console.WriteLine($"[DEBUG] Request Body in Debug Middleware: ");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[DEBUG] Error reading body in middleware: {ex.Message}");
    }

    await next();
});


app.UseAuthorization();
#region Custom Middleware
app.UseMiddleware<GlobalErrorHandlerMiddleware>();
app.UseMiddleware<TransactionMiddleware>();
#endregion
app.MapControllers();

app.Run();

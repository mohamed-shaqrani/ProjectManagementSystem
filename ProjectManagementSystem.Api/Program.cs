using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Api.Config;
using ProjectManagementSystem.Api.Data;
using ProjectManagementSystem.Api.Extensions;
using ProjectManagementSystem.Api.Helpers;
using ProjectManagementSystem.Api.MappingProfiles;
using ProjectManagementSystem.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseAuthentication();
app.UseHttpsRedirection();
MappingExtensions.Mapper = app.Services.GetRequiredService<IMapper>();

app.UseAuthorization();
#region Custom Middleware
app.UseMiddleware<GlobalErrorHandlerMiddleware>();
app.UseMiddleware<TransactionMiddleware>();
#endregion
app.MapControllers();

app.Run();

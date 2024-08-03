using AutoHub.Data;

using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AutoHub.BizLogic.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using AutoHub.BizLogic.Abstractions;
using AutoHub.BizLogic;
using AutoHub.Repositories.Abstractions;
using AutoHub.Repositories;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

// Add services to the container.

// TODO: Store these somewhere secure
var dbConnectionString = builder.Configuration["DbConnectionString"];
var secret = builder.Configuration["JwtSecret"]!;
var key = Encoding.UTF8.GetBytes(secret);
/*
var dbConnectionString = "Server=db;Port=5432;Host=localhost;Database=AutoHub;Username=postgres;Password=postgres";
var secret = "J+PrCx6i7qKsFnk28VJ4c2FL0lN+1aA6mRjfzF0sTAo=";
var key = Encoding.UTF8.GetBytes(secret);
*/

// Auth
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters() 
    {
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true
    };
});
builder.Services.AddAuthorization(x =>
{
    x.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
builder.Services.AddSingleton(new AuthService(secret));

// Db Context
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(dbConnectionString));

// Repositories
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleScheduleRepository, VehicleScheduleRepository>();
builder.Services.AddScoped<IScheduledServiceTypeRepository, ScheduledServiceTypeRepository>();

// Biz logic 
builder.Services.AddScoped<IVehicleBizLogic, VehicleBizLogic>();
builder.Services.AddScoped<IScheduledServiceTypeBizLogic, ScheduledServiceTypeBizLogic>();

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new CamelCaseParameterTransformer()));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    x.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

var excludedRoutes = new List<string>
{
    "/api/auth/login"
};
app.UseMiddleware<AuthMiddleware>(excludedRoutes);

app.MapControllers();

app.Run();

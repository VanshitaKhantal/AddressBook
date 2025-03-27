using BusinessLayer.Interface;
using BusinessLayer.Mapping;
using BusinessLayer.Service;
using BusinessLayer.Validators;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using ModelLayer.DTO;
using BusinessLayer.Helper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using RepositoryLayer.Helper;

var builder = WebApplication.CreateBuilder(args);

// Configure redis
var redisConnection = builder.Configuration["Redis:ConnectionString"];

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
    options.InstanceName = "AddressBook_";
});

// Configure RabbitMQ
var rabbitMQConsumer = new RabbitMQConsumer();
Task.Run(() => rabbitMQConsumer.StartConsumer());

// Configure DbContext with SQL Server
builder.Services.AddDbContext<AddressBookContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(AddressBookMappingProfile));

// Add FluentValidation
// Add services to the container.

builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddScoped<IValidator<AddressBookDTO>, AddressBookEntryValidator>();

// Dependency Injection
builder.Services.AddScoped<IAddressBookRL, AddressBookRL>();
builder.Services.AddScoped<IAddressBookService, AddressBookService>();
builder.Services.AddScoped<IUserRL, UserRL>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGenerateToken, GenerateToken>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<RedisCacheService>();

// JWT Authentication Setup
var key = builder.Configuration["Jwt:Key"]; // Ensure you have this in appsettings.json
var issuer = builder.Configuration["Jwt:Issuer"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization(); // Enable Authorization

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication(); // Apply Authentication Middleware

app.UseAuthorization();

app.MapControllers();

app.Run();

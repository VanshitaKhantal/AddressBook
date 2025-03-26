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

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

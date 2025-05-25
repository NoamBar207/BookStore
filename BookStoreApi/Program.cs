using BookStoreApi.Services; 
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configure environment-specific appsettings
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) 
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables(); 


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IBookService>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var xmlPath = config["XmlFilePath"]!;

    if (!File.Exists(xmlPath))
    {
        File.WriteAllText(xmlPath, @"<?xml version=""1.0"" encoding=""UTF-8""?><bookstore></bookstore>");
    }

    return new XmlBookService(xmlPath);
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();


// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers(); 

app.Run();
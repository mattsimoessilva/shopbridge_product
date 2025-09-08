using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductAPI.Data;
using ProductAPI.Repositories;
using ProductAPI.Repositories.Interfaces;
using ProductAPI.Services;
using ProductAPI.Services.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Adding EF Core with SQlite
builder.Services.AddDbContext<ProductAppDbContext>(options =>
    options.UseSqlite("Data Source=productapi.db"));

// Adding services
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ShopBridge Product Service API",
        Version = "v1",
        Description = "An ASP.NET Core Web API for managing products in the ShopBridge system.",
        Contact = new OpenApiContact
        {
            Name = "Matheus Sim?es",
            Email = "matheussimoesdasilva@outlook.com"
        }
    });
});

var app = builder.Build();

// Applying migrations.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProductAppDbContext>();
    db.Database.Migrate();
}

// Configuring the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

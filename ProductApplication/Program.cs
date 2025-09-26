using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductApplication.Data;
using ProductApplication.Models.Profiles;
using ProductApplication.Repositories;
using ProductApplication.Repositories.Interfaces;
using ProductApplication.Services;
using ProductApplication.Services.Interfaces;
using System.Reflection;
using DotNetEnv;

// --- Load .env if it exists ---
Env.Load(); 

var builder = WebApplication.CreateBuilder(args);

// --- Database connection ---
var dbPath = Environment.GetEnvironmentVariable("PRODUCT_DB_PATH")
             ?? "Storage/database.db";
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// --- AutoMapper ---
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ProductProfile>();
    cfg.AddProfile<ProductReviewProfile>();
    cfg.AddProfile<ProductVariantProfile>();
});


// --- Repositories ---
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
builder.Services.AddScoped<IProductReviewRepository, ProductReviewRepository>();

// --- Services ---
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductVariantService, ProductVariantService>();
builder.Services.AddScoped<IProductReviewService, ProductReviewService>();

// --- Controllers & Swagger ---
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
        Title = "ShopBridge - Product Service",
        Version = "v1",
        Description = "A RESTful service for managing products, product variants, and product reviews within the ShopBridge platform. It provides endpoints to create, update, retrieve, and remove products, as well as to manage variants, handle stock availability, and maintain customer reviews for each product.",
        Contact = new OpenApiContact
        {
            Name = "@mattsimoessilva",
            Email = "matheussimoesdasilva@outlook.com"
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});



// --- Build app ---
var app = builder.Build();

// --- Apply migrations & seed database ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.Migrate();
    DatabaseInitializer.Initialize(db);
}

// --- Middleware & Swagger ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShopBridge - Product Service v1");
    });

}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseCors("AllowAll");


app.UseAuthorization();
app.MapControllers();


// --- Run using .env port or default ---
var port = Environment.GetEnvironmentVariable("PRODUCT_SERVICE_PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");
app.Run();

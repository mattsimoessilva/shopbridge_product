# ShopBridge - Product Service

This repository implements the **Product Service** of the ShopBridge system, designed as part of the **MVP project for the Advanced Back-End module of the Full-Stack Development postgraduate program at CCEC - PUC-Rio**. The service provides a fully functional API for managing products, enabling creation, retrieval, update, and deletion of product data in a microservices architecture.

Developed using **ASP.NET Core**, the service follows a layered architecture and adheres to industry best practices for RESTful APIs. It is designed to be integrated seamlessly into the system orchestration layer via Docker Compose.

---

## Repository Structure

```
shopbridge_product/
├── ProductApplication/                      # Main application project
│   ├── Controllers/                         # API controllers handling HTTP requests
│   ├── Data/                                # Database context and configuration
│   ├── Migrations/                          # EF Core database migrations
│   ├── Models/                              # Domain models and DTOs
│   ├── Properties/                          # Project metadata (e.g., launchSettings.json)
│   ├── Repositories/                        # Data persistence and database access
│   ├── Services/                            # Business logic and service layer
│   ├── ProductApplication.csproj            # Project definition
│   └── README.md                            # Project documentation
├── ProductApplication.Tests/               # Unit and integration tests
│   ├── Controllers/                         # Test cases for API controllers
│   ├── Repositories/                        # Test cases for data access layer
│   ├── Services/                            # Test cases for business logic
│   └── ProductApplication.Tests.csproj      # Test project definition
├── README.md                                # Root-level documentation
├── .gitignore                               # Git ignore rules
└── ProductApplication.sln                   # Solution file for the entire project
```

---

## Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/mattsimoessilva/shopbridge_product.git
cd shopbridge_product
```

### 2. Run the service
You can run the service locally via Docker Compose (from the orchestration repository) or directly using `dotnet run`:

```bash
dotnet run --project ProductAPI.csproj
```

The API will be available at **http://localhost:5001**.

---

## API Endpoints

### Products

| Method | Endpoint           | Description                       |
|--------|------------------|-----------------------------------|
| POST   | `/api/Products`     | Creates a new product               |
| GET    | `/api/Products`     | Retrieves all products              |
| GET    | `/api/Products/{id}` | Retrieves a specific product by ID |
| PUT    | `/api/Products`     | Updates an existing product         |
| DELETE | `/api/Products/{id}` | Deletes a product by ID            |

### Product Reviews

| Method | Endpoint           | Description                       |
|--------|------------------|-----------------------------------|
| POST   | `/api/ProductReviews`     | Creates a new product review              |
| GET    | `/api/ProductReviews`     | Retrieves all product reviews             |
| GET    | `/api/ProductReviews/{id}` | Retrieves a specific product review by ID |
| PUT    | `/api/ProductReviews`     | Updates an existing product review         |
| DELETE | `/api/ProductReviews/{id}` | Deletes a product review by ID            |

### Product Variants

| Method | Endpoint           | Description                       |
|--------|------------------|-----------------------------------|
| POST   | `/api/ProductVariants`     | Creates a new product variant              |
| GET    | `/api/ProductVariants`     | Retrieves all product variants             |
| GET    | `/api/ProductVariants/{id}` | Retrieves a specific product variant by ID |
| PUT    | `/api/ProductVariants`     | Updates an existing product variant         |
| DELETE | `/api/ProductVariants/{id}` | Deletes a product variant by ID            |

All endpoints follow REST conventions and return appropriate HTTP status codes (200, 201, 204, 400, 404, 500) with JSON payloads.

---

## Notes

- The service uses **SQLite** for local persistence and supports Docker volumes for data retention.  
- It is designed to operate as part of the **ShopBridge microservices system**, communicating with other services via internal Docker networking.  
- All timestamps are in ISO 8601 format, and UUIDs are used for unique identification of records.

---

## References

[1] S. Newman, *Building Microservices: Designing Fine-Grained Systems*. O’Reilly Media, 2015.  
[2] Microsoft, *ASP.NET Core Documentation*, 2025. Available: https://docs.microsoft.com/aspnet/core  
.
# ShopBridge Product Service API

This repository implements the **Product Service** of the ShopBridge system, designed as part of the **MVP project for the Third Sprint of the Full Stack Development postgraduate program at CCEC - PUC-Rio**. The service provides a fully functional API for managing products, enabling creation, retrieval, update, and deletion of order data in a microservices architecture.

Developed using **ASP.NET Core**, the service follows a layered architecture and adheres to industry best practices for RESTful APIs. It is designed to be integrated seamlessly into the system orchestration layer via Docker Compose.

---

## Repository Structure

```
shopbridge_product/
│
├── Controllers/         # API controllers handling HTTP requests
├── Models/              # Domain models and DTOs
├── Services/            # Business logic and service layer
├── Repositories/        # Data persistence and database access
├── Migrations/          # EF Core database migrations
├── ProductAPI.csproj    # Project definition
└── README.md
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
dotnet run --project OrderAPI.csproj
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
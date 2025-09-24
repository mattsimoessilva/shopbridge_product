# ShopBridge - Product Service

This repository implements the **Product Service** of the ShopBridge system, designed as part of the **MVP project for the Advanced Back-End module of the Full-Stack Development postgraduate program at CCEC - PUC-Rio**. The service provides a fully functional API for managing products, enabling creation, retrieval, update, and deletion of product data in a microservices architecture.

Developed using **ASP.NET Core**, the service follows a layered architecture and adheres to industry best practices for RESTful APIs. It is designed to be integrated seamlessly into the system orchestration layer via Docker Compose.

---

## Repository Structure

```
shopbridge_product/
├── ProductApplication/                      # This is the main application project for the ShopBridge product service.
│   ├── Controllers/                         # Contains ASP.NET Core API controllers that define HTTP endpoints and handle incoming requests.
│                                            # Each controller typically maps to a domain entity and orchestrates calls to the service layer.
│   ├── Data/                                # Includes database context classes, configuration files, and seed data.
│                                            # This is where Entity Framework Core is configured to interact with the underlying database.
│   ├── Migrations/                          # Stores EF Core migration files that track schema changes over time.
│                                            # These files are auto-generated and used to apply or rollback database updates.
│   ├── Models/                              # Contains domain models, DTOs (Data Transfer Objects), and validation logic.
│                                            # These classes define the structure of data used throughout the application and API.
│   ├── Properties/                          # Holds project-level metadata, such as launchSettings.json for local debugging and environment setup.
│   ├── Repositories/                        # Implements the data access layer, typically using interfaces and concrete classes.
│                                            # Responsible for querying, saving, and updating data in the database.
│   ├── Services/                            # Contains business logic and application services that coordinate between controllers and repositories.
│                                            # This layer enforces business rules and encapsulates reusable operations.
│   ├── ProductApplication.csproj            # The C# project file that defines dependencies, build configuration, and project metadata.
│   └── README.md                            # Documentation specific to the ProductApplication project, including setup instructions and usage notes.
├── ProductApplication.Tests/               # This project contains unit and integration tests for the main application.
│   ├── Controllers/                         # Includes test cases for API controllers, validating routing, request handling, and response formatting.
│   ├── Repositories/                        # Contains tests for the data access layer, ensuring correct interaction with the database or mocks.
│   ├── Services/                            # Holds tests for business logic, verifying that service methods behave correctly under various conditions.
│   └── ProductApplication.Tests.csproj      # The C# project file for the test suite, defining test dependencies and configuration.
├── README.md                                # Root-level documentation for the entire repository.
│                                            # This file typically includes an overview of the project, setup instructions, architecture notes, and contribution guidelines.
├── .gitignore                               # Specifies files and folders that should be excluded from version control.
│                                            # Common entries include build artifacts, user-specific settings, and sensitive configuration files.
└── ProductApplication.sln                   # The Visual Studio solution file that groups together multiple projects.
                                             # Useful for managing builds, debugging, and navigation across the application and test projects.
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
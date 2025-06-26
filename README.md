# OnionApiTemplate# 🧱 Clean Architecture .NET Project

This is a modular .NET 8 Web API project following the principles of Clean Architecture. The architecture separates concerns into distinct layers: **Presentation**, **API**, **Application**, **Domain**, and **Infrastructure**.

---

## 📁 Project Structure

```
📦 Project.Root
├── Api/                 --> Entry point, config, middleware, static files
│   ├── Extensions/
│   ├── Factories/
│   ├── Middleware/
│   ├── Models/
│   │   └── Errors/
│   ├── wwwroot/
│   ├── Program.cs
│   └── appsettings.json
│
├── Presentation/        --> HTTP Controllers
│   └── Controllers/
│
├── Application/         --> Application logic and services
│   ├── Dtos/
│   ├── Common/
│   │   └── Settings/     --> Includes JwtSettings
│   ├── Interfaces/
│   ├── Services/
│   ├── MappingProfiles/
│   └── Validations/
│
├── Domain/              --> Core business models and rules
│   ├── Entities/
│   ├── Errors/
│   ├── Exceptions/
│   ├── Interfaces/
│   ├── Settings/
│   └── Specifications/
│
└── Infrastructure/      --> External integrations and implementations
    ├── Persistence/
    └── Services/
```

---

## 🔐 Authentication

JWT Authentication is implemented using a strongly-typed `JwtSettings` class located in:

```
Application/Common/Settings/Jwt.cs
```

JWT tokens are generated and validated in the `Application` or `Infrastructure` layer and configured in `Program.cs`.

---

## ✅ Validation

- **DTO validation** using FluentValidation.
- **Business rules** are handled in the `Domain` layer using entities, exceptions, or specifications.
- Custom validation errors are returned using `ProblemDetails` and `ValidationErrorResponse`.

---

## 🧩 Middleware

Custom middleware (like global exception handler) is registered in the `API` layer under:

```
Api/Middleware/
```

---


## 🚀 Getting Started

1. Clone the repo.
2. Set your connection strings and JWT keys in `appsettings.json`.
3. Run EF Core migrations if needed.
4. Build & run the API.

---

## 📌 Notes

- Following **Separation of Concerns** and **Dependency Inversion**.
- Use of **AutoMapper** for mapping between DTOs and Entities.
- Ready to plug in external services like Stripe, Email, etc.

---

## 🙌 Author

Built by **Mostafa ELshenawy** – using Clean Architecture with love.

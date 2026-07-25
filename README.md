# Gemini GenAI Backend (AIAPISelectWithDB)

A C# Minimal API application targeting .NET 9 that integrates with the Microsoft SQL Server **AdventureWorks2022** database. It exposes clean, RESTful endpoints for querying Person, Address, Contact Type, Country Region, State Province, and Password data. This API serves as the backing database layer for the Gemini API Route Selector.

## Features

- **High-Performance Database Mappings**: Leverages direct SQL querying mapping to clean domain models.
- **Structured Endpoint Groups**: Modular endpoint mapping using ASP.NET Core Minimal APIs.
- **Interactive Documentation**: Built-in interactive API documentation using **Scalar API Reference** and Microsoft's OpenAPI engine.
- **xUnit Integration Tests**: Comprehensive test suites verifying repository behavior and API endpoints.

---

## Prerequisites

- **.NET SDK**: 9.0 or later
- **Database**: Microsoft SQL Server (LocalDB or Local Instance)
- **Sample Database**: [AdventureWorks2022](https://github.com/Microsoft/sql-server-samples/releases/tag/adventureworks) database attached to your local instance.

---

## Getting Started

### 1. Database Connection Configuration

Ensure that your SQL Server instance connection string is set up correctly in `appsettings.json`. By default, it connects to a local server instance and searches for `AdventureWorks2022`:

```json
"ConnectionStrings": {
  "AdventureWorks": "Server=localhost;Database=AdventureWorks2022;Integrated Security=True;TrustServerCertificate=True;"
}
```

If you use a different SQL Server instance (e.g., LocalDB), update the `Server` parameter (e.g. `(localdb)\\MSSQLLocalDB`).

### 2. Run the Application

Navigate to the project directory and run the application:

```bash
dotnet run
```

By default, the application will boot up at `http://localhost:5080`. 

Once running, navigate to `http://localhost:5080/` in your browser. It will automatically redirect you to the **Scalar Interactive API documentation** dashboard (`http://localhost:5080/scalar/v1`) where you can interactively test the endpoints.

---

## API Endpoints Reference

### 👤 Person API
- `GET /api/persons` - Retrieve a list of all persons.
- `GET /api/persons/{id}` - Retrieve basic person info (name, type, demographics).
- `GET /api/persons/{id}/contact` - Retrieve address and email address for a person by ID.

### 📍 Address API
- `GET /api/addresses/{id}` - Retrieve physical address details by ID.

### 📞 Contact Type API
- `GET /api/contacttypes` - Retrieve all contact types.
- `GET /api/contacttypes/{id}` - Retrieve contact type details by ID.

### 🌐 Country Region API
- `GET /api/countryregions` - Retrieve all country regions.
- `GET /api/countryregions/{code}` - Retrieve country region details by country code (e.g., `US`, `FR`).

### 🗺️ State Province API
- `GET /api/stateprovinces` - Retrieve all state provinces.
- `GET /api/stateprovinces/{id}` - Retrieve state/province details by ID.

### 🔑 Password API
- `GET /api/passwords/{id}` - Retrieve password details (hash and salt info) by BusinessEntityID.

---

## Testing

The project includes an **xUnit** test suite containing both unit and integration tests located in the `AIApiSelection.Tests` directory.

To run the test suite:

```bash
dotnet test
```

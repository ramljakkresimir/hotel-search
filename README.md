# Hotel Search API

## Project Overview

Hotel Search API is a RESTful ASP.NET Core Web API developed as a take-home assignment. The application allows users to manage hotels and search for hotels using a natural language prompt.

The search functionality extracts a destination and an optional budget from the user's prompt, resolves the destination to geographic coordinates using the OpenStreetMap Nominatim API, calculates the distance between the searched location and every hotel, and returns ranked results.

The project was intentionally designed following Clean Architecture principles to keep business logic independent from infrastructure concerns and to make the application easy to extend and test.

---

## Architecture

The solution is organized into four projects following Clean Architecture principles:

```
HotelSearch.Api
│
├── Controllers
├── DTOs
├── Validators
│
HotelSearch.Application
│
├── Abstractions
├── Models
├── Services
│
HotelSearch.Domain
│
├── Entities
└── ValueObjects
│
HotelSearch.Infrastructure
│
├── Repositories
└── Services
```

### Responsibilities

* **Api** – HTTP endpoints, request validation and dependency injection.
* **Application** – Business logic, search algorithm and service abstractions.
* **Domain** – Core business entities and business rules.
* **Infrastructure** – Repository implementation, prompt parser and geocoding integration.

---

## Technologies

* .NET 8.0.422
* ASP.NET Core Web API
* C#
* FluentValidation
* Swagger / OpenAPI
* xUnit
* GitHub Actions
* OpenStreetMap Nominatim API

---

## Assumptions

The assignment leaves several implementation details open. The following assumptions were made:

* Hotels are stored using an in-memory repository.
* The search prompt contains a destination and may optionally contain a maximum budget.
* The destination extracted from the prompt is resolved to geographic coordinates using the OpenStreetMap Nominatim API.
* The current implementation uses a lightweight regular expression (regex) based parser to extract the destination and budget from the user's prompt.
* The parser was intentionally designed behind the `IPromptParser` interface so it can later be replaced with a more advanced LLM-based implementation without changing the rest of the application.
* Hotels are ranked using a combined score based on both price and distance.
* Current implementation represents a Proof of Concept and can be extended to use a persistent database and more advanced prompt parsing.

---

## Features

* Create hotel
* Update hotel
* Delete hotel
* Get hotel by id
* Get all hotels
* Hotel search using a natural language prompt
* Budget filtering
* Distance calculation
* Ranking based on price and distance
* Pagination
* Request validation
* Health check endpoint
* Swagger documentation

---

## Running the Project

Restore dependencies:

```bash
dotnet restore
```

Build:

```bash
dotnet build
```

Run the API:

```bash
dotnet run --project src/HotelSearch.Api
```

Swagger UI:

```
https://localhost:<port>/swagger
```

Health check:

```
https://localhost:<port>/health
```

---

## Search Algorithm

The search process consists of the following steps:

1. Parse the user prompt.
2. Extract destination and optional budget.
3. Resolve destination coordinates using OpenStreetMap Nominatim.
4. Calculate the distance between the searched location and every hotel using the Haversine formula.
5. Filter hotels by budget (if specified).
6. Normalize both price and distance.
7. Calculate a ranking score using normalized values.
8. Return paginated results ordered by the calculated score.

---

## Validation

Validation is implemented on two levels:

* API request validation using FluentValidation.
* Domain validation inside the domain model to ensure invalid entities cannot be created even if the API layer is bypassed.

---

## Testing

The solution includes focused unit tests covering the core business logic, including:

* Domain entity validation
* Geo location validation
* Distance calculation (including approximate distance verification)
* Prompt parsing (valid inputs and selected edge cases)
* Hotel search edge cases
* Hotel ranking and pagination

All tests can be executed using:

```bash
dotnet test
```

The test suite is automatically executed by the GitHub Actions CI workflow on every push and pull request.

---

## Continuous Integration

GitHub Actions automatically performs:

* Restore dependencies
* Build the solution
* Run all unit tests

on every push and pull request to the `main` branch.

---

## Future Improvements

* Replace in-memory repository with a relational database.
* Support more advanced natural language prompt parsing.
* Add authentication and authorization.
* Improve hotel ranking using configurable weights.
* Add caching for geocoding requests.
* Add integration tests.
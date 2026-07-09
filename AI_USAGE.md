# AI Usage

## Overview

AI tools were used throughout the development of this assignment as a learning and productivity aid, not as a replacement for understanding the implementation.

Since this is my first project using C# and ASP.NET Core, AI significantly accelerated the learning process by explaining framework concepts, suggesting implementation approaches, and helping identify issues during development.

---

## How AI Was Used

### Learning the .NET Ecosystem

AI was used to understand:

* Clean Architecture
* ASP.NET Core Web API
* Dependency Injection
* Repository Pattern
* FluentValidation
* xUnit
* GitHub Actions
* Swagger/OpenAPI

The focus was on understanding why each component exists and how they work together rather than simply copying code.

---

### Architecture Decisions

AI assisted in discussing different implementation options before coding.

Examples include:

* Choosing a Clean Architecture structure.
* Separating business logic from infrastructure.
* Designing repository abstractions.
* Implementing a prompt parser behind an interface to allow future replacement with more advanced implementations.

The final implementation decisions were reviewed and adjusted during development.

---

### Development Assistance

AI was used to:

* explain unfamiliar C# syntax and .NET concepts;
* suggest implementation ideas;
* help debug compilation and runtime issues;
* explain framework behaviour (e.g. FluentValidation, Dependency Injection, Swagger);
* review and improve the search algorithm;
* identify and fix edge cases.

One example was identifying an edge case where filtering removed all hotels, causing an exception during score calculation. The implementation was updated to correctly return an empty result set instead of an HTTP 500 error.

---

### Documentation

AI assisted in preparing project documentation, including:

* README.md
* AI_USAGE.md

The documentation was reviewed and adapted to accurately describe the final implementation.

---

## Verification

Every generated code sample was:

* reviewed before being added to the project;
* compiled and tested locally;
* modified where necessary to better fit the application design.

AI suggestions were treated as recommendations rather than final solutions.

---

## Reflection

This assignment was my first hands-on experience developing an ASP.NET Core Web API.

Using AI significantly accelerated the learning process, but the primary value came from understanding the reasoning behind the suggested solutions, adapting them to the project, and validating the final implementation through testing and manual review.

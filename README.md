# Event Registration System

This repository contains the source code for an event registration system, designed using domain-driven design principles and layered architecture. The system allows for management of future and past events, and registration and management of participants (both individuals and legal entities).

Login:
Email: administrator@localhost | 
Password: Administrator1!

![Database scheme](https://ibb.co/ssN9Wzd)

## Architecture Overview
The project is structured into four main layers, ensuring a clear separation of concerns and promoting maintainability and scalability:

### Domain Layer
Core: Contains the foundational elements such as entities, value objects, enumerations, and domain logic.

Value Objects: Use of complex types such as AdAccount to handle parsing and validation centrally.

Entities: Core entities include Event, Participant, and potentially User for system interactions.

Exceptions: Custom exceptions like AdAccountInvalidException are used to handle domain-specific errors.

### Application Layer

CQRS: Separates read queries from write commands, simplifying system design and allowing for easy feature expansion.

MediatR: Facilitates the handling of requests and decouples message sending from business logic.

Validation: Uses Fluent Validation for flexible, powerful validation logic.

Common: Shared resources such as logging and validation rules.

### Infrastructure Layer

Persistence: Utilizes Entity Framework Core, with DbContext serving as the Unit of Work and DbSet as Repositories.

External Services: Includes identity services, file system management, and external APIs.

Presentation Layer

Frontend: Can be implemented with any modern framework such as Angular, React, or Vue for SPAs, or traditional approaches like MVC or Razor Pages.

Controllers: Minimalist design, with most logic delegated to the application layer.

## Build

Run `dotnet build -tl` to build the solution.

## Run

To run the web application:

```bash
cd .\src\Web\
dotnet watch run
```

Navigate to https://localhost:5001. The application will automatically reload if you change any of the source files.

## Technologies

ASP.NET Core 8

Entity Framework Core 8

React 

MediatR

AutoMapper
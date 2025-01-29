Project Management System
A modern, scalable, and maintainable Project Management System built using Vertical Slice Architecture and CQRS (Command Query Responsibility Segregation). This application demonstrates clean separation of concerns, modular design, and efficient handling of complex business logic.

Key Features:
Task Management: Create, update, and track tasks with deadlines, priorities, and statuses.

User Management: Role-based access control (RBAC) for administrators, project managers, and team members.

Project Tracking: Monitor project progress, milestones, and team contributions.

Notifications: Real-time updates and email notifications for task assignments and deadlines.

Technical Stack:
Backend: Built with ASP.NET Core for high performance and scalability.

Database: Utilizes Entity Framework Core with SQL Server for robust data management.

CQRS Pattern: Separates read and write operations for improved performance and maintainability.

MediatR: Implements the mediator pattern to decouple components and simplify command/query handling.

FluentValidation: Ensures robust input validation and error handling.

AutoMapper: Simplifies object-to-object mapping for cleaner code.

Swagger/OpenAPI: Provides comprehensive API documentation for easy integration.

Dependency Injection: Leverages built-in DI for modular and testable code.

Unit Testing: Includes xUnit or NUnit tests for reliable and maintainable code.

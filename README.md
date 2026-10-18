# University Campus Management Platform

A web application built with **ASP.NET Core** to support the academic, administrative, and student-related activities of a university campus.

The project is developed collaboratively by multiple student teams. Each team is responsible for a functional module, with all modules integrated into a single application.

## Features

The application is organized into nine functional modules.

### 1. Academic Management

Manage the university's academic structure and student enrolment.

- Manage academic programs and degrees.
- Manage study plans and curricula.
- Manage course units.
- Manage academic years and semesters.
- Manage student enrolment in academic programs and course units.
- Register new students and manage their initial enrolment (matriculation).
- Create, update, view, and delete academic calendar entries.
- Manage important academic dates, events, and deadlines.

**Note:** Examination registration is handled by Module 3, Assessments and Assignments.

### 2. Timetables and Room Management

Manage class schedules and the use of campus facilities.

- Manage campus buildings and rooms.
- Define room capacities and available equipment.
- Create and manage class timetables.
- Manage room reservations and availability.
- Track room occupancy.
- Detect timetable and room booking conflicts.

### 3. Assessments and Assignments

Manage assessments, examinations, grades, and student assignments.

- Create and manage assessments and examinations.
- Manage student examination registrations.
- Record grades and academic results.
- Manage assignments and group projects.
- Create and manage student work groups.
- Handle assignment submissions and deadlines.
- Provide feedback on submitted work.
- Track assessment results and examination outcomes.

### 4. Library Management

Manage the university library and its resources.

- Manage books, authors, categories, and library copies.
- Search and browse the library catalogue.
- Track the availability and location of library items.
- Manage loans, returns, and renewals.
- Handle reservations.
- Track overdue items and applicable fines.

### 5. Accommodation Management

Manage student accommodation and university residences.

- Manage residences, rooms, and beds.
- Handle accommodation applications.
- Allocate rooms to students.
- Manage accommodation contracts.
- Track payments.
- Manage check-ins and check-outs.
- Track room availability and occupancy.

### 6. Cafeteria and Food Services

Manage campus dining facilities, meals, and associated personnel.

- Manage cafeterias and dining facilities.
- Create and manage menus.
- Manage meals, prices, and availability.
- Handle meal reservations.
- Track payments and meal consumption.
- Provide information about ingredients and allergens.
- Create, view, update, and delete lecturer records.
- Create, view, update, and delete university staff records.
- Optionally manage guest records.

### 7. Scholarship Management

Manage scholarship programs and student applications.

- Manage scholarship programs and application periods.
- Handle scholarship applications.
- Manage supporting documents.
- Define eligibility requirements and evaluation criteria.
- Evaluate applications and rank candidates.
- Approve or reject applications.
- Track scholarship awards and payments.
- Maintain application and award histories.

### 8. Alumni, Internships, and Career Management

Manage relationships with former students and support students and graduates in their transition to the labour market.

- Maintain alumni records and academic histories.
- Define and manage alumni profiles.
- Manage employers and company contacts.
- Publish job and internship opportunities.
- Handle applications for job and internship opportunities.
- Track recruitment and application status.
- Record employment information and career progression.
- Support internship follow-up.
- Maintain relationships between the university, alumni, and employers.
- Collect employment and career information through surveys.
- Generate employment and graduate outcome reports.

**Alumni definition:** Alumni are former students who have completed an academic program at the university. The module maintains their relationship with the institution after graduation, including their academic background, employment status, and professional development.

### 9. Requests and Administrative Services

Manage administrative requests and support services across the university.

- Submit and manage administrative requests.
- Handle requests for certificates and official documents.
- Manage student support requests.
- Handle maintenance requests.
- Track request status and processing history.
- Assign requests to the appropriate department or staff member.
- Support comments and document attachments.
- Maintain a history of actions taken on each request.

## Technology Stack

- **Backend:** ASP.NET Core
- **Language:** C#
- **Frontend:** HTML, CSS, JavaScript, and Bootstrap
- **Database:** Microsoft SQL Server
- **Object-Relational Mapper (ORM):** Entity Framework Core
- **Data access:** Entity Framework Core and LINQ
- **Database schema management:** Entity Framework Core Migrations
- **Version control:** Git
- **Collaboration:** GitHub

## Shared Application Infrastructure

The following concerns are shared across the application and are not assigned to a single functional module:

- Authentication and authorization.
- User accounts, roles, and permissions.
- Shared application layout and navigation.
- Input validation and error handling.
- Logging and auditing.
- Notifications.
- File uploads and document storage.
- Common API conventions and integration standards.

Shared infrastructure should be implemented consistently so that all modules can use it without duplicating functionality.

## Architecture and Integration

Each module is responsible for a specific functional area of university management. Modules should have clearly defined responsibilities, data models, and interfaces.

Examples of cross-module dependencies include:

- **Academic Management** provides academic programs, course units, and student enrolment information used by Assessments and Assignments.
- **Timetables and Room Management** uses academic information to schedule classes and allocate rooms.
- **Assessments and Assignments** uses student and course unit information to manage examinations, examination registrations, grades, and assignments.
- **Library Management** uses student information to manage loans and reservations.
- **Accommodation Management** uses student information to process applications and room allocations.
- **Scholarship Management** uses student and academic information to evaluate applications.
- **Alumni, Internships, and Career Management** uses academic records to associate graduates with their completed programs.
- **Requests and Administrative Services** provides workflows for administrative and support requests.

Dependencies should be explicit and agreed upon before implementation. Each module should remain independently testable wherever possible.

## Collaborative Development

Each team is responsible for:

- Designing and implementing its assigned module.
- Defining its data model and business rules.
- Implementing the required user interface and application logic.
- Writing automated tests.
- Documenting relevant endpoints and integration requirements.
- Reviewing code and resolving integration issues.
- Keeping its implementation compatible with the shared application.

### Development Guidelines

1. Follow the agreed project architecture and coding conventions.
2. Avoid duplicating functionality already provided by shared components or other modules.
3. Agree on shared entities and interfaces before implementing cross-module dependencies.
4. Use feature branches and pull requests for changes.
5. Keep commits focused and descriptive.
6. Include tests for important business rules.
7. Update documentation whenever functionality or integration contracts change.
8. Ensure that the application builds and runs after integration.
9. Review and test changes before merging them into the main branch.

### Pull Request Guidelines

- Use pull requests to propose changes to the main branch.
- **Each pull request must contain a maximum of three files, including all source code, tests, documentation, configuration files, and any other file types. No exceptions are allowed.**
- **Pull requests must not include Entity Framework Core migration files.** Database migrations must be managed separately according to the project's agreed migration workflow.
- If a change requires more than three files, split it into smaller, logically coherent pull requests.
- Each pull request should have a clear purpose and a descriptive title.
- Keep changes focused and avoid unrelated modifications.
- Ensure that the application builds and relevant tests pass before requesting a review.
- Review changes carefully before merging them into the main branch.

## Database and Data Access

The application uses **Microsoft SQL Server** as its relational database and **Entity Framework Core (EF Core)** for data access.

- Use entity classes to represent database entities.
- Use `DbContext` and `DbSet<T>` to interact with the database.
- Use LINQ for database queries.
- Use EF Core Migrations to manage database schema changes.
- Define relationships and constraints explicitly.
- Apply appropriate validation and business rules.
- Avoid duplicating entity definitions across modules.
- Coordinate changes to shared entities and relationships across teams.
- Test database changes before merging them into the main branch.

The database connection is already configured in the project, so no additional connection-string setup is required.

### Database Migration Guidelines

Since multiple teams contribute to the same application, database changes must be coordinated.

- Agree on entity names, relationships, and ownership before implementation.
- Avoid creating duplicate entities for the same university concept.
- Coordinate changes to shared entities and database constraints.
- **Do not include Entity Framework Core migration files in pull requests.**
- Follow the project's agreed process for creating, reviewing, sharing, and applying migrations separately from application code changes.
- Test database schema changes against a development database.
- Do not apply migrations independently to a shared production database during development.

The team must establish a single, agreed-upon strategy for managing the shared database schema and EF Core migrations.

## Getting Started

### Prerequisites

- The .NET SDK version targeted by the project.
- Access to the configured SQL Server instance.
- Git.
- An IDE or code editor, such as Visual Studio or Visual Studio Code.

### Clone the Repository

```bash
git clone <repository-url>
cd <repository-directory>
```

### Restore Dependencies

```bash
dotnet restore
```

### Apply Database Migrations

If the project uses Entity Framework Core Migrations and pending migrations need to be applied, run:

```bash
dotnet ef database update
```

If the solution separates the data-access project from the startup project, specify the appropriate project paths.

**Note:** Migration files must not be included in pull requests. Applying migrations to a shared database must follow the team's agreed migration workflow.

### Run the Application

```bash
dotnet run
```

Open the URL displayed in the terminal to access the application.

## Testing

Run the automated test suite with:

```bash
dotnet test
```

Tests should cover business rules, input validation, data access, and integration between modules where appropriate.
#  Enterprise Employee Management System (EMS)

A full-stack **Enterprise Employee Management System** designed to manage employees, departments, leave requests, payroll, authentication, and real-time communication.

The project is built with **ASP.NET Core Web API** and **PostgreSQL** on the backend, with a **React** frontend. It follows a layered architecture to keep business logic, data access, and API responsibilities separated and maintainable.

---

##  Features

###  Authentication & Authorization

* User registration and login
* JWT-based authentication
* Role-based authorization
* Support for multiple roles such as:

  * Admin
  * HR
  * Employee
  * Manager
* Secure password hashing using BCrypt
* JWT claims for user identity and role-based access

###  Employee Management

* Create and manage employee records
* Associate employees with departments
* Retrieve employee information
* Access employee data through authenticated users
* Employee search functionality
* Pagination
* Sorting by different fields
* Filtering

###  Department Management

* Create and manage departments
* Associate employees with departments
* Retrieve department information
* Department-based employee management

###  Leave Management

* Employees can submit leave requests
* Employees can view their leave history
* HR can view pending leave requests
* HR can approve or reject leave requests
* Leave status tracking
* Review timestamps and reviewer information

###  Payroll Management

* Employee payroll records
* Base salary
* Overtime
* Bonus
* Gross salary
* Deductions
* Net salary
* Payroll period
* Payroll processing status
* Employee-specific payroll access

###  Real-Time Chat

* Real-time messaging using **SignalR**
* Conversation management
* Conversation participants
* Join conversations through SignalR
* JWT authentication for SignalR connections
* Server-side participant validation
* Real-time message delivery

###  API Query Features

Employee APIs support:

* Pagination
* Searching
* Sorting
* Filtering
* Efficient database querying with `IQueryable`
* `AsNoTracking()` for read-only queries

---

##  Tech Stack

### Backend

* **C#**
* **ASP.NET Core Web API**
* **Entity Framework Core**
* **LINQ**
* **PostgreSQL**
* **JWT Authentication**
* **SignalR**
* **AutoMapper**
* **FluentValidation**
* **BCrypt.Net**

### Frontend

* **React**
* REST API integration

### Development Tools

* Git & GitHub
* Postman
* Visual Studio Code
* .NET CLI
* Entity Framework Core Migrations

---

##  Architecture

The backend follows a layered architecture:

```text
                    ┌─────────────────┐
                    │     React       │
                    │    Frontend     │
                    └────────┬────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │   Controllers   │
                    └────────┬────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │    Services     │
                    │ Business Logic  │
                    └────────┬────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │  Repositories   │
                    │  Data Access    │
                    └────────┬────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │ Entity Framework│
                    │      Core       │
                    └────────┬────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │   PostgreSQL    │
                    └─────────────────┘
```

For real-time communication:

```text
React / SignalR Client
          │
          ▼
      ChatHub
          │
          ▼
    Chat Service
          │
          ▼
     Repository
          │
          ▼
      PostgreSQL
```

---

##  Project Structure

```text
Backend/
│
├── Controllers/
│   ├── AuthController.cs
│   ├── EmployeeController.cs
│   ├── DepartmentController.cs
│   ├── LeaveController.cs
│   ├── PayrollController.cs
│   └── ...
│
├── Services/
│   ├── AuthService.cs
│   ├── EmployeeService.cs
│   ├── HrService.cs
│   ├── LeaveService.cs
│   ├── PayrollService.cs
│   ├── ChatService.cs
│   └── ...
│
├── Repository/
│   ├── EmployeeRepository.cs
│   ├── UserRepository.cs
│   ├── LeaveRepository.cs
│   ├── ChatRepository.cs
│   └── ...
│
├── Models/
│   ├── User.cs
│   ├── Employee.cs
│   ├── Department.cs
│   ├── Leave.cs
│   ├── Payroll.cs
│   ├── Conversation.cs
│   └── Message.cs
│
├── DTOs/
│   ├── Auth/
│   ├── Employee/
│   ├── Leave/
│   ├── Payroll/
│   └── Chat/
│
├── Hubs/
│   └── ChatHub.cs
│
├── Validators/
│   └── ...
│
├── Data/
│   └── AppDbContext.cs
│
├── Migrations/
│
└── Program.cs
```

---

##  Authentication Flow

The application uses JWT authentication.

```text
User
 │
 ▼
Login
 │
 ▼
AuthController
 │
 ▼
AuthService
 │
 ▼
Validate Credentials
 │
 ▼
Generate JWT
 │
 ▼
Client
 │
 ▼
Authorization Header
 │
 ▼
Protected API
```

JWT claims contain information such as:

```text
UserId
Name
Email
Role
```

These claims are used to identify the authenticated user and enforce role-based access.

---

##  Database

The system uses **PostgreSQL** with **Entity Framework Core**.

Main entities include:

```text
User
  │
  │ 1:1
  ▼
Employee
  │
  ├──────────► Department
  │
  ├──────────► Leave
  │
  └──────────► Payroll

Conversation
  │
  ├──────────► Participants
  │
  └──────────► Messages
```

Entity Framework Core migrations are used to manage database schema changes.

---

##  LINQ & EF Core

The application uses LINQ extensively for database querying.

Examples of implemented functionality include:

* Filtering
* Searching
* Sorting
* Pagination
* Projection into DTOs
* Navigation property loading
* Read-only queries using `AsNoTracking()`
* `IQueryable` for deferred database execution

Example:

```csharp
_context.Employees
    .AsNoTracking()
    .Include(e => e.Department)
    .Where(e => e.IsActive)
    .OrderBy(e => e.Name);
```

---

##  SignalR Communication

The chat system uses **ASP.NET Core SignalR** for real-time communication.

Clients establish a connection with the SignalR hub:

```text
Client
   │
   │ JWT
   ▼
ChatHub
   │
   ├── Authenticate User
   │
   ├── Validate Conversation
   │
   ├── Validate Participant
   │
   └── Send Message
          │
          ▼
      Other Participants
```

Only authorized participants can join conversations and receive messages.

---

##  API Testing

The backend APIs can be tested using **Postman**.

Authentication:

```http
POST /api/Auth/register
POST /api/Auth/login
```

Employee management:

```http
GET    /api/Employees
GET    /api/Employees/{id}
POST   /api/Employees
PUT    /api/Employees/{id}
DELETE /api/Employees/{id}
```

Leave management:

```http
POST /api/Leave
GET  /api/Leave/my-history
GET  /api/Leave/pending
PUT  /api/Leave/{id}/approve
PUT  /api/Leave/{id}/reject
```

> Endpoint names may change as the project continues to evolve.

---

##  Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/your-username/employee-management-system.git
cd employee-management-system
```

### 2. Configure PostgreSQL

Create a PostgreSQL database and update the connection string in:

```text
appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=EMS;Username=postgres;Password=your_password"
  }
}
```

### 3. Configure JWT

Add your JWT configuration:

```json
{
  "Jwt": {
    "Key": "your-secret-key",
    "Issuer": "EMS",
    "Audience": "EMS"
  }
}
```

Do **not** commit real secrets, passwords, or production JWT keys to GitHub.

### 4. Apply migrations

From the backend directory:

```bash
dotnet ef database update
```

### 5. Run the backend

```bash
dotnet run
```

The API will start on the configured HTTP/HTTPS port.

### 6. Run the frontend

From the frontend directory:

```bash
npm install
npm run dev
```

---

##  Security

The project implements several security practices:

* JWT authentication
* Role-based authorization
* BCrypt password hashing
* Authenticated SignalR connections
* Participant authorization for conversations
* DTOs to control API responses
* Validation using FluentValidation
* Protected API endpoints

---

##  Key Learning Outcomes

This project was built to gain practical experience with enterprise-level .NET backend development, including:

* ASP.NET Core Web API
* RESTful API design
* Entity Framework Core
* LINQ
* PostgreSQL
* Repository and Service patterns
* DTO-based API design
* Dependency Injection
* JWT authentication
* Role-based authorization
* FluentValidation
* AutoMapper
* Database relationships
* EF Core migrations
* Pagination, searching and sorting
* SignalR and real-time communication
* Full-stack React + .NET integration

---

##  Future Improvements

Planned improvements include:

* Advanced HR dashboards
* Attendance management
* Notifications
* Email notifications
* Advanced payroll processing
* File/document management
* Audit logging
* Redis caching
* Automated testing
* Docker containerization
* CI/CD pipeline
* Improved real-time notifications

---

##  Project Purpose

This project is being developed as a practical **enterprise-level .NET application** to demonstrate backend development, database design, authentication, authorization, API architecture, and real-time communication using modern technologies.



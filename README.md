# Academy Management API

An ASP.NET Core Web API for managing academy courses, batches, students, and enrollments. This project extends an academic lab task with authentication, role-based permissions, and enrollment management.

## Technologies

- C# and ASP.NET Core 8
- Entity Framework Core — Database First
- Microsoft SQL Server
- AutoMapper
- Cookie Authentication and Claims
- Swagger / OpenAPI
- Postman for manual API testing

## Features

- Login and logout with hashed password verification
- Role-based authorization for Admin and Manager
- Course and batch management
- Student registration and profile updates
- Duplicate student email prevention
- Student enrollment in batches accepting registrations
- Duplicate active enrollment prevention
- Enrollment cancellation with history preserved
- Deletion restrictions for records with related data

## Database Entities

| Entity | Purpose |
|---|---|
| Course | Stores course details |
| Batch | Stores course batches, start dates, tuition fees, and statuses |
| Student | Stores student details |
| Enrollment | Connects students to batches and records enrollment history |
| UserInfo | Stores login accounts, password hashes, and roles |

### Relationships

- One course can have multiple batches.
- One student can have multiple enrollments.
- One batch can have multiple enrollments.
- Each enrollment connects one student to one batch.

Student records are separate from login accounts.

## Role Permissions

| Operation | Admin | Manager |
|---|---|---|
| View courses and batches | Yes | Yes |
| Create, update, and delete courses and batches | Yes | No |
| View, create, and update students | Yes | Yes |
| Delete students | Yes | No |
| View, create, and cancel enrollments | Yes | Yes |

## Business Rules

- Batch statuses are `enrolling`, `active`, and `concluded`.
- Only batches with status `enrolling` accept new enrollments.
- Enrollment statuses are `Active` and `Cancelled`.
- A student cannot have duplicate active enrollments in the same batch.
- A student can enroll again after cancellation if the batch is still accepting enrollments.
- Cancelled enrollments remain in the database as history.
- Student email addresses must be unique.
- Students and batches with enrollment history cannot be deleted.
- Courses with existing batches cannot be deleted.

## Prerequisites

- .NET 8 SDK
- SQL Server or SQL Server Express
- SQL Server Management Studio
- Visual Studio 2022 with the ASP.NET and web development workload

## Setup Instructions

### 1. Clone the repository

```bash
git clone https://github.com/amrinshahwar/academy-management-api.git
cd academy-management-api
```

Open the included `.sln` file in Visual Studio.

The solution or physical folders may retain the original academic project name, `FE53045Api`.

### 2. Set up the database

1. Open `Database/AcademyDb.sql` in SQL Server Management Studio.
2. Review any generated database file paths and compatibility settings.
3. Adjust them for your SQL Server installation if necessary.
4. Execute the script to create the database and its objects.

The script contains the database schema only. Existing user and student records are not included.

### 3. Configure the connection string

Update the `AcademyDbContext` connection string in the API project's `appsettings.Development.json`, preserving any other settings:

```json
{
  "ConnectionStrings": {
    "AcademyDbContext": "Server=YOUR_SERVER;Database=AcademyDb;Integrated Security=True;TrustServerCertificate=True"
  }
}
```

Replace `YOUR_SERVER` with your SQL Server instance.

For a named instance, escape the backslash in JSON:

```text
localhost\\SQLEXPRESS
```

### 4. Prepare a login account

Automatic account creation and public registration are not currently included.

Before testing protected endpoints, a `UserInfo` account must be created with:

- A unique username
- A full name
- A password hash generated using ASP.NET Core's `PasswordHasher<UserInfo>`
- A role of `Admin` or `Manager`

The `Password` column must contain a password hash, not a plain-text password.

The following C# code illustrates password hashing only; account insertion must be performed separately through a local setup utility or database administration workflow:

```csharp
var hasher = new PasswordHasher<UserInfo>();
user.Password = hasher.HashPassword(user, plainPassword);
```

At login, submit the original password rather than the stored hash. Do not commit account credentials.

### 5. Run the application

1. Restore NuGet packages.
2. Build the solution.
3. Set the API project as the startup project.
4. Run using the HTTPS launch profile in Development.
5. Open `/swagger` at the application's running address.

The address is available in `Properties/launchSettings.json` or in the application console.

## Authentication

Send a request to:

```http
POST /api/Account/login
```

Example request:

```json
{
  "userName": "YOUR_USERNAME",
  "password": "YOUR_PASSWORD"
}
```

Successful login creates an authentication cookie.

Use the same hostname and protocol for subsequent requests so the client retains the cookie.

- `401 Unauthorized`: Authentication is required or login credentials are invalid.
- `403 Forbidden`: The authenticated user does not have the required role.

To log out:

```http
POST /api/Account/logout
```

## API Endpoints

### Account

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/Account/login` | Log in |
| POST | `/api/Account/logout` | Log out |

### Course

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/Course` | List courses |
| GET | `/api/Course/{id}` | Retrieve a course |
| POST | `/api/Course` | Create a course |
| PUT | `/api/Course/{id}` | Update a course |
| DELETE | `/api/Course/{id}` | Delete a course |

### Batch

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/Batch` | List batches |
| GET | `/api/Batch/{id}` | Retrieve a batch |
| POST | `/api/Batch` | Create a batch |
| PUT | `/api/Batch/{id}` | Update a batch |
| DELETE | `/api/Batch/{id}` | Delete a batch |

### Student

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/Student` | List students |
| GET | `/api/Student/{id}` | Retrieve a student |
| POST | `/api/Student` | Create a student |
| PUT | `/api/Student/{id}` | Update a student |
| DELETE | `/api/Student/{id}` | Delete a student |

### Enrollment

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/Enrollment` | List enrollments |
| GET | `/api/Enrollment/{id}` | Retrieve an enrollment |
| POST | `/api/Enrollment` | Enroll a student |
| PATCH | `/api/Enrollment/{id}/cancel` | Cancel an enrollment |

## Example Requests

### Create a student

```http
POST /api/Student
```

```json
{
  "fullName": "Sample Student",
  "email": "student@example.com",
  "phone": "01700000000"
}
```

### Enroll a student

```http
POST /api/Enrollment
```

```json
{
  "studentId": 1,
  "batchId": 1
}
```

Use existing student and batch IDs. The batch must have status `enrolling`.

### Cancel an enrollment

```http
PATCH /api/Enrollment/1/cancel
```

No request body is required.

## Testing

Manual API testing was performed using Postman.

Tested scenarios include:

- Authentication and role permissions
- Student creation, retrieval, and updates
- Invalid email rejection
- Duplicate email rejection
- Successful enrollment
- Duplicate active enrollment rejection
- Invalid student and batch references
- Enrollment rejection for batches not accepting registrations
- Enrollment cancellation
- Re-enrollment after cancellation
- Deletion restrictions for related records

Swagger can also be used for interactive API testing.

Automated tests and an exported Postman collection are not currently included.

## Future Improvements

- Automated unit and integration tests
- Initial account setup
- Student search and pagination
- Student counts per batch
- Batch enrollment reports
- Batch capacity limits
- Improved handling of concurrent database conflicts

## Author

**Amrin Shahwar**

BSc in Computer Science and Engineering  
American International University–Bangladesh (AIUB)

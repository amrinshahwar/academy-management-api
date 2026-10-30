Academy Management API
An ASP.NET Core Web API for managing courses, batches, students, and enrollments. Developed by Amrin Shahwar by extending an academic lab task.
Tech Stack
- C# / ASP.NET Core 8
- Entity Framework Core (database-first)
- Microsoft SQL Server
- AutoMapper
- Cookie authentication with claims and role-based authorization
- Swagger / OpenAPI
Features
- Login and logout with hashed password verification
- Admin and Manager access permissions
- Course, batch, and student CRUD operations
- Student enrollment in batches accepting registrations
- Duplicate active enrollment prevention
- Enrollment cancellation with history retained
- Duplicate student email validation
- Protection against deleting courses with batches, or students and batches with enrollment history
Database Structure
Entity	Purpose
Course	Course title, duration, and related batches
Batch	Course reference, start date, tuition fee, and status
Student	Student name, unique email, and optional phone
Enrollment	Student-to-batch relationship, date, and status
UserInfo	Login username, full name, password hash, and role


One course has many batches. Students and batches are connected through enrollments.
Prerequisites
- .NET 8 SDK
- SQL Server or SQL Server Express
- SQL Server Management Studio (SSMS)
- Visual Studio 2022 with ASP.NET development support
Getting Started
1. Clone the repository
git clone https://github.com/amrinshahwar/academy-management-api.git
cd academy-management-api
Open the included .sln file in Visual Studio. The solution or physical folders may retain the original lab name, FE53045Api.
2. Create the database
Open Database/AcademyDb.sql in SSMS and execute it on your development SQL Server instance. Review any generated physical database file paths and compatibility settings before execution and adjust them for your installation.

The script contains schema only, without existing user or student records. Database migrations are not used; entity classes were generated from SQL Server through EF Core scaffolding.
3. Configure the connection string
Set the following connection string in the API project's appsettings.Development.json, preserving its other settings:
{
  "ConnectionStrings": {
    "AcademyDbContext": "Server=YOUR_SERVER;Database=AcademyDb;Integrated Security=True;TrustServerCertificate=True"
  }
}
Replace YOUR_SERVER with your SQL Server instance. For a named instance in JSON, escape the backslash, for example localhost\\SQLEXPRESS.
4. Prepare a login account
Automatic account creation and public registration are not included. Before testing protected endpoints, provision a record in UserInfo with a unique username, full name, role (Admin or Manager), and a password hash generated using ASP.NET Core's PasswordHasher<UserInfo>.

The password hash must be generated in C# using the intended plain-text password:
var hasher = new PasswordHasher<UserInfo>();
user.Password = hasher.HashPassword(user, plainPassword);
This snippet illustrates hashing only; it does not insert an account. Save the provisioned user through a local setup utility or database administration workflow. Do not store a plain-text password in the database or commit credentials. At login, submit the original password, not the hash.
5. Run the API
1. Restore NuGet packages and build the solution.
2. Select the API project as the startup project.
3. Run the HTTPS launch profile in Development.
4. Open /swagger at the address shown in the console or Properties/launchSettings.json.
5. Log in through POST /api/Account/login before accessing protected endpoints.
Use the same hostname and protocol for all requests so the client retains the authentication cookie.
Permissions
Operation	Admin	Manager
View courses and batches	Yes	Yes
Create, update, or delete courses and batches	Yes	No
View, create, or update students	Yes	Yes
Delete students	Yes	No
View, create, or cancel enrollments	Yes	Yes


API Endpoints
Method	Endpoint	Purpose
POST	/api/Account/login	Log in
POST	/api/Account/logout	Log out
GET	/api/Course	List courses
GET	/api/Course/{id}	Get a course
POST	/api/Course	Create a course
PUT	/api/Course/{id}	Update a course
DELETE	/api/Course/{id}	Delete a course
GET	/api/Batch	List batches
GET	/api/Batch/{id}	Get a batch
POST	/api/Batch	Create a batch
PUT	/api/Batch/{id}	Update a batch
DELETE	/api/Batch/{id}	Delete a batch
GET	/api/Student	List students
GET	/api/Student/{id}	Get a student
POST	/api/Student	Create a student
PUT	/api/Student/{id}	Update a student
DELETE	/api/Student/{id}	Delete a student
GET	/api/Enrollment	List enrollments
GET	/api/Enrollment/{id}	Get an enrollment
POST	/api/Enrollment	Enroll a student
PATCH	/api/Enrollment/{id}/cancel	Cancel an enrollment


Example: Create a student
{
  "fullName": "Sample Student",
  "email": "student@example.com",
  "phone": "01700000000"
}
Example: Enroll a student
Use existing student and batch IDs:
{
  "studentId": 1,
  "batchId": 1
}
Business Rules
- Batch statuses are enrolling, active, and concluded.
- Only a batch with status enrolling accepts new enrollments.
- Enrollment statuses are Active and Cancelled.
- A student cannot have more than one active enrollment in the same batch.
- A student may enroll again after cancellation if the batch is still accepting enrollments.
- Cancellation preserves the enrollment record.
- Student email addresses must be unique.
- Related history prevents deletion of students and batches; existing batches prevent course deletion.
Testing
Manual API testing was performed using Postman, including:
- Login/logout and Admin/Manager permissions
- Student creation, retrieval, updates, and validation
- Duplicate email rejection
- Enrollment creation and duplicate rejection
- Invalid student/batch references
- Enrollment rejection for batches not accepting registrations
- Cancellation and enrollment after cancellation
- Deletion restrictions for related records
Swagger is available for interactive testing. A Postman collection and automated tests are not currently included.
Current Limitations and Future Improvements
- Initial accounts require manual provisioning with hashed passwords.
- Some concurrent database conflicts may return a server error rather than a friendly conflict response.
- Future improvements: automated tests, student search, pagination, batch student counts, and enrollment reports.
Author
Amrin Shahwar
BSc in Computer Science and Engineering, American International University–Bangladesh (AIUB)

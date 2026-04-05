# 🏋️ Gym Management API

A complete ASP.NET Core 10 REST API for managing gym classes with full CRUD operations.

## ⚡ Quick Start

### 1. Prerequisites
- .NET 10 SDK installed
- SQL Server installed and running
- Visual Studio 2022+ (or VS Code)

### 2. Setup Database Connection
Edit `appsettings.json` and update your SQL Server connection string:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=GymManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Replace `YOUR_SERVER` with:
- `.` (local default instance)
- `localhost`
- Your server name
- `(LocalDB)\MSSQLLocalDB` (LocalDB)

### 3. Apply Database Migrations
Open terminal in project directory:
```powershell
dotnet ef database update
```

This creates the `GymClasses` table automatically.

### 4. Run the Application
```powershell
dotnet run
```

The app will start on `https://localhost:7130`

### 5. Test the API
- **Swagger UI**: https://localhost:7130/swagger
- **OpenAPI**: https://localhost:7130/openapi/v1.json

---

## 📚 API Endpoints (CRUD Operations)

### **CREATE** - Add a new gym class
```http
POST /api/gymclasses
```
**Request Body:**
```json
{
  "name": "Morning Yoga",
  "description": "Relaxing yoga session",
  "instructorName": "John Doe",
  "maxCapacity": 20,
  "schedule": "Monday 9:00 AM",
  "difficultyLevel": "Beginner",
  "durationMinutes": 60,
  "price": 15.99
}
```
**Response:** 201 Created with new class data

---

### **READ** - Get classes
```http
GET /api/gymclasses
```
Returns all active gym classes

```http
GET /api/gymclasses/{id}
```
Returns specific class by ID

```http
GET /api/gymclasses/available/list
```
Returns only classes with available spots

---

### **UPDATE** - Modify a gym class
```http
PUT /api/gymclasses/{id}
```
**Request Body:** Same as CREATE (all fields)

**Response:** 200 OK with updated class

---

### **DELETE** - Remove a gym class
```http
DELETE /api/gymclasses/{id}
```
**Response:** 204 No Content

---

## 🏗️ Project Structure

```
backend-gym-webapp/
├── Models/
│   ├── GymClass.cs              (Database entity)
│   └── CreateGymClassDto.cs      (API input validator)
│
├── Data/
│   ├── GymDbContext.cs           (Database connection)
│   ├── GymDbContextFactory.cs    (Context factory)
│   └── Migrations/               (Database schemas)
│
├── Services/
│   └── GymClassService.cs        (Business logic & CRUD operations)
│
├── Controllers/
│   └── GymClassesController.cs   (API endpoints)
│
├── Program.cs                    (App configuration)
├── appsettings.json              (Settings & connection string)
└── README.md                     (This file)
```

---

## 🔄 How CRUD Works

### 1. **CREATE** (Add new class)
- Client sends JSON data to `POST /api/gymclasses`
- Controller receives and validates data via `CreateGymClassDto`
- Service maps DTO to `GymClass` entity
- Service sets auto-fields: `id`, `enrolledCount=0`, `createdAt`, `updatedAt`
- Entity Framework saves to database
- Returns created class with ID

### 2. **READ** (Get classes)
- Client requests `GET /api/gymclasses` or `GET /api/gymclasses/{id}`
- Service queries database
- Entity Framework retrieves records
- Returns JSON response

### 3. **UPDATE** (Modify class)
- Client sends JSON to `PUT /api/gymclasses/{id}`
- Service finds existing record by ID
- Updates fields from request
- Saves changes to database
- Returns updated record

### 4. **DELETE** (Remove class)
- Client requests `DELETE /api/gymclasses/{id}`
- Service finds and removes record
- Entity Framework deletes from database
- Returns success response

---

## 🗄️ Database Schema

The `GymClasses` table has these columns:
| Column | Type | Notes |
|--------|------|-------|
| `Id` | int | Primary key, auto-generated |
| `Name` | string(100) | Required, class name |
| `Description` | string(500) | Optional, class details |
| `InstructorName` | string | Required, teacher name |
| `MaxCapacity` | int | Required, max participants |
| `EnrolledCount` | int | Current participants (starts at 0) |
| `Schedule` | string | Class time/day |
| `DifficultyLevel` | string | Beginner/Intermediate/Advanced |
| `DurationMinutes` | int | Class length in minutes |
| `Price` | decimal | Cost per class |
| `CreatedAt` | datetime | Auto-set when created |
| `UpdatedAt` | datetime | Auto-updated when changed |
| `IsActive` | bool | Active status (default: true) |

---

## 💡 Key Concepts for Beginners

### Models
- **GymClass**: Represents a gym class in the database
- **CreateGymClassDto**: Validates user input (prevents data errors)

### Services
- **GymClassService**: Contains all business logic
- Handles validation, error checking
- Communicates with database

### Controllers
- **GymClassesController**: Handles HTTP requests
- Routes requests to services
- Returns JSON responses

### Database Context
- **GymDbContext**: Bridge between C# code and SQL Server
- Uses Entity Framework Core
- Automatically maps C# objects to database tables

---

## 🛠️ For New Team Members

### Getting Started (5 minutes)
1. Clone the repository
2. Open in Visual Studio
3. Update `appsettings.json` with your database connection
4. Run `dotnet ef database update`
5. Run `dotnet run`
6. Visit https://localhost:7130/swagger to test

### How to Add a New Endpoint
1. Add method in `GymClassService` (Services/GymClassService.cs)
2. Add HTTP method in `GymClassesController` (Controllers/GymClassesController.cs)
3. Add inline comments explaining what it does
4. Test in Swagger
5. Update this README

### Common Tasks
- **Test endpoints**: Use Swagger UI at `/swagger`
- **View database**: Use SQL Server Management Studio
- **Check logs**: Look at Visual Studio output window
- **Fix migrations**: Delete migration file, run `dotnet ef migrations add YourName`

---

## ⚠️ Troubleshooting

**Database Connection Failed**
- Check SQL Server is running
- Verify connection string in `appsettings.json`
- Check firewall permissions

**Migration Error**
- Run: `dotnet ef database update`
- If still fails: Delete `Migrations` folder (except first one), then `dotnet ef migrations add InitialCreate`

**Port Already in Use**
- Change port in `Properties/launchSettings.json`
- Or restart Visual Studio

**CRUD Endpoints Return 500 Error**
- Check browser console for error message
- Review Visual Studio output window
- Verify database connection

---

## 📝 Code Comments

Every file contains detailed comments explaining:
- What each class does
- How each method works
- Why certain patterns are used
- How to extend the code

Read the comments in these files:
- `Models/GymClass.cs` - Understand the entity
- `Services/GymClassService.cs` - Learn the business logic
- `Controllers/GymClassesController.cs` - See how endpoints work

---

## 🎯 Next Steps

1. ✅ Setup database
2. ✅ Run the app
3. ✅ Test endpoints in Swagger
4. ✅ Read code comments
5. ✅ Add new features
6. ✅ Create pull requests

---

**Happy coding! 🚀**

---

## 🚀 Getting Started

### 1. Clone Repository
```powershell
git clone https://github.com/besartakuci-dev/backend-gym-webapp
cd backend-gym-webapp
```

### 2. Restore Dependencies
```powershell
dotnet restore backend-gym-webapp.csproj
```

### 3. Set Up Database
Update `appsettings.json` if needed (default works for local SQL Server):
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=GymManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Apply migrations:
```powershell
dotnet ef database update --project backend-gym-webapp.csproj
```

### 4. Run Application
```powershell
dotnet run --project backend-gym-webapp.csproj
```

Or press **F5** in Visual Studio.

### 5. Access API Documentation
- **Swagger UI**: https://localhost:7130/swagger
- **Scalar UI**: https://localhost:7130/scalar/v1
- **OpenAPI JSON**: https://localhost:7130/openapi/v1.json

---

## 📚 API Endpoints

### GET - Retrieve Classes

```
GET /api/gymclasses
```
Returns all active classes

```
GET /api/gymclasses/{id}
```
Returns specific class by ID

```
GET /api/gymclasses/available/list
```
Returns classes with available spots

### POST - Create Class

```
POST /api/gymclasses
```
**Request:**
```json
{
  "name": "Morning Yoga",
  "description": "Relaxing yoga session",
  "instructorName": "John Doe",
  "maxCapacity": 20,
  "schedule": "Monday 9:00 AM",
  "difficultyLevel": "Beginner",
  "durationMinutes": 60,
  "price": 15.99
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "name": "Morning Yoga",
  "enrolledCount": 0,
  "createdAt": "2026-04-05T21:30:00Z",
  ...
}
```

### PUT - Update Class

```
PUT /api/gymclasses/{id}
```
Same request body as POST, updates the specified class

### DELETE - Remove Class

```
DELETE /api/gymclasses/{id}
```
Permanently removes the class (returns 204 No Content)

---

## 📁 Project Structure

```
backend-gym-webapp/
├── Models/
│   ├── GymClass.cs           # Database model
│   └── CreateGymClassDto.cs   # API input validation
├── Services/
│   └── GymClassService.cs     # Business logic (CRUD)
├── Controllers/
│   └── GymClassesController.cs # HTTP endpoints
├── Data/
│   ├── GymDbContext.cs        # Database configuration
│   └── GymDbContextFactory.cs
├── Migrations/                # Database schema versions
├── Program.cs                 # Application startup
├── appsettings.json           # Configuration
├── SETUP_GUIDE.md             # Detailed setup instructions
├── DEVELOPER_GUIDE.md         # Code understanding guide
└── README.md                  # This file
```

---

## 🔄 How It Works

### Request Flow
```
Client (Browser/Postman)
    ↓
HTTP Request → Controller
    ↓
Validate DTO → Map to Model
    ↓
Service (Business Logic)
    ↓
Database Context
    ↓
SQL Server
    ↓
Response → Client
```

### Example: Creating a Class

1. Client sends POST request with class data
2. Controller validates using `CreateGymClassDto`
3. DTO is mapped to `GymClass` model
4. Service validates business rules
5. Database context generates SQL
6. SQL Server executes INSERT
7. New class returned to client with ID

---

## 🗄️ Database Schema

### GymClasses Table

| Column | Type | Notes |
|--------|------|-------|
| Id | int | Primary key, auto-generated |
| Name | nvarchar(100) | Required |
| Description | nvarchar(500) | Optional |
| InstructorName | nvarchar(max) | Required |
| MaxCapacity | int | Required, > 0 |
| EnrolledCount | int | Auto-managed, default 0 |
| Schedule | nvarchar(max) | Required |
| DifficultyLevel | nvarchar(max) | Required |
| DurationMinutes | int | Required |
| Price | decimal(18,2) | Required |
| CreatedAt | datetime2 | Auto-managed |
| UpdatedAt | datetime2 | Auto-managed |
| IsActive | bit | Default true |

---

## 🧑‍💼 For Team Leads

### Adding New Team Members
1. Have them read `SETUP_GUIDE.md`
2. Have them read `DEVELOPER_GUIDE.md`
3. Have them run and test the application
4. Review code comments with them
5. Have them complete a small task

### Code Review Checklist
- [ ] Comments added for complex logic
- [ ] DTOs used for API inputs
- [ ] Proper error handling with try-catch
- [ ] Async/await used correctly
- [ ] No SQL injection (using EF Core)
- [ ] Valid database migrations

---

## 📝 Making Changes

### Adding a New Endpoint
1. Add method to service interface
2. Implement method in service
3. Add controller action
4. Test in Swagger

See [DEVELOPER_GUIDE.md](DEVELOPER_GUIDE.md) for detailed examples

### Adding a New Field
1. Add to `GymClass.cs`
2. Add to `CreateGymClassDto.cs`
3. Create migration
4. Apply migration

### Updating Database Schema
```powershell
# Create new migration
dotnet ef migrations add MigrationName --project backend-gym-webapp.csproj

# Apply migration
dotnet ef database update --project backend-gym-webapp.csproj

# Undo last migration
dotnet ef migrations remove --project backend-gym-webapp.csproj
```

---

## 🧪 Testing

### Using Swagger UI
1. Run application (F5)
2. Go to https://localhost:7130/swagger
3. Click endpoint
4. Click "Try it out"
5. Enter parameters/body
6. Click "Execute"

### Using Postman
1. Create new request
2. Select method (GET, POST, etc.)
3. Enter URL
4. For POST/PUT: Add JSON body
5. Send request

### Common Test Scenarios

**Create class:**
```
POST /api/gymclasses
Body: { "name": "Test", "instructorName": "John", "maxCapacity": 10, ... }
```

**Read all:**
```
GET /api/gymclasses
```

**Read specific:**
```
GET /api/gymclasses/1
```

**Update:**
```
PUT /api/gymclasses/1
Body: { "name": "Updated Name", ... }
```

**Delete:**
```
DELETE /api/gymclasses/1
```

---

## 🐛 Troubleshooting

### Database Connection Failed
- Verify SQL Server is running
- Check server name in `appsettings.json`
- Test connection in SSMS

### Port Already in Use
- Change port in `Properties/launchSettings.json`
- Or close other applications using port 7130

### Migrations Won't Apply
- Ensure database is accessible
- Check `appsettings.json` connection string
- Try: `dotnet ef database update -v` (verbose mode)

### "Async method should have await" warning
- Always `await` methods ending in `Async()`
- Or add `.Result` (not recommended)

See [SETUP_GUIDE.md](SETUP_GUIDE.md) for more troubleshooting

---

## 📊 Response Status Codes

| Code | Meaning | Example |
|------|---------|---------|
| 200 | OK | GET successful |
| 201 | Created | POST successful |
| 204 | No Content | DELETE successful |
| 400 | Bad Request | Invalid input |
| 404 | Not Found | Class doesn't exist |
| 500 | Server Error | Database error |

---

## 🔐 Security Notes

- Database connection uses Windows Authentication (local dev)
- For production: Use environment variables for secrets
- Add authentication/authorization before deployment
- Validate all inputs
- Use HTTPS in production

---

## 📚 Documentation Files

- **SETUP_GUIDE.md** - Detailed setup and database configuration
- **DEVELOPER_GUIDE.md** - Code structure and common tasks
- **README.md** - This file
- **Code Comments** - Inline documentation throughout codebase

---

## 🤝 Contributing

1. Create a branch: `git checkout -b feature/new-feature`
2. Make changes with clear comments
3. Test thoroughly
4. Commit: `git commit -m "Add new feature"`
5. Push: `git push origin feature/new-feature`
6. Create Pull Request

---

## 📞 Support

For questions or issues:
1. Check the documentation files
2. Review code comments
3. Ask the team lead
4. Check [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core)

---

## 📄 License

[Add your license info here]

---

**Last Updated**: April 5, 2026  
**Version**: 2.0 (Post-CRUD Implementation)  
**Team Lead**: [Name]

---

### 🎉 You're ready to contribute!

Start with [SETUP_GUIDE.md](SETUP_GUIDE.md) if you haven't already. Happy coding!

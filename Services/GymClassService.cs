using backend_gym_webapp.Data;
using backend_gym_webapp.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_gym_webapp.Services
{
    /// <summary>
    /// IGymClassService - Interface (Contract) that defines what operations we can do
    /// 
    /// Why use an interface?
    /// - Makes code flexible and testable
    /// - Defines the "contract" that any service must follow
    /// - Makes it easy to swap implementations
    /// - Helps with dependency injection (ASP.NET injects this automatically)
    /// 
    /// Think of it like a menu at a restaurant:
    /// - Interface = the menu (what's available)
    /// - Service = the kitchen (the actual implementation)
    /// </summary>
    public interface IGymClassService
    {
        // ===== CREATE =====
        /// <summary>
        /// CreateClassAsync - Add a new gym class to the database
        /// Input: GymClass object with all required fields
        /// Output: The created class with generated ID and timestamps
        /// Example use: When admin creates a new class
        /// </summary>
        Task<GymClass> CreateClassAsync(GymClass gymClass);

        // ===== READ =====
        /// <summary>
        /// GetAllClassesAsync - Retrieve all active gym classes
        /// Output: List of all active classes
        /// Example use: When users browse available classes
        /// </summary>
        Task<List<GymClass>> GetAllClassesAsync();

        /// <summary>
        /// GetClassByIdAsync - Get a specific class by its ID
        /// Input: Class ID (e.g., 1)
        /// Output: The class or null if not found
        /// Example use: When user views details of one class
        /// </summary>
        Task<GymClass?> GetClassByIdAsync(int id);

        // ===== UPDATE =====
        /// <summary>
        /// UpdateClassAsync - Modify an existing class
        /// Input: Class ID and updated GymClass data
        /// Output: The updated class
        /// Example use: When admin edits class details
        /// </summary>
        Task<GymClass> UpdateClassAsync(int id, GymClass gymClass);

        // ===== DELETE =====
        /// <summary>
        /// DeleteClassAsync - Remove a class from database
        /// Input: Class ID
        /// Output: true if successful
        /// Example use: When admin removes a class
        /// </summary>
        Task<bool> DeleteClassAsync(int id);

        /// <summary>
        /// EnrollUserAsync - Enroll one user in a gym class
        /// Input: Class ID
        /// Output: true if enrollment succeeds; false if class not found or full
        /// Example use: When a user clicks "Enroll" for a class
        /// </summary>
        Task<bool> EnrollUserAsync(int id);

        // ===== HELPER METHODS =====
        /// <summary>
        /// GetAvailableClassesAsync - Get classes with available spots
        /// Output: Classes where EnrolledCount < MaxCapacity
        /// Example use: Show users which classes they can still join
        /// </summary>
        Task<List<GymClass>> GetAvailableClassesAsync();
    }

    /// <summary>
    /// GymClassService - The actual implementation of IGymClassService
    /// 
    /// This is where the real business logic happens:
    /// - Validates data before saving
    /// - Handles database operations
    /// - Contains all CRUD logic
    /// 
    /// Flow:
    /// Request → Controller → Service → Database Context → SQL Server
    /// </summary>
    public class GymClassService : IGymClassService
    {
        // ===== DEPENDENCY INJECTION =====
        /// <summary>
        /// _context - The database connection
        /// 
        /// Dependency Injection explained:
        /// - Instead of creating the context ourselves, ASP.NET injects it
        /// - This makes testing easier
        /// - This centralizes database management
        /// - The "readonly" keyword prevents accidental changes
        /// 
        /// Where it comes from:
        /// 1. In Program.cs: builder.Services.AddScoped<IGymClassService, GymClassService>();
        /// 2. ASP.NET automatically provides GymDbContext to the constructor
        /// 3. We store it in _context
        /// </summary>
        private readonly GymDbContext _context;

        /// <summary>
        /// Constructor - Runs when the service is created
        /// 
        /// The 'context' parameter is automatically provided by ASP.NET
        /// This is called "Dependency Injection" or "DI"
        /// </summary>
        public GymClassService(GymDbContext context)
        {
            _context = context;
        }

        // ==================== CREATE OPERATION ====================
        /// <summary>
        /// CreateClassAsync - Add a new gym class to the database
        /// 
        /// Steps:
        /// 1. Validate that required fields are not empty
        /// 2. Set auto-managed fields (timestamps, enrolled count)
        /// 3. Add the class to the database context
        /// 4. Save changes to SQL Server
        /// 5. Return the created class
        /// 
        /// Validation:
        /// - Name must not be empty
        /// - MaxCapacity must be greater than 0
        /// 
        /// Auto-managed fields:
        /// - CreatedAt: Set to current UTC time
        /// - UpdatedAt: Set to current UTC time
        /// - EnrolledCount: Set to 0 (no one enrolled yet)
        /// </summary>
        public async Task<GymClass> CreateClassAsync(GymClass gymClass)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(gymClass.Name))
                throw new ArgumentException("Class name is required");

            if (gymClass.MaxCapacity <= 0)
                throw new ArgumentException("Max capacity must be greater than 0");

            // Set auto-managed fields to current UTC time
            gymClass.CreatedAt = DateTime.UtcNow;
            gymClass.UpdatedAt = DateTime.UtcNow;
            gymClass.EnrolledCount = 0; // No one is enrolled yet

            // Add to database context (not saved yet, just added to memory)
            _context.GymClasses.Add(gymClass);

            // Save changes to SQL Server (this is when the INSERT happens)
            await _context.SaveChangesAsync();

            return gymClass;
        }

        // ==================== READ OPERATIONS ====================
        /// <summary>
        /// GetAllClassesAsync - Retrieve all active gym classes
        /// 
        /// Steps:
        /// 1. Query all classes from the database
        /// 2. Filter to only show active classes (IsActive = true)
        /// 3. Sort by schedule (e.g., Monday classes first)
        /// 4. Return the list
        /// 
        /// "async" and "await" explained:
        /// - These keywords allow the operation to not block the application
        /// - While waiting for the database, the server can handle other requests
        /// - Makes the app more responsive and efficient
        /// </summary>
        public async Task<List<GymClass>> GetAllClassesAsync()
        {
            // Fetch all classes, filtered and sorted
            return await _context.GymClasses
                .Where(g => g.IsActive) // Only show active classes
                .OrderBy(g => g.Schedule) // Sort by schedule
                .ToListAsync(); // Convert to list and wait for result
        }

        /// <summary>
        /// GetClassByIdAsync - Get a specific class by ID
        /// 
        /// Steps:
        /// 1. Search for class with matching ID
        /// 2. Return the class or null if not found
        /// 
        /// "?" in return type means "nullable" - can be null
        /// </summary>
        public async Task<GymClass?> GetClassByIdAsync(int id)
        {
            // Find first class with matching ID, or null if not found
            return await _context.GymClasses.FirstOrDefaultAsync(g => g.Id == id);
        }

        // ==================== UPDATE OPERATION ====================
        /// <summary>
        /// UpdateClassAsync - Modify an existing class
        /// 
        /// Steps:
        /// 1. Find the existing class in database
        /// 2. If not found, throw an error
        /// 3. Update only the fields that matter
        /// 4. Update the UpdatedAt timestamp to show when it was modified
        /// 5. Save to database
        /// 6. Return the updated class
        /// 
        /// Note: We don't update CreatedAt, Id, or EnrolledCount
        /// These are managed by the system, not by the user
        /// </summary>
        public async Task<GymClass> UpdateClassAsync(int id, GymClass gymClass)
        {
            // Find the existing class in database
            var existingClass = await _context.GymClasses.FindAsync(id);

            // If not found, throw error (controller catches this and returns 404)
            if (existingClass == null)
                throw new KeyNotFoundException($"Class with ID {id} not found");

            // Update only the user-provided fields
            existingClass.Name = gymClass.Name;
            existingClass.Description = gymClass.Description;
            existingClass.InstructorName = gymClass.InstructorName;
            existingClass.MaxCapacity = gymClass.MaxCapacity;
            existingClass.Schedule = gymClass.Schedule;
            existingClass.DifficultyLevel = gymClass.DifficultyLevel;
            existingClass.DurationMinutes = gymClass.DurationMinutes;
            existingClass.Price = gymClass.Price;
            existingClass.IsActive = gymClass.IsActive;

            // Update the timestamp to show when it was modified
            // This automatically updates because EF is tracking the object
            existingClass.UpdatedAt = DateTime.UtcNow;

            // Save changes to database
            await _context.SaveChangesAsync();

            return existingClass;
        }

        // ==================== DELETE OPERATION ====================
        /// <summary>
        /// DeleteClassAsync - Remove a class from database
        /// 
        /// Steps:
        /// 1. Find the class to delete
        /// 2. If not found, throw error
        /// 3. Remove it from database context
        /// 4. Save changes to database
        /// 5. Return true to indicate success
        /// </summary>
        public async Task<bool> DeleteClassAsync(int id)
        {
            // Find the class to delete
            var gymClass = await _context.GymClasses.FindAsync(id);

            // If not found, throw error
            if (gymClass == null)
                throw new KeyNotFoundException($"Class with ID {id} not found");

            // Remove from database context
            _context.GymClasses.Remove(gymClass);

            // Save changes to database (this is when the DELETE happens)
            await _context.SaveChangesAsync();

            return true;
        }

        // ==================== ENROLLMENT OPERATION ====================
        /// <summary>
        /// EnrollUserAsync - Enroll one user in a class if capacity allows
        ///
        /// Steps:
        /// 1. Find class by ID
        /// 2. Return false if class doesn't exist
        /// 3. Check if class is full
        /// 4. Return false if no spots remain
        /// 5. Increment enrolled count and update timestamp
        /// 6. Save changes and return true
        /// </summary>
        public async Task<bool> EnrollUserAsync(int id)
        {
            var gymClass = await _context.GymClasses.FindAsync(id);

            if (gymClass == null)
            {
                return false;
            }

            if (gymClass.EnrolledCount >= gymClass.MaxCapacity)
            {
                return false;
            }

            gymClass.EnrolledCount += 1;
            gymClass.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        // ==================== HELPER METHOD ====================
        /// <summary>
        /// GetAvailableClassesAsync - Get classes that have available spots
        /// 
        /// Useful for:
        /// - Showing users which classes they can still join
        /// - Filtering out full classes
        /// 
        /// Logic:
        /// - IsActive = true (class is active)
        /// - EnrolledCount < MaxCapacity (has available spots)
        /// </summary>
        public async Task<List<GymClass>> GetAvailableClassesAsync()
        {
            return await _context.GymClasses
                .Where(g => g.IsActive && g.EnrolledCount < g.MaxCapacity)
                .OrderBy(g => g.Schedule)
                .ToListAsync();
        }
    }
}

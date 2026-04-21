using backend_gym_webapp.Models;
using backend_gym_webapp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend_gym_webapp.Controllers
{
    /// <summary>
    /// GymClassesController - API endpoints for gym class CRUD operations
    /// 
    /// What is a Controller?
    /// - Handles incoming HTTP requests from clients
    /// - Acts as the "gatekeeper" to the API
    /// - Routes requests to the appropriate service
    /// - Returns responses back to clients
    /// 
    /// How HTTP requests work:
    /// 1. Client sends request (GET, POST, PUT, DELETE)
    /// 2. ASP.NET routes it to this controller
    /// 3. The correct method (action) runs
    /// 4. The method calls the service
    /// 5. The service talks to the database
    /// 6. Response comes back through the chain
    /// 
    /// Attributes explained:
    /// - [ApiController] - Tells ASP.NET this is an API controller
    /// - [Route("api/[controller]")] - Base URL is /api/gymclasses
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GymClassesController : ControllerBase
    {
        // ===== DEPENDENCY INJECTION =====
        /// <summary>
        /// _service - The service that contains all business logic
        /// 
        /// Dependency Injection (DI) explained:
        /// - The service is automatically injected by ASP.NET
        /// - We don't create it ourselves
        /// - This makes the code testable and flexible
        /// - Multiple requests share the same service instance
        /// 
        /// Where does this come from?
        /// In Program.cs: builder.Services.AddScoped<IGymClassService, GymClassService>();
        /// This tells ASP.NET: "Whenever a controller needs IGymClassService, give it GymClassService"
        /// </summary>
        private readonly IGymClassService _service;

        /// <summary>
        /// Constructor - Runs when the controller is created
        /// 
        /// The 'service' parameter is automatically provided by ASP.NET (Dependency Injection)
        /// We store it in _service to use it throughout the class
        /// </summary>
        public GymClassesController(IGymClassService service)
        {
            _service = service;
        }

        // ==================== CREATE OPERATION ====================
        /// <summary>
        /// POST /api/gymclasses
        /// Creates a new gym class
        /// 
        /// HTTP Method: POST
        /// URL: https://localhost:7130/api/gymclasses
        /// 
        /// Request Body (JSON):
        /// {
        ///   "name": "Morning Yoga",
        ///   "description": "Relaxing yoga session",
        ///   "instructorName": "John Doe",
        ///   "maxCapacity": 20,
        ///   "schedule": "Monday 9:00 AM",
        ///   "difficultyLevel": "Beginner",
        ///   "durationMinutes": 60,
        ///   "price": 15.99
        /// }
        /// 
        /// Response (201 Created):
        /// {
        ///   "id": 1,
        ///   "name": "Morning Yoga",
        ///   ... (all fields including auto-generated ones)
        /// }
        /// 
        /// Status Codes:
        /// - 200 OK - Class created successfully
        /// - 400 Bad Request - Invalid input
        /// - 500 Internal Server Error - Database error
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<GymClass>> CreateClass([FromBody] CreateGymClassDto dto)
        {
            try
            {
                // Validate the input
                if (dto == null)
                    return BadRequest("Class data is required");

                if (string.IsNullOrWhiteSpace(dto.Name))
                    return BadRequest("Class name is required");

                // Map DTO to GymClass entity
                // DTO = Data Transfer Object (what the client sends)
                // Entity = What we store in the database
                var gymClass = new GymClass
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    InstructorName = dto.InstructorName,
                    MaxCapacity = dto.MaxCapacity,
                    Schedule = dto.Schedule,
                    DifficultyLevel = dto.DifficultyLevel,
                    DurationMinutes = dto.DurationMinutes,
                    Price = dto.Price,
                    IsActive = dto.IsActive
                };

                // Call service to create the class
                // The service handles validation, timestamps, and database save
                var createdClass = await _service.CreateClassAsync(gymClass);

                // Return 201 Created with the new class data
                // CreatedAtAction generates a Location header pointing to the new resource
                // Example: Location: /api/gymclasses/1
                return CreatedAtAction(nameof(GetClassById), new { id = createdClass.Id }, createdClass);
            }
            catch (ArgumentException ex)
            {
                // Validation error from service
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                // Database error
                return StatusCode(500, $"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                // Any other error
                return StatusCode(500, $"Error creating class: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        // ==================== READ OPERATIONS ====================
        /// <summary>
        /// GET /api/gymclasses
        /// Retrieves all active gym classes
        /// 
        /// HTTP Method: GET
        /// URL: https://localhost:7130/api/gymclasses
        /// No request body needed
        /// 
        /// Response (200 OK):
        /// [
        ///   {
        ///     "id": 1,
        ///     "name": "Morning Yoga",
        ///     ...
        ///   },
        ///   {
        ///     "id": 2,
        ///     "name": "CrossFit 101",
        ///     ...
        ///   }
        /// ]
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<GymClass>>> GetAllClasses()
        {
            try
            {
                var classes = await _service.GetAllClassesAsync();
                return Ok(classes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving classes: {ex.Message}");
            }
        }

        /// <summary>
        /// GET /api/gymclasses/{id}
        /// Retrieves a specific gym class by ID
        /// 
        /// HTTP Method: GET
        /// URL: https://localhost:7130/api/gymclasses/1
        /// No request body needed
        /// 
        /// Path Parameter:
        /// - id: The ID of the class to retrieve (e.g., 1)
        /// 
        /// Response (200 OK):
        /// {
        ///   "id": 1,
        ///   "name": "Morning Yoga",
        ///   ...
        /// }
        /// 
        /// If not found (404):
        /// "Class with ID 999 not found"
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<GymClass>> GetClassById(int id)
        {
            try
            {
                var gymClass = await _service.GetClassByIdAsync(id);

                // If not found, return 404 Not Found
                if (gymClass == null)
                    return NotFound($"Class with ID {id} not found");

                return Ok(gymClass);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving class: {ex.Message}");
            }
        }

        /// <summary>
        /// GET /api/gymclasses/available/list
        /// Retrieves only classes with available spots
        /// 
        /// HTTP Method: GET
        /// URL: https://localhost:7130/api/gymclasses/available/list
        /// 
        /// What this does:
        /// - Returns only classes where EnrolledCount < MaxCapacity
        /// - In other words, classes that still have room for more people
        /// - Useful for showing users which classes they can still join
        /// 
        /// Response (200 OK):
        /// [
        ///   {
        ///     "id": 1,
        ///     "name": "Morning Yoga",
        ///     "enrolledCount": 18,
        ///     "maxCapacity": 20,
        ///     ... (has 2 spots available)
        ///   }
        /// ]
        /// 
        /// NOTE: This route MUST come before {id} route to avoid conflicts
        /// Because "/available/list" would otherwise be interpreted as id="available"
        /// </summary>
        [HttpGet("available/list")]
        public async Task<ActionResult<List<GymClass>>> GetAvailableClasses()
        {
            try
            {
                var classes = await _service.GetAvailableClassesAsync();
                return Ok(classes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving available classes: {ex.Message}");
            }
        }

        /// <summary>
        /// POST /api/gymclasses/{id}/enroll
        /// Enrolls a user in a class if there is available capacity.
        /// </summary>
        [HttpPost("{id}/enroll")]
        public async Task<ActionResult> EnrollUser(int id)
        {
            try
            {
                var enrolled = await _service.EnrollUserAsync(id);

                if (!enrolled)
                {
                    return BadRequest("Class is full");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error enrolling user: {ex.Message}");
            }
        }

        // ==================== UPDATE OPERATION ====================
        /// <summary>
        /// PUT /api/gymclasses/{id}
        /// Updates an existing gym class
        /// 
        /// HTTP Method: PUT
        /// URL: https://localhost:7130/api/gymclasses/1
        /// 
        /// Path Parameter:
        /// - id: The ID of the class to update (e.g., 1)
        /// 
        /// Request Body (JSON):
        /// {
        ///   "name": "Advanced Yoga",
        ///   "description": "Updated description",
        ///   "instructorName": "Jane Doe",
        ///   "maxCapacity": 25,
        ///   "schedule": "Tuesday 9:00 AM",
        ///   "difficultyLevel": "Advanced",
        ///   "durationMinutes": 90,
        ///   "price": 19.99
        /// }
        /// 
        /// Response (200 OK):
        /// {
        ///   "id": 1,
        ///   "name": "Advanced Yoga",
        ///   ... (returns updated class)
        /// }
        /// 
        /// Note: id, enrolledCount, createdAt are NOT updated by the client
        /// These are managed by the system
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<GymClass>> UpdateClass(int id, [FromBody] CreateGymClassDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Class data is required");

                // Map DTO to GymClass entity
                var gymClass = new GymClass
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    InstructorName = dto.InstructorName,
                    MaxCapacity = dto.MaxCapacity,
                    Schedule = dto.Schedule,
                    DifficultyLevel = dto.DifficultyLevel,
                    DurationMinutes = dto.DurationMinutes,
                    Price = dto.Price,
                    IsActive = dto.IsActive
                };

                // Call service to update
                // The service finds the existing class and updates only the fields we provide
                var updatedClass = await _service.UpdateClassAsync(id, gymClass);

                return Ok(updatedClass);
            }
            catch (KeyNotFoundException ex)
            {
                // Class with that ID not found
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                // Validation error
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating class: {ex.Message}");
            }
        }

        // ==================== DELETE OPERATION ====================
        /// <summary>
        /// DELETE /api/gymclasses/{id}
        /// Deletes a gym class permanently
        /// 
        /// HTTP Method: DELETE
        /// URL: https://localhost:7130/api/gymclasses/1
        /// 
        /// Path Parameter:
        /// - id: The ID of the class to delete (e.g., 1)
        /// 
        /// Response (204 No Content):
        /// Empty response body on success
        /// 
        /// Status Codes:
        /// - 204 No Content - Successfully deleted
        /// - 404 Not Found - Class doesn't exist
        /// - 500 Internal Server Error - Database error
        /// 
        /// WARNING: This is permanent! Deleted classes cannot be recovered.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClass(int id)
        {
            try
            {
                // Call service to delete
                // The service finds the class and removes it from database
                await _service.DeleteClassAsync(id);

                // Return 204 No Content (successful deletion, nothing to return)
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                // Class not found
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting class: {ex.Message}");
            }
        }
    }
}

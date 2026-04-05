namespace backend_gym_webapp.Models
{
    /// <summary>
    /// CreateGymClassDto - Data Transfer Object for creating a new gym class
    /// This only contains the fields that should be sent by the client
    /// Read-only fields like Id, CreatedAt, UpdatedAt are excluded
    /// </summary>
    public class CreateGymClassDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string InstructorName { get; set; } = string.Empty;

        public int MaxCapacity { get; set; }

        public string Schedule { get; set; } = string.Empty;

        public string DifficultyLevel { get; set; } = "Beginner";

        public int DurationMinutes { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;
    }
}

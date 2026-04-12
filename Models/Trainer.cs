namespace backend_gym_webapp.Models
{
    public class Trainer
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Specialty { get; set; } = "";
        public int ExperienceYears { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
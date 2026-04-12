namespace backend_gym_webapp.Models
{
    // This model represents a membership plan in the gym
    public class MembershipPlan
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public string Description { get; set; } = "";
        public bool IsActive { get; set; } = true;
    }
}
namespace backend_gym_webapp.Models
{
    // This DTO is used to return dashboard statistics
    public class DashboardDto
    {
        public int ActiveMembers { get; set; }
        public int TotalMembershipPlans { get; set; }
        public string MostPopularClass { get; set; } = "";
    }
}
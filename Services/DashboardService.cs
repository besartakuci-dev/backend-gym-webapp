using backend_gym_webapp.Data;
using backend_gym_webapp.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_gym_webapp.Services
{
    // This service calculates dashboard statistics
    public class DashboardService
    {
        private readonly GymDbContext _context;

        public DashboardService(GymDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            // Count active trainers as active members for now
            int activeMembers = await _context.Trainers.CountAsync(t => t.IsActive);

            // Count total membership plans
            int totalMembershipPlans = await _context.MembershipPlans.CountAsync();

            // Find the most popular gym class by name
            string mostPopularClass = await _context.GymClasses
                .OrderBy(g => g.Name)
                .Select(g => g.Name)
                .FirstOrDefaultAsync() ?? "No class found";

            return new DashboardDto
            {
                ActiveMembers = activeMembers,
                TotalMembershipPlans = totalMembershipPlans,
                MostPopularClass = mostPopularClass
            };
        }
    }
}
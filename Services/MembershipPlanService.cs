using backend_gym_webapp.Data;
using backend_gym_webapp.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_gym_webapp.Services
{
    public class MembershipPlanService
    {
        private readonly GymDbContext _context;

        public MembershipPlanService(GymDbContext context)
        {
            _context = context;
        }

        public async Task<List<MembershipPlan>> GetAllAsync()
        {
            return await _context.MembershipPlans.ToListAsync();
        }

        public async Task<MembershipPlan?> GetByIdAsync(int id)
        {
            return await _context.MembershipPlans.FindAsync(id);
        }

        public async Task<MembershipPlan> CreateAsync(MembershipPlan plan)
        {
            _context.MembershipPlans.Add(plan);
            await _context.SaveChangesAsync();
            return plan;
        }

        public async Task<bool> UpdateAsync(int id, MembershipPlan updated)
        {
            var plan = await _context.MembershipPlans.FindAsync(id);

            if (plan == null)
                return false;

            plan.Name = updated.Name;
            plan.Price = updated.Price;
            plan.DurationDays = updated.DurationDays;
            plan.Description = updated.Description;
            plan.IsActive = updated.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var plan = await _context.MembershipPlans.FindAsync(id);

            if (plan == null)
                return false;

            _context.MembershipPlans.Remove(plan);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
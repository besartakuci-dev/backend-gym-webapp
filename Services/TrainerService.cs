using backend_gym_webapp.Data;
using backend_gym_webapp.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_gym_webapp.Services
{
    // This service contains the business logic for Trainer CRUD
    public class TrainerService
    {
        private readonly GymDbContext _context;

        // Constructor: gets database context through dependency injection
        public TrainerService(GymDbContext context)
        {
            _context = context;
        }

        // GET ALL trainers
        public async Task<List<Trainer>> GetAllAsync()
        {
            return await _context.Trainers.ToListAsync();
        }

        // GET trainer by id
        public async Task<Trainer?> GetByIdAsync(int id)
        {
            return await _context.Trainers.FindAsync(id);
        }

        // CREATE new trainer
        public async Task<Trainer> CreateAsync(Trainer trainer)
        {
            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();
            return trainer;
        }

        // UPDATE existing trainer
        public async Task<bool> UpdateAsync(int id, Trainer updatedTrainer)
        {
            var trainer = await _context.Trainers.FindAsync(id);

            if (trainer == null)
                return false;

            trainer.Name = updatedTrainer.Name;
            trainer.Specialty = updatedTrainer.Specialty;
            trainer.ExperienceYears = updatedTrainer.ExperienceYears;
            trainer.IsActive = updatedTrainer.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        // DELETE trainer
        public async Task<bool> DeleteAsync(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);

            if (trainer == null)
                return false;

            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
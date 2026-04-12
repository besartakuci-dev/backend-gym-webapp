using backend_gym_webapp.Models;
using backend_gym_webapp.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend_gym_webapp.Controllers
{
    // This controller handles HTTP requests for Trainer CRUD
    [ApiController]
    [Route("api/[controller]")]
    public class TrainersController : ControllerBase
    {
        private readonly TrainerService _service;

        // Constructor: gets TrainerService automatically
        public TrainersController(TrainerService service)
        {
            _service = service;
        }

        // GET: api/trainers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var trainers = await _service.GetAllAsync();
            return Ok(trainers);
        }

        // GET: api/trainers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var trainer = await _service.GetByIdAsync(id);

            if (trainer == null)
                return NotFound("Trainer not found.");

            return Ok(trainer);
        }

        // POST: api/trainers
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Trainer trainer)
        {
            var createdTrainer = await _service.CreateAsync(trainer);
            return Ok(createdTrainer);
        }

        // PUT: api/trainers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Trainer trainer)
        {
            var updated = await _service.UpdateAsync(id, trainer);

            if (!updated)
                return NotFound("Trainer not found.");

            return Ok("Trainer updated successfully.");
        }

        // DELETE: api/trainers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound("Trainer not found.");

            return Ok("Trainer deleted successfully.");
        }
    }
}
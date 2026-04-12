using backend_gym_webapp.Models;
using backend_gym_webapp.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend_gym_webapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipPlansController : ControllerBase
    {
        private readonly MembershipPlanService _service;

        public MembershipPlansController(MembershipPlanService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var plan = await _service.GetByIdAsync(id);

            if (plan == null)
                return NotFound();

            return Ok(plan);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MembershipPlan plan)
        {
            return Ok(await _service.CreateAsync(plan));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MembershipPlan plan)
        {
            var ok = await _service.UpdateAsync(id, plan);
            return ok ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? Ok() : NotFound();
        }
    }
}
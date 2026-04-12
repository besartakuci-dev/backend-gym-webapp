using backend_gym_webapp.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend_gym_webapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _service;

        public DashboardController(DashboardService service)
        {
            _service = service;
        }

        // GET: api/dashboard
        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var data = await _service.GetDashboardDataAsync();
            return Ok(data);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Dierentuin_App.Services;

namespace Dierentuin_App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DayNightController : ControllerBase
    {
        private readonly DayNightService _dayNightService;

        public DayNightController(DayNightService dayNightService)
        {
            _dayNightService = dayNightService;
        }

        [HttpGet("IsDay")]
        public ActionResult<bool> IsDay()
        {
            return _dayNightService.IsDay;
        }
    }
}
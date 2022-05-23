using Microsoft.AspNetCore.Mvc;

namespace CommonServices.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class MathematicController : ControllerBase
	{
		private readonly ILogger<MathematicController> _logger;

		public MathematicController(ILogger<MathematicController> logger)
		{
			_logger = logger;
		}

		[HttpPost]
		[Route("Calculate")]
		[ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		public IActionResult Calculate([FromBody] string mathEquation)
		{
			try
			{
				var result = Calculator.Solve(mathEquation);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
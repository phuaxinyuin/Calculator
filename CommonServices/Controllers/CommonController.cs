using CommonServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommonServices.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CommonController : ControllerBase
	{
		private readonly IMathematicService _mathService;

		public CommonController(IMathematicService mathService)
		{
			_mathService = mathService;
		}

		[HttpPost]
		[Route("Calculate")]
		[ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		public IActionResult Calculate([FromBody] string mathEquation)
		{
			try
			{
				var result = _mathService.Solve(mathEquation);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}

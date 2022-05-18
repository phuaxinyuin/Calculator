using CommonServices.Constants;
using CommonServices.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CommonServices.UnitTest;

public class TestMathematicController
{
	[Theory]
	[MemberData(nameof(SuccessData))]
	public async Task Calculate_ShouldReturn200Status(string math, double expected)
	{
		// Arrange
		var mockRepo = new Mock<ILogger<MathematicController>>();
		var controller = new MathematicController(mockRepo.Object);

		// Act
		IActionResult actionResult = controller.Calculate(math);

		// Assert			  
		var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
		Assert.Equal(expected, okObjectResult.Value);
	}

	[Theory]
	[MemberData(nameof(InvalidData))]
	public async Task Calculate_ShouldReturn400Status(string math, string expected)
	{
		// Arrange
		var mockRepo = new Mock<ILogger<MathematicController>>();
		var controller = new MathematicController(mockRepo.Object);

		// Act
		IActionResult actionResult = controller.Calculate(math);

		// Assert			  
		var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult);
		Assert.Equal(expected, badRequestObjectResult.Value);
	}

	public static IEnumerable<object[]> SuccessData =>
	   new List<object[]>
	   {
			new object[] { "1 + 1", 2 },
			new object[] { "2 * 2", 4 },
			new object[] { "1 + 2 + 3", 6 },
			new object[] { "6 / 2", 3 },
			new object[] { "11 + 23", 34 },
			new object[] { "11.1 + 23", 34.1 },
			new object[] { "1 + 1 * 3", 4 },
			new object[] { "( 11.5 + 15.4 ) + 10.1", 37 },
			new object[] { "23 - ( 29.3 - 12.5 )", 6.2 },
			new object[] { "( 1 / 2 ) - 1 + 1", 0.5 },
			new object[] { "10 - ( 2 + 3 * ( 7 - 5 ) )", 2 },
			// additional handling	  
			new object[] { "- 2", -2 },
			new object[] { "( 6 )", 6 },
			new object[] { "( -10 )", -10 },
			new object[] { "( ( -9.6 ) )", -9.6 },
			new object[] { "- ( 6 )", -6 },
			new object[] { "- ( -10 )", 10 },
			new object[] { "2 + ( ( ( -10 ) ) )", -8 },
			new object[] { "2 + ( ( ( -10 ) ) / 2 )", -3 },
			new object[] { "( 3 ) ( 2 )", 6 },
			new object[] { "6 / ( 3 ) ( 2 )", 4 },
			new object[] { "( 3 + 2 ) ( 2 * 2 )", 20 },
			new object[] { "- ( 2 ) + 2", 0 }   ,
			new object[] { "- ( 2 ) * 2", -4 },
			new object[] { "- ( 2 ) ( 2 - 5 ) / 6", 1 },
			new object[] { "7 - ( -1 )", 8 },
			new object[] { "2 ( -1 )", -2 },
			new object[] { "48 / 2 ( 9 + 3 )", 288 },
			new object[] { "3 * 2 ^ 2", 12 },
			new object[] { "( 3 * 2 ) ^ 2", 36 },
			new object[] { "-10 / -2", 5 },
			new object[] { "2 ^ 2 ^ 3", 256 },
			new object[] { "1  +   1", 2 },
			new object[] { "1 x 1", 1 }
	   };

	public static IEnumerable<object[]> InvalidData =>
		new List<object[]>
		{
			new object[] { "+ 1 + 1", ErrorMessage.InvalidInput },
			new object[] { "+ 1 ! 1", ErrorMessage.InvalidInput },
			new object[] { "1 + S", ErrorMessage.InvalidInput },
			new object[] { "-", ErrorMessage.InvalidInput },
			new object[] { "- +", ErrorMessage.InvalidInput },
			new object[] { "2 2", ErrorMessage.OperatorNotFound }
		};
}
namespace CommonServices.Services
{
	public class MathematicService : IMathematicService
	{
		public double Solve(string equation)
		{
			return Calculator.Solve(equation);
		}
	}
}

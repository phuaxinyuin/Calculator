using CommonServices.Constants;

namespace CommonServices.ArithmeticOperations
{
	public class Exponents : ArithmeticOperation
	{
		public override string Name => "Exponents";

		public override List<string> Symbols => new() { MathSymbol.Exponents };

		public override double Calculate(double number1, double number2)
		{
			return Math.Pow(number1, number2);
		}

		public override int GetIndex(List<string> items, string targetSymbol)
		{
			return RightAssociativityIndex(items, targetSymbol);
		}
	}
}

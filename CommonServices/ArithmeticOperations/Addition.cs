using CommonServices.Constants;

namespace CommonServices.ArithmeticOperations
{
	public class Addition : ArithmeticOperation
	{
		public override  string Name => "Addition";

		public override List<string> Symbols => new() { MathSymbol.Addition };

		public override double Calculate(double number1, double number2)
		{
			return number1 + number2;
		}

		public override int GetIndex(List<string> items, string targetSymbol)
		{
			return LeftAssociativityIndex(items, targetSymbol);
		}
	}
}

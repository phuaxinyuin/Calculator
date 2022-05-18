using CommonServices.Constants;

namespace CommonServices.ArithmeticOperations
{
	public class Division : ArithmeticOperation
	{
		public override string Name => "Division";

		public override List<string> Symbols => new() { MathSymbol.Division1, MathSymbol.Division2 };

		public override double Calculate(double number1, double number2)
		{
			return number1 / number2;
		}

		public override int GetIndex(List<string> items, string targetSymbol)
		{
			return LeftAssociativityIndex(items, targetSymbol);
		}
	}
}

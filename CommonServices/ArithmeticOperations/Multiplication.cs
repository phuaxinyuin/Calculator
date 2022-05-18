using CommonServices.Constants;

namespace CommonServices.ArithmeticOperations
{
	public class Multiplication : ArithmeticOperation
	{
		public override string Name => "Multiplication";

		public override List<string> Symbols => new() { MathSymbol.Multiplication1, MathSymbol.Multiplication2 };

		public override double Calculate(double number1, double number2)
		{
			return number1 * number2;
		}

		public override int GetIndex(List<string> items, string targetSymbol)
		{
			return LeftAssociativityIndex(items, targetSymbol);
		}
	}
}

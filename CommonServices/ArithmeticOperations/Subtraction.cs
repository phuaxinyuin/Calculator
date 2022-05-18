using CommonServices.Constants;

namespace CommonServices.ArithmeticOperations
{
	public class Subtraction : ArithmeticOperation
	{
		public override string Name => "Subtraction";

		public override List<string> Symbols => new() { MathSymbol.Subtraction };

		public override double Calculate(double number1, double number2)
		{
			// Rules for Subtraction Operations (-)
			// x - ( -y ) --> x + y
			return Convert.ToDouble(Convert.ToDecimal(number1) + Convert.ToDecimal(number2 * -1));
		}

		public override int GetIndex(List<string> items, string targetSymbol)
		{
			return LeftAssociativityIndex(items, targetSymbol);
		}
	}
}

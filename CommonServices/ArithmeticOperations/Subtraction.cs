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

		public override bool Validate(List<string> items, int index, out double number1, out double number2)
		{
			if (index == 0)
			{
				number1 = 0;
				return double.TryParse(items[index + 1], out number2);
			}
			else
			{
				number2 = 0;
				return double.TryParse(items[index - 1], out number1) && double.TryParse(items[index + 1], out number2);
			}
		}
	}
}

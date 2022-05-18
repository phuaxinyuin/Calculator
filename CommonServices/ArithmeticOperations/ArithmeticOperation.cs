using CommonServices.Constants;

namespace CommonServices.ArithmeticOperations
{
	public abstract class ArithmeticOperation
	{
		public abstract string Name { get; }
		public abstract List<string> Symbols { get; }

		public abstract double Calculate(double number1, double number2);

		public abstract int GetIndex(List<string> items, string targetSymbol);

		public virtual bool Validate(List<string> items, int index, out double number1, out double number2)
		{
			if (index == 0)
			{
				throw new Exception(ErrorMessage.InvalidInput);
			}

			number2 = 0;
			return double.TryParse(items[index - 1], out number1) && double.TryParse(items[index + 1], out number2);
		}

		public int LeftAssociativityIndex(List<string> items, string targetSymbol)
		{
			return items.IndexOf(targetSymbol);
		}

		public int RightAssociativityIndex(List<string> items, string targetSymbol)
		{
			return items.LastIndexOf(targetSymbol);
		}
	}
}
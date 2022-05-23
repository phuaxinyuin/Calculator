using CommonServices.Constants;

namespace CommonServices.ArithmeticOperations
{
	public abstract class ArithmeticOperation 
	{
		private static readonly List<string> _exponentsSymbols = new Exponents().Symbols;
		private static readonly List<string> _multiplicationDivisionSymbols = new Multiplication().Symbols.Concat(new Division().Symbols).ToList();
		private static readonly List<string> _additionSubtractionSymbols = new Addition().Symbols.Concat(new Subtraction().Symbols).ToList();

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

		public static string? GetTargetMathSymbolByOperationsOrder(List<string> items)
		{
			// Math Order of Operations - PEMDAS
			return items.LastOrDefault(x => _exponentsSymbols.Contains(x))
				?? items.FirstOrDefault(x => _multiplicationDivisionSymbols.Contains(x))
				?? items.FirstOrDefault(x => _additionSubtractionSymbols.Contains(x));
		}
	}
}
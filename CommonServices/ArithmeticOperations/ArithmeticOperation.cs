namespace CommonServices.ArithmeticOperations
{
	public abstract class ArithmeticOperation
	{
		public abstract string Name { get; }
		public abstract List<string> Symbols { get; }

		public abstract double Calculate(double number1, double number2);

		public abstract int GetIndex(List<string> items, string targetSymbol);

		public int LeftAssociativityIndex(List<string> items, string targetSymbol)
		{
			return items.IndexOf(targetSymbol);
		}

		public int RightAssociativityIndex(List<string> items, string targetSymbol){
			return items.LastIndexOf(targetSymbol);
		}
	}
}
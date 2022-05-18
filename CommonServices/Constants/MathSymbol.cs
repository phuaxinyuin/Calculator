namespace CommonServices.Constants
{
	public class MathSymbol
	{
		public const string Addition = "+";
		public const string Subtraction = "-";
		public const string Multiplication1 = "*";
		public const string Multiplication2 = "x";
		public const string Division1 = "/";
		public const string Division2 = "÷";
		public const string Exponents = "^";
		public const string ParenthesesOpening = "(";
		public const string ParenthesesClosing = ")";

		public static readonly List<string> ParenthesesSymbols = new() { ParenthesesOpening, ParenthesesClosing };
		public static readonly List<string> MultiplicationDivisionSymbols = new() { Multiplication1, Multiplication2, Division1, Division2 };
		public static readonly List<string> AdditionSubtractionSymbols = new() { Addition, Subtraction };
		public static readonly List<string> ExponentsSymbols = new() { Exponents };
		public static readonly List<string> StartWithSymbols = new() { Subtraction, ParenthesesOpening };
	}
}

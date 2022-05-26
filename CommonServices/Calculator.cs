using CommonServices.ArithmeticOperations;
using CommonServices.Constants;
using System.Reflection;

namespace CommonServices
{
	public class Calculator
	{
		private static readonly string _separator = " ";
		private static readonly IEnumerable<ArithmeticOperation?> _arithmeticOperations = typeof(ArithmeticOperation)
																						.Assembly.GetTypes()
																						.Where(t => t.IsSubclassOf(typeof(ArithmeticOperation)) && !t.IsAbstract)
																						.Select(t => Activator.CreateInstance(t) as ArithmeticOperation);
		private static readonly IEnumerable<string?> _mathSymbols = typeof(MathSymbol)
																	.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
																	.Where(fi => fi.IsLiteral && !fi.IsInitOnly)
																	.Select(x => x.GetRawConstantValue() as string);
		private static readonly List<string> _exponentsSymbols = new Exponents().Symbols;
		private static readonly List<string> _multiplicationDivisionSymbols = new Multiplication().Symbols.Concat(new Division().Symbols).ToList();
		private static readonly List<string> _additionSubtractionSymbols = new Addition().Symbols.Concat(new Subtraction().Symbols).ToList();

		// Math Order of Operations - PEMDAS
		public static double Solve(string mathEquation)
		{
			SimplifyEquation(mathEquation, out List<string> mathEquationItems);

			while (mathEquationItems.Count > 1)
			{
				// step 1 : Get the first math equation that need to be resolved, this is to cater parentheses or brackets
				var targetMathEquationItems = GetTargetMathEquation(mathEquationItems, out int startIndex, out int endIndex);

				// step 2 : Calculate target math equation
				var result = Calculate(targetMathEquationItems);

				// step 3 : Replace the target math equation with the calculated result
				mathEquationItems.RemoveRange(startIndex, endIndex - startIndex + 1);
				mathEquationItems.Insert(startIndex, result);
			}

			return Convert.ToDouble(mathEquationItems.Single());
		}

		private static void SimplifyEquation(string mathEquation, out List<string> mathEquationItems)
		{
			if (string.IsNullOrWhiteSpace(mathEquation))
			{
				throw new Exception(ErrorMessage.InvalidInput);
			}

			mathEquationItems = mathEquation.Split(_separator).ToList();

			// Trim extra spaces
			mathEquationItems.RemoveAll(x => string.IsNullOrEmpty(x));

			var simplifiedEquationItems = new List<string>();
			for (int i = 0; i < mathEquationItems.Count; i++)
			{
				Validate(mathEquationItems, i);

				// Handle ( x ) ( y ) --> ( x ) * ( y )
				// Handle x ( y ) --> x * ( y )
				if (i > 0
					&& mathEquationItems[i] == MathSymbol.ParenthesesOpening
					&& (mathEquationItems[i - 1] == MathSymbol.ParenthesesClosing || double.TryParse(mathEquationItems[i - 1], out _)))
				{
					simplifiedEquationItems.Add(MathSymbol.Multiplication1);
				}

				simplifiedEquationItems.Add(mathEquationItems[i]);
			}

			mathEquationItems.Clear();
			mathEquationItems.AddRange(simplifiedEquationItems);
		}

		private static List<string> GetTargetMathEquation(List<string> mathEquationItems, out int startIndex, out int endIndex)
		{
			startIndex = 0;
			endIndex = mathEquationItems.Count - 1;
			var targetMathEquationItems = mathEquationItems.ToList();

			// Solve expressions in parentheses first
			if (mathEquationItems.Any(x => MathSymbol.ParenthesesSymbols.Contains(x)))
			{
				// Find innermost parentheses, proceed from left to right
				// ( x ) ( y ) OR ( x * ( y - z ) )
				endIndex = mathEquationItems.IndexOf(MathSymbol.ParenthesesClosing);
				startIndex = mathEquationItems.LastIndexOf(MathSymbol.ParenthesesOpening, endIndex);
				targetMathEquationItems = mathEquationItems.GetRange(startIndex + 1, endIndex - startIndex - 1);
			}

			return targetMathEquationItems;
		}

		private static string Calculate(List<string> mathEquationItems)
		{
			while (mathEquationItems.Count > 1)
			{
				var targetMathSymbol = mathEquationItems.LastOrDefault(x => _exponentsSymbols.Contains(x))
										?? mathEquationItems.FirstOrDefault(x => _multiplicationDivisionSymbols.Contains(x))
										?? mathEquationItems.FirstOrDefault(x => _additionSubtractionSymbols.Contains(x));

				if (targetMathSymbol != null)
				{
					var arithmeticOperation = _arithmeticOperations.SingleOrDefault(x => x?.Symbols.Contains(targetMathSymbol) ?? false);

					if (arithmeticOperation != null)
					{
						var symbolIndex = arithmeticOperation.GetIndex(mathEquationItems, targetMathSymbol);

						if (arithmeticOperation.Validate(mathEquationItems, symbolIndex, out double number1, out double number2))
						{
							double result = arithmeticOperation.Calculate(number1, number2);

							var targetIndex = symbolIndex - 1;
							var targetItemCount = 3;

							if (symbolIndex == 0)
							{
								targetIndex = 0;
								targetItemCount = 2;
							}

							mathEquationItems.RemoveRange(targetIndex, targetItemCount);
							mathEquationItems.Insert(targetIndex, result.ToString());
						}
						else
						{
							throw new Exception(ErrorMessage.InvalidInput);
						}
					}
					else
					{
						throw new Exception(ErrorMessage.ArithmeticOperationNotFound);
					}
				}
				else
				{
					throw new Exception(ErrorMessage.OperatorNotFound);
				}
			}

			return mathEquationItems.Single();
		}

		private static void Validate(List<string> mathEquationItems, int currentIndex)
		{
			var isValid = false;

			if (mathEquationItems != null && mathEquationItems.Any())
			{
				var startWithSymbols = new List<string> { MathSymbol.Subtraction, MathSymbol.ParenthesesOpening };
				var isSymbol = currentIndex == 0 ? startWithSymbols.Contains(mathEquationItems[currentIndex]) : _mathSymbols.Contains(mathEquationItems[currentIndex]);
				var isNumericalValue = double.TryParse(mathEquationItems[currentIndex], out _);
				isValid = mathEquationItems.Count == 1 ? isNumericalValue : (isSymbol || isNumericalValue);
			}

			if (!isValid)
			{
				throw new Exception(ErrorMessage.InvalidInput);
			}
		}
	}
}

﻿using CommonServices.ArithmeticOperations;
using CommonServices.Constants;
using System.Reflection;

namespace CommonServices
{
	public class Calculator
	{
		private static readonly string separator = " ";
		private static readonly IEnumerable<ArithmeticOperation?> arithmeticOperations = typeof(ArithmeticOperation)
																						.Assembly.GetTypes()
																						.Where(t => t.IsSubclassOf(typeof(ArithmeticOperation)) && !t.IsAbstract)
																						.Select(t => Activator.CreateInstance(t) as ArithmeticOperation);
		private static readonly IEnumerable<string?> mathSymbols = typeof(MathSymbol)
																	.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
																	.Where(fi => fi.IsLiteral && !fi.IsInitOnly)
																	.Select(x => x.GetRawConstantValue() as string);
		public static double Calculate(string mathEquation)
		{
			var mathEquationItems = mathEquation.Split(separator).ToList();

			// Trim extra spaces
			mathEquationItems.RemoveAll(x => string.IsNullOrEmpty(x));

			SimplifyEquationItems(mathEquationItems);

			// Math Order of Operations - PEMDAS
			while (mathEquationItems.Count > 1)
			{
				var firstIndex = 0;
				var lastIndex = mathEquationItems.Count - 1;
				var targetMathEquationItems = mathEquationItems.ToList();

				// Solve expressions in parentheses first
				if (mathEquationItems.Any(x => MathSymbol.ParenthesesSymbols.Contains(x)))
				{
					// Find innermost parentheses, proceed from left to right
					//( x ) ( y ) OR ( x * ( y - z ) )
					lastIndex = mathEquationItems.IndexOf(MathSymbol.ParenthesesClosing);
					firstIndex = mathEquationItems.LastIndexOf(MathSymbol.ParenthesesOpening, lastIndex);
					targetMathEquationItems = mathEquationItems.GetRange(firstIndex + 1, lastIndex - firstIndex - 1);
				}

				var result = RewriteEquation(targetMathEquationItems);
				mathEquationItems.RemoveRange(firstIndex, lastIndex - firstIndex + 1);
				mathEquationItems.Insert(firstIndex, result);
			}

			return Convert.ToDouble(mathEquationItems.First());
		}

		private static void SimplifyEquationItems(List<string> mathEquationItems)
		{
			if (mathEquationItems?.Any() ?? false)
			{
				// Handle - ( x )
				if (mathEquationItems[0] == MathSymbol.Subtraction)
				{
					mathEquationItems[0] = "-1";
					mathEquationItems.Insert(1, MathSymbol.Multiplication1);
				}

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
		}

		private static string RewriteEquation(List<string> mathEquationItems)
		{
			double result = 0;

			while (mathEquationItems.Count > 1)
			{
				var targetMathSymbol = mathEquationItems.LastOrDefault(x => MathSymbol.ExponentsSymbols.Contains(x))
										?? mathEquationItems.FirstOrDefault(x => MathSymbol.MultiplicationDivisionSymbols.Contains(x))
										?? mathEquationItems.FirstOrDefault(x => MathSymbol.AdditionSubtractionSymbols.Contains(x));

				if (targetMathSymbol != null)
				{
					var arithmeticOperation = arithmeticOperations.SingleOrDefault(x => x?.Symbols.Contains(targetMathSymbol) ?? false);

					if (arithmeticOperation != null)
					{
						var index = arithmeticOperation.GetIndex(mathEquationItems, targetMathSymbol);
						if (double.TryParse(mathEquationItems[index - 1], out double number1) && double.TryParse(mathEquationItems[index + 1], out double number2))
						{
							result = arithmeticOperation.Calculate(number1, number2);
							mathEquationItems.RemoveRange(index - 1, 3);
							mathEquationItems.Insert(index - 1, result.ToString());
						}
						else
						{
							throw new Exception(ErrorMessage.UnableToParseNumber);
						}
					}
					else
					{
						throw new Exception(ErrorMessage.ArithmeticOperationNotFound);
					}
				}
				else
				{
					throw new Exception(ErrorMessage.SymbolNotFound);
				}
			}

			return mathEquationItems.First();
		}

		private static void Validate(List<string> mathEquationItems, int currentIndex)
		{
			var isSymbol = currentIndex == 0 ? MathSymbol.StartWithSymbols.Contains(mathEquationItems[currentIndex]) : mathSymbols.Contains(mathEquationItems[currentIndex]);
			var isNumericalValue = !isSymbol ? double.TryParse(mathEquationItems[currentIndex], out _) : !isSymbol;
			var isValid = isSymbol || isNumericalValue;

			//if first index of ")" < first index of "("-- > Throw
			//if current == number & previous == number-- > throw

			if (!isValid)
			{
				throw new Exception(ErrorMessage.InvalidInput);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var x = new[] { 6, 7, 1 };
			var y = new[] { 5, 6, 7 };

			var convolution = new Convolution().CalculateConvolution(x, y);

			Console.WriteLine($"Signal x = {EnumerableToString(x)}");
			Console.WriteLine($"Signal y = {EnumerableToString(y)}");
			Console.WriteLine($"Convolution = {EnumerableToString(convolution)}");

			Console.ReadKey();
		}

		private static string EnumerableToString(IEnumerable<int> enumerable)
		{
			var sb = new StringBuilder();

			sb.Append("{");
			foreach (var item in enumerable)
			{
				sb.Append(item + ", ");
			}

			if (sb.Length > 2)
				sb.Remove(sb.Length - 2, 2);

			sb.Append("}");

			return sb.ToString();
		}
	}
}

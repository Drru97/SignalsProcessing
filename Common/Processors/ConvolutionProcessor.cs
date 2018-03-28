using Common.Models;
using System.Collections.Generic;
using System.Linq;

namespace Common.Processors
{
	public class ConvolutionProcessor : IConvolutionProcessor
	{
		public Signal CalculateConvolution(Signal lhs, Signal rhs)
		{
			var matrix = ConvolutionFunc(lhs.Readouts, rhs.Readouts);
			var readouts = CalculateReadouts(matrix);
			var resultSignal = new Signal(readouts.ToArray());

			return resultSignal;
		}

		private static double[,] ConvolutionFunc(IList<double> lhs, IList<double> rhs)
		{
			var sum = 0.0;
			var matrix = new double[lhs.Count, rhs.Count];

			for (var i = 0; i < lhs.Count; i++)
			{
				for (var j = 0; j < rhs.Count; j++)
				{
					sum = sum + lhs[i] * rhs[j];
					matrix[i, j] = lhs[i] * rhs[j];
				}
			}

			return matrix;
		}

		private static IEnumerable<double> CalculateReadouts(double[,] matrix)
		{
			var output = new double[matrix.GetLength(0) + matrix.GetLength(1) - 1];

			for (var i = 0; i < matrix.GetLength(0); i++)
			{
				for (var j = 0; j < matrix.GetLength(1); j++)
				{
					output[j + i] += matrix[i, j];
				}
			}

			return output;
		}
	}
}

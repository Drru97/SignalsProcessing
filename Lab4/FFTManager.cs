using Common.Models;
using MathNet.Numerics.IntegralTransforms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4
{
	public class FFTManager : IFFTManager
	{
		private readonly Signal _signal = new Signal(4, 2, 1, 4, 6, 3, 5, 2);

		public void GetFFT()
		{
			var result = CalculateFFT();

			Console.WriteLine($"Signal = {_signal}");

			Console.WriteLine($"FFT Real = {EnumerableToString(result.real)}");
			Console.WriteLine($"FFT Imaginary = {EnumerableToString(result.imaginary)}");
		}

		private (double[] real, double[] imaginary) CalculateFFT()
		{
			const int roundDecimals = 2;
			var real = GetRealPart();
			var imaginary = GetImaginaryPart();

			Fourier.Forward(real, imaginary);

			Round(real, roundDecimals);
			Round(imaginary, roundDecimals);

			return (real, imaginary);
		}

		private double[] GetRealPart()
		{
			var bufferLength = _signal.Count % 2 == 0 ? _signal.Count + 2 : _signal.Count + 1;
			var real = new double[bufferLength];

			for (var i = 0; i < _signal.Count; i++)
				real[i] = _signal[i];

			return real;
		}

		private double[] GetImaginaryPart()
		{
			var bufferLength = _signal.Count % 2 == 0 ? _signal.Count + 2 : _signal.Count + 1;
			var imaginary = new double[bufferLength];

			return imaginary;
		}

		private static void Round(IList<double> list, int decimals)
		{
			for (var i = 0; i < list.Count; i++)
			{
				var rounded = Math.Round(list[i], decimals);
				list[i] = rounded;
			}
		}

		private static string EnumerableToString(IEnumerable<double> enumerable)
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

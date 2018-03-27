using Accord.Math;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Lab3
{
	public class DFTManager : IDFTManager
	{
		private readonly Signal _signal = new Signal(5, 7, 3, 3, 0, 0, 15, 32, 13, 7);

		public void GetDFT()
		{
			const int roundDecimals = 2;

			var result = CalculateDFT();
			var real = result.real;

			Round(real, roundDecimals);
			Console.WriteLine($"Signal = {_signal}");

			Console.WriteLine($"DFT Real = {EnumerableToString(real)}");
		}

		private (double[] real, double[] imaginary) CalculateDFT()
		{
			var data = GetComplexData().ToArray();

			FourierTransform.DFT(data, FourierTransform.Direction.Forward);

			return (data.Re(), data.Im());
		}

		private IEnumerable<Complex> GetComplexData()
		{
			var real = GetRealPart();
			var imaginary = GetImaginaryPart();
			var length = Math.Max(real.Length, imaginary.Length);
			var complexData = new Complex[length];

			for (var i = 0; i < length; i++)
			{
				var complex = new Complex(real[i], imaginary[i]);
				complexData[i] = complex;
			}

			return complexData;
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

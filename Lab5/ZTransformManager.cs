using Common.Models;
using Common.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab5
{
	public class ZTransformManager : IZTransformManager
	{
		private readonly Signal _signal = new Signal(5, 7, 3, 3, 0, 0, 15, 32, 13, 7);
		private readonly IZTransform _zTransform;

		public ZTransformManager(IZTransform zTransform)
		{
			_zTransform = zTransform;
		}

		public void MakeZTransformation()
		{
			var result = CalculateZTransformation();

			Console.WriteLine($"Signal = {_signal}");
			Console.WriteLine($"Z Transform = {EnumerableToString(result)}");
		}

		private IEnumerable<float> CalculateZTransformation()
		{
			const int roundDecimals = 3;
			var data = GetComplexData().ToArray();
			var length = data.Length;
			var x = new float[length];

			_zTransform.RawInverse(data, x, 0, length, 2);

			Round(x, roundDecimals);

			return x;
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

		private static void Round(IList<float> list, int decimals)
		{
			for (var i = 0; i < list.Count; i++)
			{
				var rounded = (float)Math.Round(list[i], decimals);
				list[i] = rounded;
			}
		}

		private static string EnumerableToString(IEnumerable<float> enumerable)
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

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lab5
{
	/// <summary>
	/// Z-Transform is the discrete version of the Laplace transform.
	/// In their transformed form, the convolution of two distributions
	/// is just the point-wise product of their Z-transform coefficients.
	/// </summary>
	/// <remarks>
	/// The whole approach consists of projecting distributions into a
	/// fixed-size space where desirable operations such as convolutions
	/// and point-wise additions.
	/// 
	/// See https://en.wikipedia.org/wiki/Z-transform
	/// 
	/// The transformed space lies on the complex unit circle. The naive
	/// approach consists of taking nth roots - equi-spaced around the
	/// unit circle. However, this approach is numerically quite inefficient.
	/// 
	/// Tests performed July 2016 indicates that taking points concentrated
	/// according to (1 - cos(pi * x)) / 2 for x in [0 .. 1] works better.
	/// </remarks>
	public class ZTransform : IZTransform
	{
		// July 2016, The DFT (Discrete Fourier Transform) takes too many coefficient to be accurate.
		// Seems to be addressed in https://dl.acm.org/citation.cfm?id=2309873

		const int MaxStep = 16;

		/// <summary>
		/// By fixing the size of the transformed space upfront, it's possible to
		/// precompute a lot of coefficients. Precomputing those coefficients
		/// save a lot of time when multiple z-transforms are performed.
		/// </summary>
		private readonly int _zcount;

		/// <summary>Goes from 0 to 2 * Pi (roots on the unit circle).</summary>
		private readonly double[] _a;

		/// <summary>Arc length on unit circle.</summary>
		private readonly double[] _h;

		private readonly Complex[][] _s;

		public ZTransform(int zcount)
		{
			_zcount = zcount;

			_a = new double[zcount];
			_h = new double[zcount];
			for (int i = 0; i < _a.Length; i++)
			{
				//// equi-spaced roots on the unit circle
				//_a[i] = 2.0 * Math.PI * i / _a.Length;
				//_h[i] = 1.0 / _a.Length;

				_a[i] = (1 - Math.Cos(Math.PI * i / _a.Length)) * Math.PI;
				var b = (1 - Math.Cos(Math.PI * i / _a.Length)) / 2;
				_h[i] = (1 - Math.Cos(Math.PI * (i + 1) / _a.Length)) / 2 - b;
			}

			_s = new Complex[MaxStep][];
			for (int k = 0; k < MaxStep; k++)
			{
				var sk = _s[k] = new Complex[zcount];
				var powerstep = 1 << k;

				for (int i = 0; i < sk.Length; i++)
				{
					var a = _a[i];

					// geometric series: sum(r ^ k, k = 1 .. n) = r (1 - r ^ n) / (1 - r)
					Complex s;
					if (i == 0)
					{
						s = new Complex(powerstep, 0);
					}
					else
					{
						var ea = new Complex(-a);
						var num = new Complex(1, 0).Sub(new Complex(-a * (powerstep - 1)));
						var denum = new Complex(1, 0).Sub(ea);
						s = new Complex(1, 0).Add(ea.Mult(num).Div(denum));
					}

					sk[i] = s;
				}
			}
		}

		/// <remarks>
		/// The zero bucket of the input distribution 'p' is expected to start
		/// at 'index'. Then, the zero bucket is always of size 1, but all the buckets
		/// that follows have a size of Power2(step). The distribution has 'length'
		/// buckets; zero bucket being included in the count.
		/// </remarks>
		public void Transform(IReadOnlyList<float> p, int index, int length, int step, Complex[] z)
		{
			var powerstep = 1 << step;

			for (int k = 0; k < z.Length; k++)
			{
				// 'a' goes from 0 to 2 * Pi
				var a = _a[k];

				var zk = new Complex(0, 0);

				{ // zero-bucket always has a size of 1
					zk = zk.Add(new Complex(0).Mult(p[0 + index]));
				}

				if (step == 0) // buckets have a size of 1
				{
					for (int i = 1; i < length; i++)
					{
						var pi = p[i + index];
						zk = zk.Add(new Complex(-i * a).Mult(pi));
					}
				}
				else // buckest are larger than 1
				{
					var s = _s[step][k];
					for (int i = 1; i < length; i++)
					{
						var c = new Complex(-((i - 1) * powerstep + 1) * a);
						zk = zk.Add(c.Mult(s).Mult(p[i + index] / powerstep));
					}
				}

				z[k] = zk;
			}
		}

		/// <summary>
		/// Outputs in 'x' the bucketed random variable with 'length' and 'step' as parameters.
		/// </summary>
		/// <remarks>
		/// The 'inverse' seeks the minimal step where to quasi-totality of the weight of the 
		/// inverted distribution fit within 'maxLength'.
		/// 
		/// The 'initialStep' can be provided to force the method to skip the first steps.
		/// This argument is intended as a way to speed-up the inversion which represents the
		/// heaviest processing part in the transform/convolution/inverse cycle.
		/// </remarks>
		public void Inverse(Complex[] z, float[] x, int index, int maxLength,
			out int length, out int step, int initialStep = 0)
		{
			const float Tolerance = 0.001f;

			step = initialStep - 1;
			double sum;
			do
			{
				step++;
				RawInverse(z, x, index, maxLength, step);

				sum = 0.0;
				for (int i = 0; i < maxLength; i++)
				{
					var pi = x[i + index];
					if (pi > 0)
					{
						sum += pi;
					}

					if (sum >= 1 - Tolerance)
					{
						// and bounding at zero, to keep only positive probabilities
						for (int j = 0; j < i + 1; j++)
						{
							x[j + index] = x[j + index] > 0 ? x[j + index] : 0;
						}

						// normalizing to have a clean random variable
						var sum2 = (float)sum;
						for (int j = 0; j < i + 1; j++)
						{
							x[j + index] /= sum2;
						}

						length = i + 1;
						return;
					}
				}

				if (step == MaxStep - 1)
				{
					throw new NotSupportedException("Inverse z-transform fails");
				}

			} while (sum < 1 - Tolerance);

			throw new NotSupportedException(); // should not be reached
		}

		/// <summary>
		/// Raw inverse, not numerically adjusted. Returns small negative
		/// probabilities. Due to the cyclical nature of the calculations,
		/// if length is long enough, total may be greater than one too.
		/// </summary>
		/// <remarks>
		/// Write the resulting distribution in 'x' starting at 'index' for 'length' buckets,
		/// assumming Power2(step) for the bucket size (except for the zero bucket).
		/// </remarks>
		public void RawInverse(Complex[] z, float[] x, int index, int length, int step)
		{
			var powerstep = 1 << step;

			{ // bucket-zero is always unit

				var xi = new Complex(0, 0);
				for (int k = 0; k < z.Length; k++)
				{
					xi = xi.Add(z[k].Mult(_h[k]));
				}

				x[0 + index] = (float)xi.Re;
			}

			if (step == 0) // unit bucket case
			{
				for (int i = 1; i < length; i++)
				{
					var xi = new Complex(0, 0);
					for (int k = 0; k < z.Length; k++)
					{
						var w = new Complex(i * _a[k]);
						xi = xi.Add(z[k].Mult(w).Mult(_h[k]));
					}

					x[i + index] = (float)xi.Re;
				}

				return;
			}

			// fat bucket case
			for (int i = 1; (i - 1) / powerstep + 1 < length; i += powerstep)
			{
				var xi = new Complex(0, 0);
				for (int k = 0; k < z.Length; k++)
				{
					var s = _s[step][k].Conjugate();
					s = s.Mult(new Complex(i * _a[k]));
					xi = xi.Add(z[k].Mult(s).Mult(_h[k]));
				}

				x[(i - 1) / powerstep + 1 + index] = (float)xi.Re;
			}
		}
	}

	/// <summary> Basic complex number implementation. </summary>
	[DebuggerDisplay("{PrettyPrint}")]
	public struct Complex
	{
		public readonly double Re;
		public readonly double Im;

		private string PrettyPrint => Re + (Im > 0 ? "+i" + Im : "-i" + (-Im));

		public double Norm => Math.Sqrt(Re * Re + Im * Im);

		public bool IsNaN => double.IsNaN(Re) || double.IsNaN(Im);

		public Complex(double re, double im)
		{
			Re = re;
			Im = im;
		}

		public Complex(double omega)
		{
			Re = Math.Cos(omega);
			Im = Math.Sin(omega);
		}

		public Complex Add(Complex other)
		{
			return new Complex(Re + other.Re, Im + other.Im);
		}

		public Complex Sub(Complex other)
		{
			return new Complex(Re - other.Re, Im - other.Im);
		}

		public Complex Mult(double v)
		{
			return new Complex(Re * v, Im * v);
		}

		public Complex Mult(Complex other)
		{
			return new Complex(
				Re * other.Re - Im * other.Im,
				Im * other.Re + Re * other.Im);
		}

		public Complex Div(Complex other)
		{
			return new Complex(
				(Re * other.Re + Im * other.Im) / (other.Re * other.Re + other.Im * other.Im),
				(Im * other.Re - Re * other.Im) / (other.Re * other.Re + other.Im * other.Im));
		}

		public Complex Pow(double v)
		{
			var phi = Math.Atan2(Im, Re) * v;
			var r = Math.Pow(Math.Sqrt(Re * Re + Im * Im), v);
			return new Complex(r * Math.Cos(phi), r * Math.Sin(phi));
		}

		public Complex Conjugate()
		{
			return new Complex(Re, -Im);
		}
	}
}

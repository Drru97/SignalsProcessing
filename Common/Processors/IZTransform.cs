using System.Collections.Generic;

namespace Common.Processors
{
	public interface IZTransform
	{
		void Transform(IReadOnlyList<float> p, int index, int length, int step, Complex[] z);
		void Inverse(Complex[] z, float[] x, int index, int maxLength, out int length, out int step, int initialStep = 0);
		void RawInverse(Complex[] z, float[] x, int index, int length, int step);

	}
}

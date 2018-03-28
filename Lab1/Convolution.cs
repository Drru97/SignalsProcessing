namespace Lab1
{
	public class Convolution
	{
		public int[] CalculateConvolution(int[] a, int[] b)
		{
			var matrix = new int[a.Length, b.Length];
			var output = new int[a.Length + b.Length - 1];

			ZgortkaFunc(a, b, matrix);
			DivideMatrix(matrix, output, a.Length, b.Length);

			return output;
		}

		private static void ZgortkaFunc(int[] a, int[] b, int[,] matrix)
		{
			var sum = 0;
			for (var i = 0; i < a.Length; i++)
			{
				for (var j = 0; j < b.Length; j++)
				{
					sum = sum + a[i] * b[j];
					matrix[i, j] = a[i] * b[j];
				}
			}
		}

		private static void DivideMatrix(int[,] matrix, int[] output, int aLength, int bLength)
		{
			for (var i = 0; i < matrix.GetLength(0); i++)
			{
				for (var j = 0; j < matrix.GetLength(1); j++)
				{
					output[j + i] += matrix[i, j];
				}
			}
		}
	}
}

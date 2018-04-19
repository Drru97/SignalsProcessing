using System.Collections.Generic;

namespace Lab3
{
	public interface IDFTManager
	{
		void GetDFT();
        (double[] real, double[] imaginary) CalculateDFT();
        void SetNewSignal(IEnumerable<double> input);
    }
}

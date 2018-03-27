using Common.Models;
using MathNet.Numerics.Statistics;
using MathNet.Numerics.Statistics.Mcmc;
using System;

namespace Lab6
{
	public class CorrelationManager : ICorrelationManager
	{
		private readonly Signal _signal1 = new Signal(4, 2, -1, 3, -2, -6, -5, 4, 5);
		private readonly Signal _signal2 = new Signal(-4, 1, 3, 7, 4, -2, -8, -2, 1);

		public void PrintCorrelation()
		{
			Console.WriteLine($"Signal 1 = {_signal1}");
			Console.WriteLine($"Signal 2 = {_signal2}");

			var correlation = CalculateCorrelation();
			Console.WriteLine($"Correlation = {correlation}");
		}

		private double CalculateCorrelation()
		{
			return Correlation.Pearson(_signal1.Readouts, _signal2.Readouts);
		}

		public void PrintAutocorrelation()
		{
			const int lag = 2;

			var ac1 = CalculateAutocorrelation(_signal1, lag);
			var ac2 = CalculateAutocorrelation(_signal2, lag);

			Console.WriteLine($"Autocorrelation of signal 1 = {ac1}");
			Console.WriteLine($"Autocorrelation of signal 2 = {ac2}");
		}

		private static double CalculateAutocorrelation(Signal signal, int lag)
		{
			return MCMCDiagnostics.ACF(signal.Readouts, lag, x => x);
		}
	}
}

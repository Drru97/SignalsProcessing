using System;
using System.Linq;
using Common.Models;

namespace Common.Processors
{
	public class SignalProcessor : ISignalProcessor
	{
		public Signal Scale(Signal signal, double scaleCoefficient)
		{
			var scaledSignal = new Signal();

			foreach (var readout in signal.Readouts)
				scaledSignal.Add(readout * scaleCoefficient);

			return scaledSignal;
		}

		public Signal Reverse(Signal signal)
		{
			var readouts = signal.Readouts.Reverse();
			var reversedSignal = new Signal { Readouts = readouts.ToList() };

			return reversedSignal;
		}

		public Signal Shift(Signal signal, int shift)
		{
			var shiftedSignal = new Signal(signal.Count);

			for (var i = 0; i < signal.Count; i++)
				shiftedSignal[i] = signal[i - shift];

			return shiftedSignal;
		}

		public Signal Extension(Signal signal, int extensionCoefficient)
		{
			if (extensionCoefficient < 1)
				throw new ArgumentException("Value should be > 1", nameof(extensionCoefficient));

			var readoutsCount = signal.Count * extensionCoefficient;
			var extendedSignal = new Signal(readoutsCount);

			for (var i = 0; i < readoutsCount; i++)
				extendedSignal[i] = signal[extensionCoefficient * i];

			return extendedSignal;
		}

		public Signal Addition(Signal lhs, Signal rhs)
		{
			var maxSignalRedoutsCount = Math.Max(lhs.Count, rhs.Count);
			var signalsSum = new Signal(maxSignalRedoutsCount);

			for (var i = 0; i < maxSignalRedoutsCount; i++)
				signalsSum[i] = lhs[i] + rhs[i];

			return signalsSum;
		}

		public Signal Multiplication(Signal lhs, Signal rhs)
		{
			var maxSignalRedoutsCount = Math.Max(lhs.Count, rhs.Count);
			var signalsMultiplication = new Signal(maxSignalRedoutsCount);

			for (var i = 0; i < maxSignalRedoutsCount; i++)
				signalsMultiplication[i] = lhs[i] * rhs[i];

			return signalsMultiplication;
		}
	}
}

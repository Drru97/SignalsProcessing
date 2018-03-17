using Common.Models;

namespace Common.Processors
{
	public interface ISignalProcessor
	{
		Signal Scale(Signal signal, double scaleCoefficient);
		Signal Reverse(Signal signal);
		Signal Shift(Signal signal, int shift);
		Signal Extension(Signal signal, int extensionCoefficient);
		Signal Addition(Signal lhs, Signal rhs);
		Signal Multiplication(Signal lhs, Signal rhs);
	}
}

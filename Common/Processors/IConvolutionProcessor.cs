using Common.Models;

namespace Common.Processors
{
	public interface IConvolutionProcessor
	{
		Signal CalculateConvolution(Signal lhs, Signal rhs);
	}
}

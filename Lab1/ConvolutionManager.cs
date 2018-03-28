using Common.Models;
using Common.Processors;
using System;
using System.Collections.Generic;

namespace Lab1
{
	public class ConvolutionManager : IConvolutionManager
	{
		private readonly List<Signal> _signals = new List<Signal>
		{
			new Signal(6, 7, 1),
			new Signal(6, 4, 7, 9, 2, 3, 6, 4, 9),
			new Signal(2, 3, 6, 4, 2, 4, 8, 2),
			new Signal(5, 6, 7),
			new Signal(8, 2, 7, 3, 6, 4, 0),
			new Signal(1, 2, 8, 6, 4, 0, 8, 3, 7, 6, 4, 0)
		};
		private readonly IConvolutionProcessor _convolutionProcessor;

		public ConvolutionManager(IConvolutionProcessor convolutionProcessor)
		{
			_convolutionProcessor = convolutionProcessor;
		}

		public void Compute()
		{
			PrintSignals();
			PrintConvolution();
		}

		private Signal CalculateConvolutionBeetweenSignals(int index1, int index2)
		{
			return _convolutionProcessor.CalculateConvolution(_signals[index1], _signals[index2]);
		}

		private void PrintSignals()
		{
			for (var i = 0; i < _signals.Count; i++)
			{
				Console.WriteLine($"Signal {i} = {_signals[i]}");
			}
		}

		private void PrintConvolution()
		{
			Console.WriteLine($"Convolution beetween {0} and {3} = {CalculateConvolutionBeetweenSignals(0, 3)}");
			Console.WriteLine($"Convolution beetween {1} and {4} = {CalculateConvolutionBeetweenSignals(1, 4)}");
			Console.WriteLine($"Convolution beetween {2} and {5} = {CalculateConvolutionBeetweenSignals(2, 5)}");
		}
	}
}

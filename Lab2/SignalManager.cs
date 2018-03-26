using Common.Models;
using Common.Processors;
using System;
using System.Collections.Generic;

namespace Lab2
{
	public class SignalManager : ISignalManager
	{
		private readonly ISignalProcessor _signalProcessor;
		private readonly List<Signal> _signals = new List<Signal>
		{
			new Signal(9, 8, 7, 2, 1, 0),
			new Signal(5, 7, 6, 1, 0, 8, 7, 6),
			new Signal(7, 1, 3, 2, 9, 4, 7, 1),
			new Signal(8, 2, 9, 3, 0)
		};

		private const double ScaleCoefficient = 1.5;
		private const int Shift = 2;
		private const int ExtensionCoefficient = 3;


		public SignalManager(ISignalProcessor signalProcessor)
		{
			_signalProcessor = signalProcessor;
		}

		public void MakeMainOperations()
		{
			CalculateScale();
			CalculateReverse();
			CalculateShift();
			CalculateExtension();
		}

		private void CalculateScale()
		{
			foreach (var signal in _signals)
			{
				var result = _signalProcessor.Scale(signal, ScaleCoefficient);
				Console.WriteLine($"Scale {signal} on {ScaleCoefficient} = {result}");
			}
		}

		private void CalculateReverse()
		{
			foreach (var signal in _signals)
			{
				var result = _signalProcessor.Reverse(signal);
				Console.WriteLine($"Reverse {signal} = {result}");
			}
		}

		private void CalculateShift()
		{
			foreach (var signal in _signals)
			{
				var result = _signalProcessor.Shift(signal, Shift);
				Console.WriteLine($"Shift {signal} on {Shift} = {result}");
			}
		}

		private void CalculateExtension()
		{
			foreach (var signal in _signals)
			{
				var result = _signalProcessor.Extension(signal, ExtensionCoefficient);
				Console.WriteLine($"Extension {signal} on {ExtensionCoefficient} = {result}");
			}
		}
	}
}

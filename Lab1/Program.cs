using Microsoft.Extensions.DependencyInjection;
using System;
using Common.Processors;

namespace Lab1
{
	internal class Program
	{
		private static IServiceProvider _serviceProvider;

		private static void Main(string[] args)
		{
			ConfigureDependencies();

			var manager = _serviceProvider.GetService<IConvolutionManager>();
			manager.Compute();

			Console.ReadKey();
		}

		private static void ConfigureDependencies()
		{
			var services = new ServiceCollection();

			services
				.AddTransient<IConvolutionProcessor, ConvolutionProcessor>()
				.AddTransient<IConvolutionManager, ConvolutionManager>();

			_serviceProvider = services.BuildServiceProvider();
		}
	}
}

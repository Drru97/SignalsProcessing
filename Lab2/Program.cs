using Common.Processors;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lab2
{
	internal class Program
	{
		private static IServiceProvider _serviceProvider;

		internal static void Main(string[] args)
		{
			ConfigureDependencies();

			var manager = _serviceProvider.GetService<ISignalManager>();
			manager.MakeMainOperations();

			Console.ReadKey();
		}

		private static void ConfigureDependencies()
		{
			var services = new ServiceCollection();

			services
				.AddTransient<ISignalProcessor, SignalProcessor>()
				.AddTransient<ISignalManager, SignalManager>();

			_serviceProvider = services.BuildServiceProvider();
		}
	}
}

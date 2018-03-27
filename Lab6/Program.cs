using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lab6
{
	internal class Program
	{
		private static IServiceProvider _serviceProvider;

		private static void Main(string[] args)
		{
			ConfigureDependencies();

			var manager = _serviceProvider.GetService<ICorrelationManager>();
			manager.PrintCorrelation();
			manager.PrintAutocorrelation();

			Console.ReadKey();
		}

		private static void ConfigureDependencies()
		{
			var services = new ServiceCollection();

			services
				.AddTransient<ICorrelationManager, CorrelationManager>();

			_serviceProvider = services.BuildServiceProvider();
		}
	}
}

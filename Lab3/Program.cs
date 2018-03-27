using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lab3
{
	internal class Program
	{
		private static IServiceProvider _serviceProvider;

		private static void Main(string[] args)
		{
			ConfigureDependencies();

			var manager = _serviceProvider.GetService<IDFTManager>();
			manager.GetDFT();

			Console.ReadKey();
		}

		private static void ConfigureDependencies()
		{
			var services = new ServiceCollection();

			services
				.AddTransient<IDFTManager, DFTManager>();

			_serviceProvider = services.BuildServiceProvider();
		}
	}
}

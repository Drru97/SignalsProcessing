using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lab4
{
	internal class Program
	{
		private static IServiceProvider _serviceProvider;

		internal static void Main(string[] args)
		{
			ConfigureDependencies();

			var manager = _serviceProvider.GetService<IFFTManager>();
			manager.GetFFT();

			Console.ReadKey();
		}

		private static void ConfigureDependencies()
		{
			var services = new ServiceCollection();

			services
				.AddTransient<IFFTManager, FFTManager>();

			_serviceProvider = services.BuildServiceProvider();
		}
	}
}

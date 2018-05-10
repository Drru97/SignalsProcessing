using Common.Processors;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lab5
{
	internal class Program
	{
		private static IServiceProvider _serviceProvider;

		private static void Main(string[] args)
		{
			ConfigureDependencies();

			var manager = _serviceProvider.GetService<IZTransformManager>();
			manager.MakeZTransformation();

			Console.ReadKey();
		}

		private static void ConfigureDependencies()
		{
			var services = new ServiceCollection();

			services
				.AddTransient<IZTransform, ZTransform>(svc => new ZTransform(5))
				.AddTransient<IZTransformManager, ZTransformManager>();

			_serviceProvider = services.BuildServiceProvider();
		}
	}
}

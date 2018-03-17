using System;
using System.Collections.Generic;
using Common.Models;
using Common.Processors;
using NUnit.Framework;

namespace Tests.Processors
{
	[TestFixture]
	public class SignalProcessorTests
	{
		private Signal _signal1;
		private Signal _signal2;

		private ISignalProcessor _subject;

		[SetUp]
		public void SetUp()
		{
			_signal1 = new Signal { Readouts = new List<double> { 1.5, 3, 5.5 } };
			_signal2 = new Signal { Readouts = new List<double> { 1.5, -0.5, 2.5, -1 } };

			_subject = new SignalProcessor();
		}

		[Test]
		[TestCase(-1.5, -2.25, -4.5, -8.25)]
		[TestCase(0, 0, 0, 0)]
		[TestCase(2.5, 3.75, 7.5, 13.75)]
		public void Scale(double scaleCoefficient, params double[] readouts)
		{
			var result = _subject.Scale(_signal1, scaleCoefficient);

			Assert.That(result.Readouts, Is.Not.Null);
			Assert.That(result.Readouts[0], Is.EqualTo(readouts[0]));
			Assert.That(result.Readouts[1], Is.EqualTo(readouts[1]));
			Assert.That(result.Readouts[2], Is.EqualTo(readouts[2]));
		}

		[Test]
		public void Reverse()
		{
			const double expectedReadout1 = 5.5;
			const double expectedReadout2 = 3;
			const double expectedReadout3 = 1.5;

			var result = _subject.Reverse(_signal1);

			Assert.That(result.Readouts, Is.Not.Null);
			Assert.That(result.Readouts[0], Is.EqualTo(expectedReadout1));
			Assert.That(result.Readouts[1], Is.EqualTo(expectedReadout2));
			Assert.That(result.Readouts[2], Is.EqualTo(expectedReadout3));
		}

		[Test]
		[TestCase(-4, 0, 0, 0)]
		[TestCase(-1, 3, 5.5, 0)]
		[TestCase(0, 1.5, 3, 5.5)]
		[TestCase(1, 0, 1.5, 3)]
		[TestCase(4, 0, 0, 0)]
		public void Shift(int shift, params double[] readouts)
		{
			var result = _subject.Shift(_signal1, shift);

			Assert.That(result.Readouts, Is.Not.Null);
			Assert.That(result.Readouts[0], Is.EqualTo(readouts[0]));
			Assert.That(result.Readouts[1], Is.EqualTo(readouts[1]));
			Assert.That(result.Readouts[2], Is.EqualTo(readouts[2]));
		}

		[Test]
		[TestCase(1, 1.5, -0.5, 2.5, -1)]
		[TestCase(4, 1.5, 0, 0, 0)]
		public void Extension(int extensionCoefficient, params double[] readouts)
		{
			var expectedReadoutsCount = extensionCoefficient * _signal2.Count;

			var result = _subject.Extension(_signal2, extensionCoefficient);

			Assert.That(result.Readouts, Is.Not.Null);
			Assert.That(result.Readouts.Count, Is.EqualTo(expectedReadoutsCount));
			Assert.That(result.Readouts[0], Is.EqualTo(readouts[0]));
			Assert.That(result.Readouts[1], Is.EqualTo(readouts[1]));
			Assert.That(result.Readouts[2], Is.EqualTo(readouts[2]));
			Assert.That(result.Readouts[3], Is.EqualTo(readouts[3]));
		}

		[Test]
		public void Extension_Throws_ArgumentException_When_Less_Than_1_Coefficient_Passed()
		{
			const int extensionCoefficient = 0;

			Assert.Throws<ArgumentException>(() => _subject.Extension(_signal2, extensionCoefficient));
		}

		[Test]
		public void Addition()
		{
			const int expectedReadoutsCount = 4;
			var expectedReadouts = new List<double> { 3, 2.5, 8, -1 };

			var result = _subject.Addition(_signal1, _signal2);

			Assert.That(result.Readouts, Is.Not.Null);
			Assert.That(result.Readouts.Count, Is.EqualTo(expectedReadoutsCount));
			Assert.That(result.Readouts, Is.EqualTo(expectedReadouts));
		}

		[Test]
		public void Multiplication()
		{
			const int expectedReadoutsCount = 4;
			var expectedReadouts = new List<double> { 2.25, -1.5, 13.75, 0 };

			var result = _subject.Multiplication(_signal1, _signal2);

			Assert.That(result.Readouts, Is.Not.Null);
			Assert.That(result.Readouts.Count, Is.EqualTo(expectedReadoutsCount));
			Assert.That(result.Readouts, Is.EqualTo(expectedReadouts));
		}
	}
}

using System.Collections.Generic;
using System.Linq;
using Common.Models;
using NUnit.Framework;

namespace Tests.Models
{
	[TestFixture]
	public class SignalTests
	{
		[Test]
		public void Constructor_Creates_List_Of_Readouts_When_Capacity_Not_Specified()
		{
			var subject = new Signal();

			var result = subject.Readouts;

			Assert.That(result, Is.Not.Null);
			Assert.That(result, Is.TypeOf<List<double>>());
		}

		[Test]
		public void Constructor_Creates_List_Of_Readouts_With_Specified_Capacity_When_Capacity_Is_Set()
		{
			const int expectedCapacity = 69;

			var subject = new Signal(expectedCapacity);
			var result = subject.Readouts;

			Assert.That(result, Is.Not.Null);
			Assert.That(((List<double>)result).Capacity, Is.EqualTo(expectedCapacity));
		}

		[Test]
		public void Constructor_Creates_List_Of_Readouts_Based_On_Another_Signal()
		{
			const int expectedReadoutsCount = 3;
			var expectedReadouts = new List<double> { 3, 4, 5 };
			var expectedSignal = new Signal { Readouts = expectedReadouts };

			var subject = new Signal(expectedSignal);
			var result = subject.Readouts;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Count(), Is.EqualTo(expectedReadoutsCount));
			Assert.That(result, Is.EqualTo(expectedReadouts));
		}

		[Test]
		public void Readouts_Sets_Readout_List()
		{
			const int expectedReadoutsCount = 3;
			var expectedReadouts = new List<double> { 3, 4, 5 };

			var subject = new Signal { Readouts = expectedReadouts };
			var result = subject.Readouts;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Count(), Is.EqualTo(expectedReadoutsCount));
			Assert.That(result, Is.EqualTo(expectedReadouts));
		}

		[Test]
		public void Readouts_Not_Set_Readout_List_When_Passed_Null()
		{
			const int expectedReadoutsCount = 3;
			var expectedReadouts = new List<double> { 3, 4, 5 };
			var subject = new Signal { Readouts = expectedReadouts };

			subject.Readouts = null;
			var result = subject.Readouts;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Count(), Is.EqualTo(expectedReadoutsCount));
			Assert.That(result, Is.EqualTo(expectedReadouts));
		}

		[Test]
		public void Count_Returns_Readout_Count()
		{
			const int expectedReadoutsCount = 3;
			var expectedReadouts = new List<double> { 3, 4, 5 };

			var subject = new Signal { Readouts = expectedReadouts };
			var result = subject.Count;

			Assert.That(result, Is.EqualTo(expectedReadoutsCount));
		}

		[Test]
		public void Indexer_Returns_Readout_When_Exists()
		{
			const int index = 1;
			const double expectedReadout = 4;
			var expectedReadouts = new List<double> { 3, 4, 5 };

			var subject = new Signal { Readouts = expectedReadouts };
			var result = subject[index];

			Assert.That(result, Is.EqualTo(expectedReadout));
		}

		[Test]
		public void Indexer_Returns_0_When_Not_Exists()
		{
			const int index = 3;
			const double expectedReadout = 0;
			var expectedReadouts = new List<double> { 3, 4, 5 };

			var subject = new Signal { Readouts = expectedReadouts };
			var result = subject[index];

			Assert.That(result, Is.EqualTo(expectedReadout));
		}

		[Test]
		public void Add_Inserts_Readout_Into_List()
		{
			const int expectedReadoutsCount = 4;
			const double expectedReadout = 3.14;
			var readouts = new List<double> { 3, 4, 5 };

			var subject = new Signal { Readouts = readouts };
			subject.Add(expectedReadout);
			var result = subject.Readouts;

			Assert.That(result.Count(), Is.EqualTo(expectedReadoutsCount));
			Assert.That(result.Last(), Is.EqualTo(expectedReadout));
		}

		[Test]
		public void Equals_Returns_False_When_Null_Passed()
		{
			var subject = new Signal();

			var result = subject.Equals(null);

			Assert.That(result, Is.False);
		}

		[Test]
		public void Equals_Returns_True_If_Refenence_To_The_Same_Object_Passed()
		{
			var subject = new Signal();
			var secondReference = subject;

			var result = subject.Equals(secondReference);

			Assert.That(result, Is.True);
		}

		[Test]
		public void Equals_Returns_False_When_Readouts_List_Is_Not_Equal()
		{
			var readouts1 = new List<double> { 1, 2, 3 };
			var readouts2 = new List<double> { 1, 2, 3, 4 };
			var subject = new Signal { Readouts = readouts1 };
			var secondSignal = new Signal { Readouts = readouts2 };

			var result = subject.Equals(secondSignal);

			Assert.That(result, Is.False);
		}

		[Test]
		public void Equals_Returns_True_When_Readouts_List_Is_Equal()
		{
			var readouts1 = new List<double> { 1, 2, 3 };
			var readouts2 = new List<double> { 1, 2, 3 };
			var subject = new Signal { Readouts = readouts1 };
			var secondSignal = new Signal { Readouts = readouts2 };

			var result = subject.Equals(secondSignal);

			Assert.That(result, Is.True);
		}

		[Test]
		public void Equals_Returns_False_When_Object_Passed_Is_Not_The_Same_Type()
		{
			var subject = new Signal();
			var obj = new object();

			var result = subject.Equals(obj);

			Assert.That(result, Is.False);
		}
	}
}

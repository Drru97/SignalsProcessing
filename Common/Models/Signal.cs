using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Models
{
	public class Signal : IEquatable<Signal>
	{
		private IList<double> _readouts;

		public Signal()
		{
			_readouts = new List<double>();
		}

		public Signal(int capacity)
		{
			_readouts = new List<double>(capacity);
		}

		public Signal(Signal signal) : this(signal.Count)
		{
			((List<double>)_readouts).AddRange(signal.Readouts);
		}

		public IList<double> Readouts
		{
			get => _readouts;
			set
			{
				if (value != null)
					_readouts = value;
			}
		}

		public int Count => Readouts.Count;

		public double this[int index]
		{
			get => _readouts.ElementAtOrDefault(index);
			set => Readouts.Insert(index, value);
		}

		public void Add(double readout)
		{
			Readouts.Add(readout);
		}

		public bool Equals(Signal other)
		{
			if (ReferenceEquals(null, other))
				return false;

			if (ReferenceEquals(this, other))
				return true;

			return _readouts.SequenceEqual(other._readouts);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (obj.GetType() != GetType())
				return false;

			return Equals((Signal)obj);
		}

		public override int GetHashCode()
		{
			return _readouts.GetHashCode();
		}
	}
}

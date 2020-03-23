namespace BTC
{
	public class BTCNumber : IBTCData
	{
		private double _value;
		public double Value { get { return this._value; } }
		public BTCNumber(double value)
		{
			this._value = value;
		}

		public int ToInteger()
		{
			return ((int) this._value);
		}
		public double ToDouble()
		{
			return this._value;
		}

		public string Encode()
		{
			return this._value.ToString();
		}
		public string Encode(int separators)
		{
			return this._value.ToString();
		}
	}
}
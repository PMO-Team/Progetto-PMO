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
			return string.Join(".", this._value.ToString().Split(','));
		}
		public string Encode(int separators)
		{
			return string.Join(".", this._value.ToString().Split(','));
		}
	}
}
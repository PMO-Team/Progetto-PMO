namespace BTC
{
	public class BTCString : IBTCData
	{
		private string _value;
		public string Value { get; }
		public BTCString(string value)
		{
			this._value = value;
		}

		public string Encode()
		{
			return ("\"" + this._value + "\"");
		}

		public string Encode(int separators)
		{
			return ("\"" + this._value + "\"");
		}
	}
}
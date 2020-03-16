namespace BTC
{
	public class BTCBool : IBTCData
	{
		private bool _value;
		public bool Value { get; }
		public BTCBool(bool value)
		{
			this._value = value;
		}

		public string Encode()
		{
			return ((this._value) ? "true" : "false");
		}

		public string Encode(int separators)
		{
			return ((this._value) ? "true" : "false");
		}
	}
}
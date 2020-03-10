namespace BTC
{
	public class BTCElement<T> : IBTCData
	{
		private T value;
		public T Value { get; }

		public BTCElement(T value)
		{
			this.Value = value;
		}

		public string Encode()
		{
			return "";
		}
	}
}
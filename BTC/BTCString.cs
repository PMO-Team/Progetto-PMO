namespace BTC
{
	public class BTCString : IBTCData
	{
		private string _value;
		public string Value { get; }
		public BTCString(string value)
		{
			for (int i = 0; i < value.Length; i++)
				switch (value[i])
				{
					case '\r':  
					case '\n': 
					case '\t': 
					case '\"': 
					//case '\':
					case '\\': 
					case '\f': 
					case '\a': 
					case '\v': 
					case '\b': 
						throw new BTCSyntaxErrorException("Syntax Error: invalid string >> " + value);
					default:
						break;
				}
			
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
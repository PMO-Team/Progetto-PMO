namespace BTC
{
	public class BTCString : IBTCData
	{
		private string _value;
		public string Value { get { return this._value; } }
		public BTCString(string value)
		{
			this._value = value;
		}

		public string Encode()
		{
			string ret = "\"";

			for (int i = 0; i < this._value.Length; i++)
			{
				switch (this._value[i])
					{
						case '\n':
							ret += "\\n";
							break;
						case '\'':
							ret += "\\\'";
							break;
						case '\"':
							ret += "\\\"";
							break;
						case '\\':
							ret += "\\\\";
							break;
						case '\r':
							ret += "\\r";
							break;
						case '\t':
							ret += "\\t";
							break;
						default:
							ret += this._value[i];
							break;
					}
			}
			
			ret += '\"';

			return ret;
		}

		public string Encode(int separators)
		{
			string ret = "\"";

			for (int i = 0; i < this._value.Length; i++)
			{
				switch (this._value[i])
					{
						case '\n':
							ret += "\\n";
							break;
						case '\'':
							ret += "\\\'";
							break;
						case '\"':
							ret += "\\\"";
							break;
						case '\\':
							ret += "\\\\";
							break;
						case '\r':
							ret += "\\r";
							break;
						case '\t':
							ret += "\\t";
							break;
						default:
							ret += this._value[i];
							break;
					}
			}
			
			ret += '\"';

			return ret;
		}
	}
}
namespace BTC
{
	public class BTCString : IBTCData
	{
		private string value;
		public string Value { get { return this.value; } }
		public BTCString(string value)
		{
			this.value = value;
		}

		public string Encode()
		{
			string ret = "\"";

			for (int i = 0; i < this.value.Length; i++)
			{
				switch (this.value[i])
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
							ret += this.value[i];
							break;
					}
			}
			
			ret += '\"';

			return ret;
		}

		public string Encode(int separators)
		{
			return ("\"" + this.value + "\"");
		}
	}
}
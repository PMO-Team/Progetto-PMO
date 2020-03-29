namespace BTC
{
	/**
	 * @class			BTCString
	 * @implements		IBTCData
	 *
	 * @brief			Basic class for rappresenting a string value (Element and/or Item)
	 */
	public class BTCString : IBTCData
	{
		private string _value;
		/**
		 * @property	Value
		 *
		 * @brief		Getter for the value of the object
		 */
		public string Value { get { return this._value; } }
		/**
		 * @fn			BTCString(string value)
		 * @param		value	The real value of the BTC Data Element
		 *
		 * @brief		Constructor
		 */
		public BTCString(string value)
		{
			this._value = value;
		}

		/**
		 * @todo	COMMENT ALGORITHMS
		 */
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
			return this.Encode();
		}
	}
}
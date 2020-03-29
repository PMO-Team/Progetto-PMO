namespace BTC
{
	/**
	 * @class			BTCBool
	 * @implements		IBTCData
	 *
	 * @brief			Basic class for rappresenting a boolean value (Element and/or Item)
	 */
	public class BTCBool : IBTCData
	{
		private bool _value;
		/**
		 * @property	Value
		 *
		 * @brief		Getter for the value of the object
		 */
		public bool Value { get { return this._value; } }
		/**
		 * @fn			BTCBool(bool value)
		 * @param		value	The real value of the BTC Data Element
		 *
		 * @brief		Constructor
		 */
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
			return this.Encode();
		}
	}
}
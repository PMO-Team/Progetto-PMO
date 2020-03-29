namespace BTC
{
	/**
	 * @class			BTCNumber
	 * @implements		IBTCData
	 *
	 * @brief			Basic class for rappresenting a numeric value (Element and/or Item)
	 */
	public class BTCNumber : IBTCData
	{
		private double _value;
		/**
		 * @property	Value
		 *
		 * @brief		Getter for the value of the object.
		 * @details		It behaves like ToDouble() method
		 */
		public double Value { get { return this._value; } }
		/**
		 * @fn			BTCNumber(double value)
		 * @param		value	The real value of the BTC Data Element
		 *
		 * @brief		Constructor
		 */
		public BTCNumber(double value)
		{
			this._value = value;
		}

		/**
		 * @fn			ToInteger()
		 * @return		The numeric value as integer
		 */
		public int ToInteger()
		{
			return ((int) this._value);
		}
		/**
		* @fn			ToDouble()
		* @return		The numeric value as decimal (double)
		*/
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
			return this.Encode();
		}
	}
}
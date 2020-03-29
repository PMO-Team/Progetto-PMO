namespace BTC
{
	/**
	 * @interface	IBTCData
	 * @brief		Data Tree Component Interface
	 *
	 * @details		It exposes the two main methods for encode the BTC Data:
	 * 					- Encode() for the compat format
	 * 					- Encode(int) for the pretty format 
	 */
	public interface IBTCData
	{
		/**
		 * @fn			Encode()
		 * @return		Encoded string of the data value
		 * @brief		This method return the data value encoded in a compact way
		 */
		string Encode();
		/**
		 * @fn			Encode(int separators)
		 * @return		Encoded string of the data value
		 * @brief		This method return the data value encoded in a pretty way
		 */
		string Encode(int separators);
	}
}
namespace BTC
{
	/**
	 * @class			BTCSyntaxErrorException
	 * @extends			System.Exception
	 *
	 * @brief			Class provided to detect SyntaxErrors in file
	 */
	public class BTCSyntaxErrorException : System.Exception
	{
		/**
		 * @fn			BTCSyntaxErrorException()
		 * 
		 * @brief		Default Constructor
		 */
		public BTCSyntaxErrorException() {}
		/**
		 * @fn			BTCSyntaxErrorException(string message)
		 * 
		 * @brief		Use the Conctructor of the Superclass 
		 */
		public BTCSyntaxErrorException(string message) : base(message) {}
	}
}
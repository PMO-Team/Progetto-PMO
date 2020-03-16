using System;
using System.Runtime.Serialization;

namespace BTC
{
	public class BTCSyntaxErrorException : System.Exception
	{
		public BTCSyntaxErrorException()
		{
		}

		public BTCSyntaxErrorException(string message) : base(message)
		{
		}
	}
}
using System.Collections.Generic;

namespace BTC
{
	public class BTCList : List<IBTCData>, IBTCData
	{
		public string Encode()
		{
			return "";
		}
	}
}
using System.Collections.Generic;

namespace BTC
{
	public class BTCList : List<IBTCData>, IBTCData
	{
		public void Add(BTCString i)
		{
			this.Add(i);
		}
		public void Add(BTCNumber i)
		{
			this.Add(i);
		}
		public void Add(BTCBool i)
		{
			this.Add(i);
		}
		public void Add(BTCObject i)
		{
			this.Add(i);
		}
		public void Add(BTCList i)
		{
			this.Add(i);
		}
		public string Encode()
		{
			string result = "[";

			for (int i = 0; i < this.Count; i++)
			{
				result += this[i].Encode();
				if (i != (this.Count - 1))
					result += "|"; 
			}

			result += "]";

			return result;
		}
		public string Encode(int separators)
		{
			string result = "[\r\n";
			string sep = "";
			for (var i = 0; i < (separators - 1); i++)
				sep += "\t";

			for (int i = 0; i < this.Count; i++)
			{
				result += sep + "\t";
				result += this[i].Encode(separators++);
				if (i != (this.Count - 1))
					result += " |";
				result += "\r\n";
			}

			result += sep + "]";

			return result;
		}
	}
}
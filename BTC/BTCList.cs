using System.Collections.Generic;

namespace BTC
{
	public class BTCList : IBTCData
	{
		private List<IBTCData> elements;

		public BTCList()
		{
			this.elements = new List<IBTCData>();
		}

		public void Add(BTCString i)
		{
			this.elements.Add(i);
		}
		public void Add(BTCNumber i)
		{
			this.elements.Add(i);
		}
		public void Add(BTCBool i)
		{
			this.elements.Add(i);
		}
		public void Add(BTCObject i)
		{
			this.elements.Add(i);
		}
		public void Add(BTCList i)
		{
			this.elements.Add(i);
		}
		
		public void RemoveAt(int index)
		{
			this.elements.RemoveAt(index);
		}
		public int Count()
		{
			return this.elements.Count;
		}
		public IBTCData At(int index)
		{
			return this.elements[index];
		}

		public string Encode()
		{
			string result = "[";

			for (int i = 0; i < this.elements.Count; i++)
			{
				result += this.elements[i].Encode();
				if (i != (this.elements.Count - 1))
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

			for (int i = 0; i < this.elements.Count; i++)
			{
				result += sep + "\t";
				result += this.elements[i].Encode(separators++);
				if (i != (this.elements.Count - 1))
					result += " |";
				result += "\r\n";
			}

			result += sep + "]";

			return result;
		}
	}
}
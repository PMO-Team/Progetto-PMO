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
					result += ","; 
			}

			result += "]";

			return result;
		}
		public string Encode(int separators)
		{
			string result = "[\r\n";
			string sep = "";
			string closingSep = "";
			
			// Set-up the separotor string
			for (int i = 0; i < separators; i++)
				sep += "\t";
			for (int i = 1; i < separators; i++)
				closingSep += "\t";

			for (int i = 0; i < this.elements.Count; i++)
			{
				result += sep;
				result += this.elements[i].Encode(separators + 1);
				if (i != (this.elements.Count - 1))
					result += " ,";
				result += "\r\n";
			}

			result += closingSep + "]";

			return result;
		}
	}
}
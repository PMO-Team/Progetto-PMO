using System.Collections.Generic;

namespace BTC
{
	public class BTCObject : IBTCData
	{
		private Dictionary<string, IBTCData> elements;

		public BTCObject()
		{
			this.elements = new Dictionary<string, IBTCData>();
		}

		public void Add(string tag, BTCNumber value)
		{
			this.elements.Add(tag, value);
		}
		public void Add(string tag, BTCString value)
		{
			this.elements.Add(tag, value);
		}
		public void Add(string tag, BTCBool value)
		{
			this.elements.Add(tag, value);
		}
		public void Add(string tag, BTCObject value)
		{
			this.elements.Add(tag, value);
		}
		public void Add(string tag, BTCList value)
		{
			this.elements.Add(tag, value);
		}
		
		public void Remove(string tag)
		{
			this.elements.Remove(tag);
		}
		public int Count()
		{
			return this.elements.Count;
		}

		// Guarantee exception safe
		public IBTCData Tag(string tag)
		{
			IBTCData val;

			try
			{
				val = this.elements[tag];
			}
			catch (KeyNotFoundException)
			{
				val = null;
			}

			return val;
		}
		public List<string> Tags()
		{
			List<string> result = new List<string>();

			foreach (var item in this.elements)
			{
				result.Add(item.Key);
			}

			return result;
		}

		public string Encode()
		{
			string result = "(";
			
			foreach (var item in this.elements)
			{
				result += "@" + item.Key + ">";
				result += item.Value.Encode();
			}

			result += ")";

			return result;
		}
		public string Encode(int separators)
		{
			string result = "(\r\n";
			string sep = "";
			
			// Set-up the separotor string
			for (int i = 0; i < separators; i++)
				sep += "\t";

			foreach (var item in this.elements)
			{
				result += sep;
				result += "@" + item.Key + " > ";
				result += item.Value.Encode(separators++);
				result += "\r\n";
			}

			result += sep + ")";

			return result;
		}
	}
}
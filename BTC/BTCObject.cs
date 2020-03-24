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

		// If TAG already exists, the new value will be ignored
		public void Add(string tag, BTCNumber value)
		{
			try
			{
				this.elements.Add(tag, value);
			}
			catch (System.ArgumentException) {}
		}
		public void Add(string tag, BTCString value)
		{
			try
			{
				this.elements.Add(tag, value);
			}
			catch (System.ArgumentException) {}
		}
		public void Add(string tag, BTCBool value)
		{
			try
			{
				this.elements.Add(tag, value);
			}
			catch (System.ArgumentException) {}
		}
		public void Add(string tag, BTCObject value)
		{
			try
			{
				this.elements.Add(tag, value);
			}
			catch (System.ArgumentException) {}
		}
		public void Add(string tag, BTCList value)
		{
			try
			{
				this.elements.Add(tag, value);
			}
			catch (System.ArgumentException) {}
		}
		
		// Remove the element with the corresponding TAG
		public void Remove(string tag)
		{
			this.elements.Remove(tag);
		}
		// Return the numbers of elements (this element only)
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
			string closingSep = "";
			
			// Set-up the separotor string
			for (int i = 0; i < separators; i++)
				sep += "\t";
			for (int i = 1; i < separators; i++)
				closingSep += "\t";

			foreach (var item in this.elements)
			{
				result += sep;
				result += "@" + item.Key + " > ";
				result += item.Value.Encode(separators + 1);
				result += "\r\n";
			}

			result += closingSep + ")";

			return result;
		}
	}
}
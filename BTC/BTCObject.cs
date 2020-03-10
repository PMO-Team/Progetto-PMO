using System.Collections.Generic;

namespace BTC
{
	public class BTCObject : IBTCData
	{
		private Dictionary<string, IBTCData> elements;

		public void Add(string tag, int value)
		{
			this.elements.Add(tag, new BTCElement<int>(value));
		}
		public void Add(string tag, double value)
		{
			this.elements.Add(tag, new BTCElement<double>(value));
		}
		public void Add(string tag, string value)
		{
			this.elements.Add(tag, new BTCElement<string>(value));
		}
		public void Add(string tag, bool value)
		{
			this.elements.Add(tag, new BTCElement<bool>(value));
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

		public string Encode()
		{
			return "";
		}
	}
}
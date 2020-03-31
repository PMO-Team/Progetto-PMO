using System.Collections.Generic;

namespace BTC
{
	/**
	 * @class			BTCObject
	 * @implements		IBTCData
	 *
	 * @brief			Class for containing TAGs and ELEMENTs
	 * @details			This class rappresent the data tree structure,
	 * 					where each element is associated with a UNIQUE STRING TAG
	 */
	public class BTCObject : IBTCData
	{
		/**
		 * Since each element is a pair of a TAG and it's relative VALUE,
		 * the using of Dictionary data structure (Map in many other languages)
		 * suited very well for this purpose.
		 * The main reason for using a Map is that every BTCObject implements IBTCData,
		 * so we can see a BTCObject as a list of pairs <string, IBTCData> (since TAGs are strings). 
		 */
		private Dictionary<string, IBTCData> elements;

		/**
		 * @fn			BTCObject()
		 *
		 * @brief		Constructor
		 */
		public BTCObject()
		{
			this.elements = new Dictionary<string, IBTCData>();
		}

		/**
		 * @fn			bool IsTag(in string tagName)
		 * @brief		This method is used for detect if a given string is
		 * 				well formatted as TAG BTC SPEC says  
		 */
		public static bool IsTag(in string tagName)
		{
			bool correct = true;
			
			for (int i = 0; i < tagName.Length; i++)
			{
				if ((tagName[i] < 'a') && (tagName[i] > 'z') || (tagName[i] < 'A') && (tagName[i] > 'Z') && (tagName[i] != '-'))
					correct = false;
			}

			return correct;
		}

		/**
		 * @fn			bool Add(string tag, BTCNumber value)
		 * @brief		If TAG is malformed or already insert, the new value will be ignored
		 */
		public void Add(string tag, BTCNumber value)
		{
			if (IsTag(tag))
				try
				{
					this.elements.Add(tag, value);
				}
				catch (System.ArgumentException) {}
		}
		/**
		 * @fn			bool Add(string tag, BTCString value)
		 * @brief		If TAG is malformed or already insert, the new value will be ignored
		 */
		public bool Add(string tag, BTCString value)
		{
			bool ret = false;
			if (IsTag(tag))
				try
				{
					this.elements.Add(tag, value);
					ret = true;
				}
				catch (System.ArgumentException) {}
			
			return ret;
		}
		/**
		 * @fn			bool Add(string tag, BTCBool value)
		 * @brief		If TAG is malformed or already insert, the new value will be ignored
		 */
		public bool Add(string tag, BTCBool value)
		{
			bool ret = false;
			if (IsTag(tag))
				try
				{
					this.elements.Add(tag, value);
					ret = true;
				}
				catch (System.ArgumentException) { }

			return ret;
		}
		/**
		 * @fn			bool Add(string tag, BTCObject value)
		 * @brief		If TAG is malformed or already insert, the new value will be ignored
		 */
		public bool Add(string tag, BTCObject value)
		{
			bool ret = false;
			if (IsTag(tag))
				try
				{
					this.elements.Add(tag, value);
					ret = true;
				}
				catch (System.ArgumentException) { }

			return ret;
		}
		/**
		 * @fn			bool Add(string tag, BTCList value)
		 * @brief		If TAG is malformed or already insert, the new value will be ignored
		 */
		public bool Add(string tag, BTCList value)
		{
			bool ret = false;
			if (IsTag(tag))
				try
				{
					this.elements.Add(tag, value);
					ret = true;
				}
				catch (System.ArgumentException) { }

			return ret;
		}

		/**
		 * @fn			void Remove(string tag)
		 * @brief		Remove the element with the corrisponding TAG
		 */
		public void Remove(string tag)
		{
			this.elements.Remove(tag);
		}
		/**
		 * @fn			int Count()
		 * @brief		Returns the number of THIS object children
		 */
		public int Count()
		{
			return this.elements.Count;
		}

		/**
		 * @fn			IBTCData Tag(string tag)
		 * @brief		Returns the IBTCData that is paired with TAG.
		 *				If TAG doesn't exist, it returns null
		 */
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
		/**
		 * @fn			List<string> Tags()
		 * @brief		Return a list of TAGs associated with THIS object children
		 */
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
			string result;
			string sep = "";
			string closingSep = "";
			
			// Set-up the separotor string
			for (int i = 0; i < separators; i++)
				sep += "\t";
			for (int i = 1; i < separators; i++)
				closingSep += "\t";

			if (this.elements.Count > 0)
			{
				result = "(\r\n";
				foreach (var item in this.elements)
				{
					result += sep;
					result += "@" + item.Key + " > ";
					result += item.Value.Encode(separators + 1);
					result += "\r\n";
				}

				result += closingSep + ")";
			}
			else
			{
				result = "( )";
			}

			return result;
		}
	}
}
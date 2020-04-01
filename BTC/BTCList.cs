using System.Collections.Generic;

namespace BTC
{
	/**
	 * @class			BTCList
	 * @implements		IBTCData
	 *
	 * @brief			Class for containing ITEMs
	 * @details			This class rappresent the data tree structure.
	 */
	public class BTCList : IBTCData
	{
		/**
		 * Since all the children of BTCObject are ITEMs (only value: it's
		 * different from ELEMENT because ITEM is not a pair,
		 * so it doesn't have a TAG), a List is the most suitable object.
		 */
		private List<IBTCData> elements;

		/**
		 * @fn			BTCList()
		 *
		 * @brief		Constructor
		 */
		public BTCList()
		{
			this.elements = new List<IBTCData>();
		}

		/**
		 * @fn			void Add(BTCString i)
		 * 
		 * @param		i		item to be added
		 * 
		 * @brief		Insert the new item in the list
		 */
		public void Add(BTCString i)
		{
			this.elements.Add(i);
		}
		/**
		 * @fn			void Add(BTCNumber i)
		 * 
		 * @param		i		item to be added
		 * 
		 * @brief		Insert the new item in the list
		 */
		public void Add(BTCNumber i)
		{
			this.elements.Add(i);
		}
		/**
		 * @fn			void Add(BTCBool i)
		 * 
		 * @param		i		item to be added
		 * 
		 * @brief		Insert the new item in the list
		 */
		public void Add(BTCBool i)
		{
			this.elements.Add(i);
		}
		/**
		 * @fn			void Add(BTCObject i)
		 * 
		 * @param		i		item to be added
		 * 
		 * @brief		Insert the new item in the list
		 */
		public void Add(BTCObject i)
		{
			this.elements.Add(i);
		}
		/**
		 * @fn			void Add(BTCList i)
		 * 
		 * @param		i		item to be added
		 * 
		 * @brief		Insert the new item in the list
		 */
		public void Add(BTCList i)
		{
			this.elements.Add(i);
		}

		/**
		 * @fn			void RemoveAt(int index)
		 *
		 * @param		index	Index to be removed
		 *
		 * @brief		Remove the item at the corresponding index
		 */
		public void RemoveAt(int index)
		{
			this.elements.RemoveAt(index);
		}
		/**
		 * @fn			int Count()
		 * 
		 * @return		The number of elements in the list
		 *
		 * @brief		Return the number of items in the list
		 */
		public int Count()
		{
			return this.elements.Count;
		}
		/**
		 * @fn			IBTCData At(int index)
		 *
		 * @param		index	Index of the item to read
		 *
		 * @return		The item if index is valid, null otherwise
		 *
		 * @brief		Return the IBTCData instance contained at 'index'.
		 * 				If 'index' does not exist, will be returned 'null' value.
		 */
		public IBTCData At(int index)
		{
			IBTCData instance;

			try
			{
				instance = this.elements[index];
			}
			catch (System.ArgumentOutOfRangeException)
			{
				instance = null;
			}

			return instance;
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
				result = "[\r\n";
				for (int i = 0; i < this.elements.Count; i++)
				{
					result += sep;
					result += this.elements[i].Encode(separators + 1);
					if (i != (this.elements.Count - 1))
						result += ",";
					result += "\r\n";
				}

				result += closingSep + "]";
			}
			else
			{
				result = "[ ]";
			}

			return result;
		}
	}
}
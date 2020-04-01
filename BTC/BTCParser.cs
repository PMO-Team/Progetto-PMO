using System.IO;

namespace BTC
{
	public class BTCParser
	{
		/**
		 * @fn				static bool IsPadding(char c)
		 * @brief			Checks if a character is a padding character
		 * @details			This function is used for make short the character
		 * 					control in Normalize function
		 */
		private static bool IsPadding(char c)
		{
			return ((c == ' ') || (c == '\t') || (c == '\r') || (c == '\n'));
		}


		// Conversion Utils
		private static bool TryParse(in string s, out double o)
		{
			string val = string.Join(",", s.Split('.'));

			return double.TryParse(val, out o);
		}
		//
		private static bool TryParse(in string s, out bool o)
		{
			return (bool.TryParse(s, out o));
		}

		// Parsing utils
		/**
		 * @fn			static BTCObject ParseObject(ref int i, in string str)
		 * @params		i		Current character index
		 * @params		str		String to parse
		 *
		 * @brief		Used for sub-parsing process of an object
		 *
		 */
		private static BTCObject ParseObject(ref int i, in string str)
		{
			BTCObject mObj = new BTCObject();

			/*
			 * This method is invoked when the algortithm encounter
			 * the character '(', so it increases the index position by 1
			 */
			for (i += 1; str[i] != ')'; )
			{
				/*
				 * Until it doesn't find the closing object character, it continues
				 * to parse elements (TAG > VALUE)
				 */
				ParseElement(ref i, in str, ref mObj);
			}

			/*
			 * Increase the index position by one, for skip the end object character
			 */
			i += 1;

			return mObj;
		}
		//
		private static BTCList ParseList(ref int i, in string str)
		{
			BTCList mList = new BTCList();

			/*
			 * This method is invoked when the algortithm encounter
			 * the character '[', so 
			 */
			for (i += 1; str[i] != ']'; )
			{
				/*
				 * Until it doesn't find the closing list character, it continues
				 * to parse items (VALUE)
				 */
				ParseItem(ref i, in str, ref mList);

				/*
				 * If the item is followed by ',', it means that
				 * there are other items
				 */
				if (str[i] == ',')
					i += 1;
			}

			/*
			 * Increase the index position by one, for skip the end object character
			 */
			i += 1;

			return mList;
		}
		//
		private static void ParseElement(ref int i, in string str, ref BTCObject obj)
		{
			if (str[i] != '@')
				throw new BTCSyntaxErrorException("Syntax Error: Not A Tag. Found At: " + i);
			
			string tag = "";
			for (i += 1; str[i] != '>'; i++)
				tag += str[i];

				if ((tag.Length == 0) || (!BTCObject.IsTag(tag)))
					throw new BTCSyntaxErrorException("Syntax Error: Invalid TAG. Found at: " + i);
			
			i += 1;
			if (str[i] == '(')
				obj.Add(tag, ParseObject(ref i, in str));
			else if (str[i] == '[')
				obj.Add(tag, ParseList(ref i, in str));
			else if (str[i] == '\"')
				obj.Add(tag, new BTCString(ParseString(ref i, in str)));
			else
			{
				string value = "";
				for (; (str[i] != '@') && (str[i] != ')'); i++)
					value += str[i];
				
				if (value.Length == 0)
					throw new BTCSyntaxErrorException("Syntax Error: No Element Value. Found At: " + i);
				
				double dValue;
				bool bValue;

				if (TryParse(value, out dValue))
					obj.Add(tag, new BTCNumber(dValue));
				else if (TryParse(value, out bValue))
					obj.Add(tag, new BTCBool(bValue));
				else 
					throw new BTCSyntaxErrorException("Syntax Error: Invalid Element Value. Found At: " + i);
			}
		}
		//
		private static void ParseItem(ref int i, in string str, ref BTCList list)
		{
			if (str[i] == '(')
				list.Add(ParseObject(ref i, in str));
			else if (str[i] == '[')
				list.Add(ParseList(ref i, in str));
			else if (str[i] == '\"')
				list.Add(new BTCString(ParseString(ref i, in str)));
			else
			{
				string value = "";
				for (; (str[i] != ',') && (str[i] != ']'); i++)
					value += str[i];
				
				if (value.Length == 0)
					throw new BTCSyntaxErrorException("Syntax Error: no item value. Found At: " + i);
				
				double dValue;
				bool bValue;
				
				if (TryParse(value, out dValue))
					list.Add(new BTCNumber(dValue));
				else if (TryParse(value, out bValue))
					list.Add(new BTCBool(bValue));
				else 
					throw new BTCSyntaxErrorException("Syntax Error: invalid item value. Found At: " + i);
			}
		}
		//
		private static string ParseString(ref int i, in  string str)
		{
			string ret = "";
			for (i += 1; str[i] != '\"'; i++)
			{
				if (str[i] == '\\')
				{
					switch (str[i + 1])
					{
						case 'n':
							ret += '\n';
							break;
						case '\'':
							ret += '\'';
							break;
						case '\"':
							ret += '\"';
							break;
						case '\\':
							break;
						case 'r':
							ret += '\r';
							break;
						case 't':
							ret += '\t';
							break;
						default:
							throw new BTCSyntaxErrorException("Syntax Error: invalid special character at " + i);
					}
					i += 1;
				}
				else
					ret += str[i];
			}

			i += 1; // Next character after \"
			
			return ret;
		}

		/********************************************************************************/
		/*								Public Methods									*/
		/********************************************************************************/

		// Normalize the string (created for network usage) [O(n)]
		/**
		 * @fn			static string Normalize(in string str)
		 * @param		str		String to normalize
		 *
		 * @return		The string without padding characters
		 *
		 * @brief		Returns the string without padding characters
		 * @details		By a given string, creates a version without padding characters, but
		 * 				leaves string values intact (BTC specs for file).
		 */
		public static string Normalize(in string str)
		{
			string normalized = "";
			bool mLock = false;
			
			for (int i = 0; i < str.Length; i++)
			{
				if (mLock)
				{
					if (str[i] == '\"')
						if (str[i - 1] != '\\')
							mLock = false;
					normalized += str[i];
				}
				else
				{
					if (str[i] == '\"')
					{
						mLock = true;
						normalized += str[i];
					}
					else
						if (!IsPadding(str[i]))
							normalized += str[i];
				}
			}
			
			return normalized;
		}
		/**
		 * @fn			static BTCObject Decode(string str)
		 * @param		str		String to decode
		 *
		 * @return		The instance of BTCObject
		 *
		 * @throw		BTCSyntaxErrorException
		 *
		 * @brief		Decode the string into an instance of BTCObject
		 */
		public static BTCObject Decode(string str)
		{
			try
			{
				string norm = Normalize(in str);

				int i = 0;
				if (norm[i] != '(')
					throw new BTCSyntaxErrorException("Syntax Error: not an object");
				BTCObject rObj = ParseObject(ref i, in norm);

				return rObj;
			}
			catch (System.IndexOutOfRangeException)
			{
				throw new BTCSyntaxErrorException("Syntax Error: Data Are Incomplete");
			}
		}
		/**
		 * @fn			static BTCObject DecodeFromFile(string filepath)
		 * @param		filepath	Path to the file to decode
		 *
		 * @return		The instance of BTCObject
		 *
		 * @throw		BTCSyntaxErrorException
		 *
		 * @brief		Decode the specified file into an instance of BTCObject
		 */
		public static BTCObject DecodeFromFile(string filepath)
		{
			try
			{
				StreamReader sr = new StreamReader(filepath);
				string file = sr.ReadToEnd();
				sr.Close();
				string norm = Normalize(in file);

				int i = 0;
				if (norm[i] != '(')
					throw new BTCSyntaxErrorException("Syntax Error: not an object");
				BTCObject rObj = ParseObject(ref i, in norm);
				
				return rObj;
			}
			catch (System.IndexOutOfRangeException)
			{
				throw new BTCSyntaxErrorException("Syntax Error: Data in File are Incomplete");
			}
		}
		/**
		 * @fn			static string Encode(BTCObject obj, bool nicelyFormatted = true)
		 * @param		obj					BTCObject instance that is intended to be encode
		 * @param		nicelyFormatted		(optional) Specify how the object should be encoded,
		 * 									default value set true 
		 *
		 * @return		The encoded object as string
		 *
		 * @brief		Encode the instance of BTCObject in a string
		 */
		public static string Encode(BTCObject obj, bool nicelyFormatted = true)
		{
			string encoded;

			if (nicelyFormatted)
				encoded = obj.Encode(1);
			else
				encoded = obj.Encode();

			return encoded;
		}
		/**
		 * @fn			static void EncodeIntoFile(BTCObject obj, string filepath, bool nicelyFormatted = true)
		 * @param		obj					BTCObject instance that is intended to be encode
		 * @param		filepath			Path of the file that will be created
		 * @param		nicelyFormatted		(optional) Specify how the object should be encoded,
		 * 									default value set true 
		 *
		 * @brief		Encode the instance of BTCObject in the specified file
		 */
		public static void EncodeIntoFile(BTCObject obj, string filepath, bool nicelyFormatted = true)
		{
			string encoded;

			StreamWriter sw = new StreamWriter(filepath);

			if (nicelyFormatted)
				encoded = obj.Encode(1);
			else
				encoded = obj.Encode();

			sw.Write(encoded);
			sw.Close();
		}
	}
}
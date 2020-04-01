using System.IO;

namespace BTC
{
	/**
	 * @class		BTCParser
	 * @brief		Class that exposes public and static methods for dealing with BTC
	 */
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


		/********************************************************************************/
		/*								  Conversion Utils  							*/
		/********************************************************************************/
		/**
		 * @fn		static bool TryParse(in string s, out double o)
		 *
		 * @param	s	String to try to parse
		 * @param	o	Output destination
		 *
		 * @return		Return the success of the parsing of the string
		 *
		 * @brief		Try to parse a double from string 
		 */
		private static bool TryParse(in string s, out double o)
		{
			string val = string.Join(",", s.Split('.'));

			return double.TryParse(val, out o);
		}
		/**
		 * @fn		static bool TryParse(in string s, out bool o)
		 *
		 * @param	s	String to try to parse
		 * @param	o	Output destination
		 *
		 * @return		Return the success of the parsing of the string
		 *
		 * @brief		Try to parse a boolean from string 
		 */
		private static bool TryParse(in string s, out bool o)
		{
			return (bool.TryParse(s, out o));
		}

		/********************************************************************************/
		/*								   Parsing Utils								*/
		/********************************************************************************/
		/**
		 * @fn			static BTCObject ParseObject(ref int i, in string str)
		 * @params		i		Current character index
		 * @params		str		String to parse
		 *
		 * @return		An instance of BTCObject
		 *
		 * @brief		Used for sub-parsing process of an object
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
		/**
		 * @fn			static BTCList ParseList(ref int i, in string str)
		 * @params		i		Current character index
		 * @params		str		String to parse
		 *
		 * @return		An instance of BTCList
		 *
		 * @brief		Used for sub-parsing process of a list
		 */
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
		/**
		 * @fn			static void ParseElement(ref int i, in string str, ref BTCObject obj)
		 *
		 * @params		i		Current character index
		 * @params		str		String to parse
		 * @param		obj		BTCObject instance 
		 * 
		 * @throw		BTCSyntaxErrorException
		 *
		 * @brief		Add an element to the specified object
		 * @details		This method parse an element piece by piece:
		 * 				first of all read the tag, then the value and finally add it to the object
		 */
		private static void ParseElement(ref int i, in string str, ref BTCObject obj)
		{
			/*
			 * Checks if the element starts with the element character. If it's not correct,
			 * a BTCSyntaxErrorException is thrown.
			 */
			if (str[i] != '@')
				throw new BTCSyntaxErrorException("Syntax Error: Not A Tag. Found At: " + i);
			
			/*
			 * Read the tag one character at time, until '>' character
			 */
			string tag = "";
			for (i += 1; str[i] != '>'; i++)
				tag += str[i];

			/*
			 * If TAG is empty or malformed, throws BTCSyntaxErrorException
			 */
			if ((tag.Length == 0) || (!BTCObject.IsTag(tag)))
				throw new BTCSyntaxErrorException("Syntax Error: Invalid TAG. Found at: " + i);
			
			/*
			 * Skip '>'
			 */
			i += 1;

			/*
			 * Check which is the data value by controlling it's first character
			 */
			if (str[i] == '(') // For Object value
				obj.Add(tag, ParseObject(ref i, in str));
			else if (str[i] == '[') // For List value
				obj.Add(tag, ParseList(ref i, in str));
			else if (str[i] == '\"')  // For string value
				obj.Add(tag, new BTCString(ParseString(ref i, in str)));
			else  // For any other value
			{
				/*
				 * Read the value character by character, until it's different from
				 * element starting character ('@') or object closing character (')')
				 */
				string value = "";
				for (; (str[i] != '@') && (str[i] != ')'); i++)
					value += str[i];
				
				/*
				 * If the value is empty, a BTCSyntaxErrorException is thrown
				 */
				if (value.Length == 0)
					throw new BTCSyntaxErrorException("Syntax Error: No Element Value. Found At: " + i);
				
				double dValue;
				bool bValue;

				/*
				 * Otherwise, it tries to parse both double and boolean.
				 * If nothing works, throws BTCSyntaxErrorException
				 */
				if (TryParse(value, out dValue))
					obj.Add(tag, new BTCNumber(dValue)); // Succesfully parsed double, add the element
				else if (TryParse(value, out bValue))
					obj.Add(tag, new BTCBool(bValue)); // Succesfully parsed boolean, add the element
				else
					throw new BTCSyntaxErrorException("Syntax Error: Invalid Element Value. Found At: " + i);
			}
		}
		/**
		 * @fn			static void ParseItem(ref int i, in string str, ref BTCList list)
		 *
		 * @params		i		Current character index
		 * @params		str		String to parse
		 * @param		list	BTCList instance 
		 * 
		 * @throw		BTCSyntaxErrorException
		 *
		 * @brief		Add an item to the specified list
		 * @details		This method parse an item.
		 */
		private static void ParseItem(ref int i, in string str, ref BTCList list)
		{
			/*
			 * Check which is the data value by controlling it's first character
			 */
			if (str[i] == '(') // For Object value
				list.Add(ParseObject(ref i, in str));					// Add the item
			else if (str[i] == '[') // For List value
				list.Add(ParseList(ref i, in str));						// Add the item
			else if (str[i] == '\"') // For string value
				list.Add(new BTCString(ParseString(ref i, in str)));	// Add the item
			else // For any other value
			{
				string value = "";
				/*
				 * Read the value character by character, until it's different from
				 * item separator character (',') or list closing character (']')
				 */
				for (; (str[i] != ',') && (str[i] != ']'); i++)
					value += str[i];

				/*
				 * If the value is empty, a BTCSyntaxErrorException is thrown
				 */
				if (value.Length == 0)
					throw new BTCSyntaxErrorException("Syntax Error: no item value. Found At: " + i);
				
				double dValue;
				bool bValue;

				/*
				 * Otherwise, it tries to parse both double and boolean.
				 * If nothing works, throws BTCSyntaxErrorException
				 */
				if (TryParse(value, out dValue))
					list.Add(new BTCNumber(dValue));    // Succesfully parsed double, add the item
				else if (TryParse(value, out bValue))
					list.Add(new BTCBool(bValue));      // Succesfully parsed boolean, add the item
				else 
					throw new BTCSyntaxErrorException("Syntax Error: invalid item value. Found At: " + i);
			}
		}
		/**
		 * @fn			static string ParseString(ref int i, in  string str)
		 *
		 * @params		i		Current character index
		 * @params		str		String to parse
		 * 
		 * @throw		BTCSyntaxErrorException
		 *
		 * @brief		Parse a string
		 * @details		This method parse an element piece by piece:
		 * 				first of all read the tag, then the value and finally add it to the object
		 */
		private static string ParseString(ref int i, in  string str)
		{
			string ret = "";
			/*
			 * Scan the given string until the '"' character.
			 * Skips the first character (string opening '"')
			 */
			for (i += 1; str[i] != '\"'; i++)
			{
				/*
				 * Add characters unless '\' is found.
				 * In that case it checks for valid special character.
				 * If present, it is added to the string,
				 * throws a BTCSyntaxErrorException otherwise.
				 */
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
					/*
					 * Skip the character next to '\'
					 */
					i += 1;
				}
				else
					ret += str[i];
			}

			i += 1; // Next character after \"
			
			return ret;
		}

		/********************************************************************************/
		/*								  Public Methods								*/
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
				// Normalize the string, for optimize string analysis
				string norm = Normalize(in str);

				int i = 0;
				// Check if the first character is '(' (not done by ParseObject)
				if (norm[i] != '(')
					throw new BTCSyntaxErrorException("Syntax Error: not an object");
				
				// Actual Parsing
				BTCObject rObj = ParseObject(ref i, in norm);

				return rObj;
			}
			catch (System.IndexOutOfRangeException)
			{
				// If the IndexOutOfRangeException occours, means that file is incomplete
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
				// Read all the content of the file
				StreamReader sr = new StreamReader(filepath);
				string file = sr.ReadToEnd();
				sr.Close();

				// Normalize the string, for optimize string analysis
				string norm = Normalize(in file);

				int i = 0;
				// Check if the first character is '(' (not done by ParseObject)
				if (norm[i] != '(')
					throw new BTCSyntaxErrorException("Syntax Error: not an object");
				
				// Actual Parsing
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

			// Choose the algorithm to use
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

			// Creates the instance for writing in the secified file
			StreamWriter sw = new StreamWriter(filepath);

			// Choose the algorithm to use
			if (nicelyFormatted)
				encoded = obj.Encode(1);
			else
				encoded = obj.Encode();

			// Write down the result
			sw.Write(encoded);
			sw.Close();
		}
	}
}
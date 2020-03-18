using System.IO;

namespace BTC
{
	public class BTCParser
	{
		// Parsing Utilities
		private static BTCObject ParseObject(in string str)
		{
			BTCObject mObj;

			int i = 0;
			mObj = ParseObject(ref i, in str);

			return mObj;
		}
		private static BTCObject ParseObject(ref int i, in string str)
		{
			BTCObject mObj = new BTCObject();

			if (str[i] != '(')
				throw new BTCSyntaxErrorException("Error at character: " + i);

			for (i += 1; (i < str.Length) && (str[i] != ')'); i++)
			{
				ParseElement(ref i, in str, ref mObj);
			}

			return mObj;
		}
		
		// TODO LISTS
		private static BTCList ParseList(in string str)
		{
			BTCList mList;

			int i = 0;
			mList = ParseList(ref i, in str);
			
			return mList;
		}
		private static BTCList ParseList(ref int i, in string str)
		{
			BTCList mList = new BTCList();
			
			if (str[i] != '[')
				throw new BTCSyntaxErrorException("Error at character: " + i);

			for (; (i < str.Length) && (str[i] != ']'); i++)
			{
				ParseItem(ref i, in str, ref mList);
			}

			return mList;
		}
		
		// Check if the character is a padding character
		private static bool IsPadding(char c)
		{
			return ((c == ' ') || (c == '\t') || (c == '\r') || (c == '\n'));
		}
		// Check if the character is a valid tag character
		private static bool IsValidTag(char c)
		{
			return (((c >= 'a') && (c <= 'z')) || ((c >= 'A') || (c <= 'Z')));
		}
		// Check if the character is a valid number character
		private static bool IsNumber(char c)
		{
			return (((c >= '0') && (c <= '9')) || (c == '-') || (c == '.'));
		}
		// Parse a string
		private static BTCString ParseString(ref int i, in string str)
		{
			string value = "";
			for (i += 1; (i < str.Length) && (str[i] != '\"') ; i++)
			{
				value += str[i];
			}

			if (i < str.Length)
				return new BTCString(value);
			else
				throw new BTCSyntaxErrorException("Error at character: " + i);
		}
		// Parse a number
		private static BTCNumber ParseNumber(ref int i, in string str)
		{
			string value = "";
			double val;
			for (; (i < str.Length) && (str[i] != '\"') ; i++)
			{
				value += str[i];
			}

			if (i < str.Length)
				if (double.TryParse(value, out val))
					return new BTCNumber(val);
				else
					throw new BTCSyntaxErrorException("Error at character: " + i);
			else
				throw new BTCSyntaxErrorException("Error at character: " + i);
		}
		// Parse a boolean
		private static BTCBool ParseBool(ref int i, in string str)
		{
			string value = "";
			for (; (i < str.Length) && (str[i] != '\"') ; i++)
			{
				value += str[i];
			}

			if (i < str.Length)
				if (value == "true")
					return new BTCBool(true);
				else if (value == "false")
					return new BTCBool(false);
				else
					throw new BTCSyntaxErrorException("Error at character: " + i);
			else
				throw new BTCSyntaxErrorException("Error at character: " + i);
		}
		// Parse an element in an object
		private static void ParseElement(ref int i, in string str, ref BTCObject rObj)
		{
			string tag = "";
			for (i += 1; (i < str.Length) && (IsPadding(str[i])); i++);
			if (i < str.Length)
			{
				if (str[i] == '@')
				{
					// Building the TAG string
					for (i += 1; (i < str.Length) && (IsValidTag(str[i])); i++)
						tag += str[i];
					// Skipping padding characters
					for (; (i < str.Length) && (IsPadding(str[i])); i++);
					if (i < str.Length)
					{
						if (str[i] == '>')
						{
							// Skipping padding characters
							for (i += 1; (i < str.Length) && (IsPadding(str[i])); i++);
							// Checking for value
							if (i < str.Length)
							{
								if (IsNumber(str[i]))
								{
									BTCNumber mNum = ParseNumber(ref i, in str);

									rObj.Add(tag, mNum);
								}
								else if ((str[i] == 't') || (str[i] == 'f'))
								{
									BTCBool mBool = ParseBool(ref i, in str);

									rObj.Add(tag, mBool);
								}
								else if (str[i] == '\"')
								{
									BTCString mString = ParseString(ref i, in str);

									rObj.Add(tag, mString);
								}
								else
									throw new BTCSyntaxErrorException("Error at character: " + i);
							}
						}
						else
							throw new BTCSyntaxErrorException("Error at character: " + i);
					}
					
				}
				else if (str[i] == ')')
					return;
				else
					throw new BTCSyntaxErrorException("Error at character: " + i);
			}
			else
				throw new BTCSyntaxErrorException("Error at character: " + i);
		}
		private static void ParseItem(ref int i, in string str, ref BTCList rList)
		{

		}

		// Public Parsing Methods
		public static BTCObject Parse(string str)
		{
			BTCObject rObj = ParseObject(in str);
			
			return rObj;
		}
		public static BTCObject ParseFromFile(string filepath)
		{
			StreamReader sr = new StreamReader(filepath);
			string wholeFile = sr.ReadToEnd();

			BTCObject rObj = ParseObject(in wholeFile);
			
			return rObj;
		}

		public static string Encode(BTCObject obj, bool nicelyFormatted = false)
		{
			string encoded;

			if (nicelyFormatted)
				encoded = obj.Encode(0);
			else
				encoded = obj.Encode();

			return encoded;
		}
		public static void EncodeIntoFile(BTCObject obj, string filepath, bool nicelyFormatted = false)
		{
			string encoded;

			StreamWriter sw = new StreamWriter(filepath);

			if (nicelyFormatted)
				encoded = obj.Encode(0);
			else
				encoded = obj.Encode();

			sw.Write(encoded);
			sw.Close();
		}
	}
}
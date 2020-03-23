using System.IO;

namespace BTC
{
	public class BTCParser
	{
		// Checks if a character is a padding character
		private static bool IsPadding(char c)
		{
			return ((c == ' ') || (c == '\t') || (c == '\r') || (c == '\n'));
		}
		//
		private static bool IsTag(char c)
		{
			return (((c >= 'a') && (c <= 'z')) || ((c >= 'A') && (c <= 'Z')) || (c == '-'));
		}
		// Normalize the string (created for network usage) [O(n)]
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
		
		// Parsing utils
		private static BTCObject ParseObject(ref int i, in string str)
		{
			BTCObject mObj = new BTCObject();

			for (i += 1; str[i] != ')'; )
			{
				ParseElement(ref i, in str, ref mObj);
			}

			if (mObj.Count() == 0)
				throw new BTCSyntaxErrorException("Syntax Error: cannot insert an empty object");

			i += 1;

			return mObj;
		}
	//
		private static BTCList ParseList(ref int i, in string str)
		{
			BTCList mList = new BTCList();

			for (i += 1; str[i] != ']'; )
			{
				ParseItem(ref i, in str, ref mList);
				if (str[i] == ',')
					i += 1;
			}
			
			if (mList.Count() == 0)
				throw new BTCSyntaxErrorException("Syntax Error: cannot insert an empty list");

			i += 1;

			return mList;
		}
		//
		private static void ParseElement(ref int i, in string str, ref BTCObject obj)
		{
			if (str[i] != '@')
				throw new BTCSyntaxErrorException("Syntax Error: Not A Tag;\r\nFound At: " + i);
			
			string tag = "";
			for (i += 1; IsTag(str[i]) ; i++)
				tag += str[i];
			
			if ((tag.Length == 0) || (str[i] != '>'))
				throw new BTCSyntaxErrorException("Syntax Error: Invalid Separator in Element;\r\nFound At: " + i);
			
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
					throw new BTCSyntaxErrorException("Syntax Error: No Element Value;\r\nFound At: " + i);
				
				double dValue;
				bool bValue;

				if (double.TryParse(value, out dValue))
					obj.Add(tag, new BTCNumber(dValue));
				else if (bool.TryParse(value, out bValue))
					obj.Add(tag, new BTCBool(bValue));
				else 
					throw new BTCSyntaxErrorException("Syntax Error: Invalid Element Value;\r\nFound At: " + i);
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
					throw new BTCSyntaxErrorException("Syntax Error: no item value;\r\nFound At: " + i);
				
				double dValue;
				bool bValue;

				if (double.TryParse(value, out dValue))
					list.Add(new BTCNumber(dValue));
				else if (bool.TryParse(value, out bValue))
					list.Add(new BTCBool(bValue));
				else 
					throw new BTCSyntaxErrorException("Syntax Error: invalid item value;\r\nFound At: " + i);
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

		// Public Parsing Methods
		//
		public static BTCObject Decode(string str)
		{
			try
			{
				string norm = Normalize(in str);

				int i = 0;
				BTCObject rObj = ParseObject(ref i, in norm);

				return rObj;
			}
			catch (System.IndexOutOfRangeException)
			{
				throw new BTCSyntaxErrorException("Syntax Error: Data Are Incomplete");
			}
		}
		//
		public static BTCObject DecodeFromFile(string filepath)
		{
			try
			{
				StreamReader sr = new StreamReader(filepath);
				string file = sr.ReadToEnd();
				string norm = Normalize(in file);

				//System.Console.WriteLine(norm);

				int i = 0;
				BTCObject rObj = ParseObject(ref i, in norm);
				
				return rObj;
			}
			catch (System.IndexOutOfRangeException)
			{
				throw new BTCSyntaxErrorException("Syntax Error: Data in File are Incomplete");
			}
		}
		//
		public static string Encode(BTCObject obj, bool nicelyFormatted = false)
		{
			string encoded;

			if (nicelyFormatted)
				encoded = obj.Encode(1);
			else
				encoded = obj.Encode();

			return encoded;
		}
		//
		public static void EncodeIntoFile(BTCObject obj, string filepath, bool nicelyFormatted = false)
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
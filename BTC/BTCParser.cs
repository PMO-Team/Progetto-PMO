using System.IO;

namespace BTC
{
	public class BTCParser
	{
		// Public Parsing Methods
		public static BTCObject Parse(string str)
		{
			BTCObject rObj = new BTCObject();
			
			return rObj;
		}
		public static BTCObject ParseFromFile(string filepath)
		{
			BTCObject rObj = new BTCObject();
			
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
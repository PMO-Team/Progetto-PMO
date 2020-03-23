namespace BTC
{
	class BTCUtil
	{
		public static bool TryParse(in string s, out double o)
		{
			string val = string.Join(",", s.Split('.'));;

			return double.TryParse(val, out o);
		}

		public static bool TryParse(in string s, out bool o)
		{
			return (bool.TryParse(s, out o));
		}
	}
}
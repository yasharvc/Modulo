using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
	public static class Extenstions
	{
		public static IEnumerable<byte> ToByte(this string data, Encoding encoding = null)
		{
			encoding = encoding ?? Encoding.UTF8;
			return encoding.GetBytes(data);
		}

		public static string ToString(this IEnumerable<byte> bytes,Encoding encoding = null)
		{
			encoding = encoding ?? Encoding.UTF8;
			return encoding.GetString(bytes.Select(i => i).ToArray());
		}
	}
}

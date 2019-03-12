using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

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

		public static bool IsPrimitiveType(this object o)
		{
			Type returnType = o is Type ? (Type)o : o.GetType();
			return returnType.IsPrimitive || returnType == typeof(Decimal) || returnType == typeof(String) || returnType == typeof(byte[]);
		}
		public static object ConvertToType(this string value, Type CastType)
		{
			if (CastType.IsPrimitiveType())
			{
				if (CastType == typeof(int))
				{
					return Convert.ToInt32(value);
				}
				else if (CastType == typeof(string))
				{
					return HttpUtility.UrlDecode(value);
				}
				throw new NotImplementedException();
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}

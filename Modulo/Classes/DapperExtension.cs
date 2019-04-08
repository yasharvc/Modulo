using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dapper
{
	public static class DapperExtension
	{
		public static T Get<T>(this IDbConnection connection, string where, IDbTransaction transaction = null, int? commandTimeout = null)
		{
			var currenttype = typeof(T);
			var name = GetTableName(currenttype);
			var sb = new StringBuilder();
			sb.Append($"Select * From [{name}] Where {where}");
			return connection.Query<T>(sb.ToString(), null, transaction, true, commandTimeout).FirstOrDefault();
		}
		private static string GetTableName(Type type)
		{
			return (type.GetCustomAttributes(true).SingleOrDefault(attr => attr.GetType().Name == typeof(TableAttribute).Name) as dynamic).Name;
		}
		private static IEnumerable<PropertyInfo> GetScaffoldableProperties<T>()
		{
			IEnumerable<PropertyInfo> props = typeof(T).GetProperties();

			props = props.Where(p => p.GetCustomAttributes(true).Any(attr => attr.GetType().Name == typeof(EditableAttribute).Name && !IsEditable(p)) == false);


			return props.Where(p => p.PropertyType.IsSimpleType() || IsEditable(p));
		}
		private static bool IsEditable(PropertyInfo pi)
		{
			var attributes = pi.GetCustomAttributes(false);
			if (attributes.Length > 0)
			{
				dynamic write = attributes.FirstOrDefault(x => x.GetType().Name == typeof(EditableAttribute).Name);
				if (write != null)
				{
					return write.AllowEdit;
				}
			}
			return false;
		}

		public static bool IsSimpleType(this Type type)
		{
			var underlyingType = Nullable.GetUnderlyingType(type);
			type = underlyingType ?? type;
			var simpleTypes = new List<Type>
							   {
								   typeof(byte),
								   typeof(sbyte),
								   typeof(short),
								   typeof(ushort),
								   typeof(int),
								   typeof(uint),
								   typeof(long),
								   typeof(ulong),
								   typeof(float),
								   typeof(double),
								   typeof(decimal),
								   typeof(bool),
								   typeof(string),
								   typeof(char),
								   typeof(Guid),
								   typeof(DateTime),
								   typeof(DateTimeOffset),
								   typeof(TimeSpan),
								   typeof(byte[])
							   };
			return simpleTypes.Contains(type) || type.IsEnum;
		}

		public static string CacheKey(this IEnumerable<PropertyInfo> props)
		{
			return string.Join(",", props.Select(p => p.DeclaringType.FullName + "." + p.Name).ToArray());
		}
	}
}

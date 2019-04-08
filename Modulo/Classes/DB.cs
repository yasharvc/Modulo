using System.Data.SqlClient;

namespace Modulo.Classes
{
	public class DB
	{
		public enum DBKind
		{
			Permission,
			Data
		}
		public string GetConnectionString(bool debugMode = false)
		{
			//if (Startup.IsInDebugMode)
			//{
			//	if (debugMode)
			//		return "Data Source=192.168.0.56;Initial Catalog=CMMS_WEB_DEBUG;User ID=sa;Password=@321@123#;";
			//	return "Data Source=192.168.0.56;Initial Catalog=CMMS_WEB_DEBUG;User ID=sa;Password=@321@123#;";
			//}
			//else
			{
				if (debugMode)
					return "Data Source=192.168.0.56;Initial Catalog=CMMS_WEB_DEBUG;User ID=sa;Password=@321@123#;";
				return "Data Source=192.168.0.56;Initial Catalog=CMMS_WEB;User ID=sa;Password=@321@123#;";
			}
		}

		public SqlConnection GetConnection(DBKind kind)
		{
			return new SqlConnection(GetConnectionString());
		}
		public SqlCommand GetCommand(string Query, DBKind kind)
		{
			return new SqlCommand(Query, GetConnection(kind));
		}

		public object ExecuteScalar(string Query, DBKind kind)
		{
			object res;
			using (var cmd = GetCommand(Query, kind))
			{
				try
				{
					cmd.Connection.Open();
					res = cmd.ExecuteScalar();
				}
				finally
				{
					cmd.Connection.Close();
				}
			}
			return res;
		}
	}
}
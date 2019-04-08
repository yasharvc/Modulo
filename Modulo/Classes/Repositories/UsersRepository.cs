using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace Modulo.Classes.Repositories
{
	public class UsersRepository : IRepository<User>
	{
		public void Create(User entity)
		{
			using (IDbConnection con = new DB().GetConnection(DB.DBKind.Permission))
			{
				if (con.State == ConnectionState.Closed)
					con.Open();
				try
				{
					var x = con.Insert(entity);
					entity.SetID(x);
				}
				finally
				{
					con.Close();
				}
			}
		}


		public IEnumerable<User> ReadAll()
		{
			using (IDbConnection con = new DB().GetConnection(DB.DBKind.Permission))
			{
				if (con.State == ConnectionState.Closed)
					con.Open();
				try
				{
					return con.GetList<User>();
				}
				catch(Exception e)
				{
					throw e;
				}
				finally
				{
					con.Close();
				}
			}
		}
	}
}

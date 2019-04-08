using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Modulo.Classes.Repositories
{
	public interface IEntity
	{
	}
	public interface IRepository<T> where T : IEntity
	{
		void Create(T entity);
		IEnumerable<T> ReadAll();
	}
	public abstract class Entity<T> : IEntity where T: IEntity
	{
		protected IRepository<T> repository;

		public void Create()
		{
			repository.Create(GetThis());
		}
		protected abstract T GetThis();

		public void SetID(int? x)
		{
			if (x.HasValue)
			{
				GetIDColumn().SetValue(this, x);
			}
		}

		private PropertyInfo GetIDColumn()
		{
			var x = GetType().GetProperties().FirstOrDefault(p => p.CustomAttributes.Any(m => m.GetType() == typeof(KeyAttribute)));
			return x;
		}
	}


}

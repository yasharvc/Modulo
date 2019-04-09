using System;
using System.Collections.Generic;
using System.Reflection;

namespace ModuloContracts.Module.Meta
{
	public class ModelMeta:BaseMeta
	{
		public String Namespace { get; set; }
		public IEnumerable<FieldMeta> Properties { get; set; }
		public ModelMeta()
		{

		}
		public ModelMeta(Type type)
		{
			Namespace = type.Namespace;
			var props = GetModelProperties(type);
			Name = type.Name;
			List<FieldMeta> properties = new List<FieldMeta>();
			foreach (var prop in props)
				properties.Add(toFieldInformation(prop));
			Properties = properties;
		}

		private FieldMeta toFieldInformation(PropertyInfo prop)
		{
			FieldMeta res = new FieldMeta();
			res.FullTypeName = prop.PropertyType.FullName;
			res.Name = prop.Name;
			return res;
		}

		public override string ToString()
		{
			
			var propsStr = "";
			var methodsStr = "";
			foreach (var prop in Properties)
				propsStr += $"\tpublic {prop.FullTypeName} {prop.Name}" + "{ get;set; }\r\n";

			String template = 
				//"namespace " + Namespace + "\r\n{\r\n" +
				"\tpublic class " + Name + "\r\n\t{\r\n\t#region Properties\r\n" + propsStr + "\r\n\t#endregion Properties\r\n\r\n\t#region Methods\r\n"
				+ methodsStr + "\r\n\t#endregion\r\n\t}";
				//"\r\n}";
			return template;
		}

		private PropertyInfo[] GetModelProperties(Type type)
		{
			return type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		}
	}
}

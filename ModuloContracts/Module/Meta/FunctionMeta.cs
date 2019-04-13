using ModuloContracts.Module.Service;
using System;
using System.Collections.Generic;
using System.Reflection;
using Utility;

namespace ModuloContracts.Module.Meta
{
	public class FunctionMeta:BaseMeta
	{
		public IEnumerable<FieldMeta> Parameters { get; set; }
		public FieldMeta ReturnType { get; set; }
		public bool IsObsolete { get; set; } = false;
		public string Namespace { get; set; }
		public string ClassName { get; set; }
		public string Description { get; private set; }
		public FunctionMeta()
		{

		}
		public FunctionMeta(MethodInfo method,ServiceFunction serviceFunction,IService service)
		{
			Namespace = method.DeclaringType.Namespace;
			ClassName = method.DeclaringType.Name;
			Name = method.Name;
			Description = service.Description;
			List<FieldMeta> parameters = new List<FieldMeta>();
			ReturnType = new FieldMeta
			{
				Name = "",
				IsNullable = Nullable.GetUnderlyingType(method.ReturnType) != null
			};
			if (string.IsNullOrEmpty(serviceFunction.ReturnType))
			{
				if (method.ReturnType.IsGenericType)
				{
					ReturnType.FullTypeName = method.ReturnType.UnderlyingSystemType.ToString().Replace("[", "<").Replace("]", ">").Replace("`1", "").Replace("`2", "").Replace("`3", "");
				}
				else
				{
					ReturnType.FullTypeName = method.ReturnType.FullName;
				}
			}
			else
			{
				ReturnType.FullTypeName = serviceFunction.ReturnType;
			}
			var types = serviceFunction.ParametersTypes;
			var i = 0;
			foreach(var type in types)
			{
				var parameter = new FieldMeta
				{
					Name = $"var{++i}",
					FullTypeName = type.FullName
				};
				parameters.Add(parameter);
			}
			Parameters = parameters;
		}
		public override string ToString()
		{
			var paramList = "";
			var paramsJson = new List<string>();
			foreach (var parameter in Parameters)
			{
				paramList += $"{(paramList.Length > 0 ? ", " : "")}{parameter.FullTypeName} {parameter.Name}";
				try
				{
					if (Type.GetType(parameter.FullTypeName).IsPrimitiveType())
						paramsJson.Add(parameter.Name);
					else
						paramsJson.Add($"Newtonsoft.Json.JsonConvert.SerializeObject({parameter.Name})");
				}
				catch
				{
					paramsJson.Add($"Newtonsoft.Json.JsonConvert.SerializeObject({parameter.Name})");
				}
			}

			var body = $"\r\n\t\tvar returnType = typeof({ReturnType.FullTypeName});" +
				$"\r\n\t\tvar result = {ModuleClassName}.InvokationHub.ServiceInvoke(this.ModuleName,\"{Namespace}.{ClassName}\",\"{Name}\",{string.Join(",", paramsJson)});\r\n" +
				$"\t\tif (returnType.IsPrimitive || returnType == typeof(System.Decimal) || returnType == typeof(System.String) || returnType == typeof(byte[]))\r\n\t\t\t" +
				GetReturnPart() +
				$"\t\telse\r\n\t\t\treturn Newtonsoft.Json.JsonConvert.DeserializeObject<{ReturnType.FullTypeName}>(result as string);\r\n\t";

			string template = $"\t/*{Description}*/\r\n\tpublic {ReturnType.FullTypeName} {Name}({paramList})" + "\r\n\t{" + body + "}";
			return template;
		}

		private string GetReturnPart()
		{
			if (ReturnType.IsNullable)
				return $"return result as {ReturnType.FullTypeName};\r\n";
			else if(ReturnType.FullTypeName.ToLower() == "System.Boolean".ToLower())
			{
				return $"return System.Convert.ToBoolean(result);\r\n";
			}
			else
				return $"return ({ReturnType.FullTypeName})result;\r\n";
		}
	}
}

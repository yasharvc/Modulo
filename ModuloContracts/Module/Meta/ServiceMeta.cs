using ModuloContracts.Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModuloContracts.Module.Meta
{
	public class ServiceMeta : BaseMeta
	{
		protected Assembly Assembly { get; set; }
		public List<ModelMeta> Models { get; set; } = new List<ModelMeta>();
		public List<FunctionMeta> Functions { get; set; } = new List<FunctionMeta>();
		public string ModuleName { get; set; }
		protected int VersionMajor { get; set; }
		public ServiceMeta()
		{

		}
		public ServiceMeta(Assembly asm,IManifest manifest)
		{
			Assembly = asm;
			Name = Assembly.GetName().Name;
			ModuleName = manifest.ModuleName;
			VersionMajor = manifest.Version.Major;
		}
		public void Reload(byte[] AssemblyBytes) => Assembly = Assembly.Load(AssemblyBytes);
		public object CreateObject(string FullTypeName)
		{
			object obj = Assembly.CreateInstance(FullTypeName);
			return obj;
		}
		public object CreateObject(string Namespace, string ClassName) => CreateObject($"{Namespace}.{ClassName}");
		public object InvokeMethod(object obj, string MethodName, params object[] Parameters)
		{
			var methodInfo = obj.GetType().GetMethod(MethodName);
			var parameters = methodInfo.GetParameters();
			var convertedParams = new object[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				convertedParams[i] = Convert.ChangeType(Parameters[i], parameters[i].ParameterType);
			}
			try
			{
				return obj.GetType().InvokeMember(MethodName, BindingFlags.DeclaredOnly |
													   BindingFlags.Public | BindingFlags.NonPublic |
													   BindingFlags.Instance | BindingFlags.InvokeMethod, null, obj, convertedParams);
			}
			catch
			{
				return methodInfo.Invoke(obj, convertedParams);
			}
		}
		public IEnumerable<ParameterInfo> GetMethodParameters(object obj, string MethodName) => obj.GetType().GetMethod(MethodName).GetParameters();
		public override string ToString()
		{
			var models = $"public class Models{{\r\n{string.Join("\r\n", Models.Select(m => m.ToString()))}\r\n}}";
			models += models.Length > 0 ? "\r\n\r\n" : "";
			var functions = string.Join("\r\n", Functions.Select(m => m.ToString().Replace("\t", "\t\t")));
			var ctor = $"\r\n\t\tpublic {Name}Services()" + " { }";
			functions = "\tpublic class " + Name + "Services\r\n\t{\r\n\t\tprotected string ModuleName=\"" + ModuleName + "\";\r\n\t\tprotected int VersionMajor=" + VersionMajor + ";" + ctor + "\r\n" + functions 
				+ $"\r\n\t\tpublic static implicit operator {ModuleClassName}.Dependency(" + Name + "Services s) { return new " + ModuleClassName + ".Dependency { ModuleName = s.ModuleName, AcceptableMajor = s.VersionMajor }; }"
				+ "\r\n\t}";
			string imports = "using System;\r\nusing System.Collections.Generic;";
			string template = imports + "namespace " + Name + "\r\n{\r\n" + models + functions + "\r\n}";
			return template;
		}
	}
}

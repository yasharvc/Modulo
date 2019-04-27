using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSDL_To_Class
{
	public class ParameterInfo
	{
		private string _type = "";
		public string Type
		{
			get { return _type; }
			set
			{
				_type = value;
				var lower = _type.ToLower();
				if (lower.StartsWith("arrayof"))
					lower = lower.Substring("arrayof".Length);
				IsPrimitive = lower == "int" || lower == "float" || lower == "double" || lower == "long" || lower == "string" || lower == "bool";
			}
		}
		public string Name { get; set; }
		public bool IsPrimitive { get; set; }
	}
}

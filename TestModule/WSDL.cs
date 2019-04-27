using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestModule
{
	public class WmsService
	{
		private static string URL = "http://shonizit:180/WmsService.svc";

		public class GoodsCoding
		{
			public string DiagramCode { get; set; }
			public string DiagramName { get; set; }
			public string FullCode { get; set; }
			public string FullName { get; set; }
			public string GoodsFamilyCode { get; set; }
			public string GoodsFamilyName { get; set; }
			public string GoodsGroupCode { get; set; }
			public string GoodsGroupName { get; set; }
			public int GoodsId { get; set; }
			public string GoodsSpecificationsCode { get; set; }
			public string GoodsSpecificationsName { get; set; }
			public string GoodsSubGroupCode { get; set; }
			public string GoodsSubGroupName { get; set; }


			public void AddToWebService(SOAPWebService ws)
			{
				ws.AddParameter("DiagramCode", DiagramCode);
				ws.AddParameter("DiagramName", DiagramName);
				ws.AddParameter("FullCode", FullCode);
				ws.AddParameter("FullName", FullName);
				ws.AddParameter("GoodsFamilyCode", GoodsFamilyCode);
				ws.AddParameter("GoodsFamilyName", GoodsFamilyName);
				ws.AddParameter("GoodsGroupCode", GoodsGroupCode);
				ws.AddParameter("GoodsGroupName", GoodsGroupName);
				ws.AddParameter("GoodsId", GoodsId);
				ws.AddParameter("GoodsSpecificationsCode", GoodsSpecificationsCode);
				ws.AddParameter("GoodsSpecificationsName", GoodsSpecificationsName);
				ws.AddParameter("GoodsSubGroupCode", GoodsSubGroupCode);
				ws.AddParameter("GoodsSubGroupName", GoodsSubGroupName);
			}
		}

		public GoodsCoding GetGoodsCodingById(int value)
		{
			var ws = new SOAPWebService(URL, "GetGoodsCodingById");
			ws.AddParameter("value", value);

			ws.Invoke();
			return ws.GetObject<GoodsCoding>("/GetGoodsCodingByIdResponse/GetGoodsCodingByIdResult");
		}
		public System.Collections.Generic.List<GoodsCoding> GetGoodsCodingList()
		{
			var ws = new SOAPWebService(URL, "GetGoodsCodingList");

			ws.Invoke();
			return ws.GetObject<List<GoodsCoding>>("/GetGoodsCodingListResponse/GetGoodsCodingListResult");
		}
	}
}

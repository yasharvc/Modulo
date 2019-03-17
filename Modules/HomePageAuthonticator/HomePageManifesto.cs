using Microsoft.AspNetCore.Http;
using ModuloContracts.Data;
using ModuloContracts.Module;
using ModuloContracts.Module.Interfaces;
using ModuloContracts.Web;

namespace HomePageAuthonticator
{
	public class HomePageManifesto : Manifesto
	{
		public override string ModuleName => "HomePageAuthenticatorModule";

		public override string Title => "اعتبارسنج صفحه اصلی";

		public override string Description => "ماژولی که اعتبار کاربر را برای استفاده از صفحه می سنجد";
	}
}

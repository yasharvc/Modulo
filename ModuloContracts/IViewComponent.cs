using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ModuloContracts
{
	public interface IViewComponent
	{
		Task<IViewComponentResult> InvokeAsync();
	}
}

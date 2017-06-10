using Ch0601.Infrastructure;
using System.Web;
using System.Web.Mvc;

namespace Ch0601
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new HandleConcurrencyExceptionFilter());
        }
    }
}

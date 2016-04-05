using SimpleBlog2.Infrastructure;
using System.Web;
using System.Web.Mvc;

namespace SimpleBlog2
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new TransactionFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
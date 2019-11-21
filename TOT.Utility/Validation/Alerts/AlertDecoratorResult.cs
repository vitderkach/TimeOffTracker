using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace TOT.Utility.Validation.Alerts
{
    public class AlertDecoratorResult : IActionResult
    {
        public IActionResult Result { get; }
        public string Type { get; }
        public string Body { get; }

        public AlertDecoratorResult(IActionResult result, string type, string body)
        {
            Result = result;
            Type = type;
            Body = body;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var factory = context.HttpContext.RequestServices.GetService<ITempDataDictionaryFactory>();

            var tempData = factory.GetTempData(context.HttpContext);
            tempData["_alert.type"] = Type;
            tempData["_alert.body"] = Body;

            await Result.ExecuteResultAsync(context);
        }
    }
}

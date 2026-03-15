using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace webapi;

public class ApiPrefixConvention : IApplicationModelConvention
{
    private readonly string _prefix = "api";

    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            if (!controller.Attributes.OfType<ApiControllerAttribute>().Any())
                continue;

            foreach (var selector in controller.Selectors)
            {
                if (selector.AttributeRouteModel != null)
                {
                    var currentTemplate = selector.AttributeRouteModel.Template ?? "";

                    // ✅ Skip if already starts with "api"
                    if (currentTemplate.StartsWith(_prefix, StringComparison.OrdinalIgnoreCase))
                        continue;

                    selector.AttributeRouteModel = new AttributeRouteModel
                    {
                        Template = AttributeRouteModel.CombineTemplates(_prefix, currentTemplate)
                    };
                }
                else
                {
                    selector.AttributeRouteModel = new AttributeRouteModel
                    {
                        Template = _prefix
                    };
                }
            }
        }
    }
}
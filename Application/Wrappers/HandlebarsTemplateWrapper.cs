using Application.Interfaces;

namespace Application.Wrappers;
using HandlebarsDotNet;

public class HandlebarsTemplateWrapper : ITemplate
{
    private readonly Func<object, string> _compiledTemplate;

    public HandlebarsTemplateWrapper(Func<object, string> compiledTemplate)
    {
        _compiledTemplate = compiledTemplate;
    }

    public string Render(object data)
    {
        return _compiledTemplate(data);
    }
}   
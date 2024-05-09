using Identity.Application.Properties;

namespace Identity.Application.Common.Rules;

public class BaseBusinessRules(IStringLocalizer<Resources> localizer)
{
    protected readonly IStringLocalizer<Resources> localizer;
}
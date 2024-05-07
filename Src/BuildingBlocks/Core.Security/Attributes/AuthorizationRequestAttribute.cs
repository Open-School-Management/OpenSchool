using static Core.Security.Enums.SecurityEnum;

namespace Core.Security.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class AuthorizationAttribute : Attribute
{
    public ActionExponent[] Exponents { get; } = new ActionExponent[] { ActionExponent.View };

    public AuthorizationAttribute(ActionExponent[] exponents)
    {
        Exponents = Exponents.Concat(exponents).ToArray();
    }

    public AuthorizationAttribute()
    {
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using static SharedKernel.Contracts.Enum;

namespace SharedKernel.Libraries
{
    public class BaseAttributes
    {
        public static List<Type> GetCommonIgnoreAttribute()
        {
            return new List<Type>()
            {
                typeof(IgnoreAttribute)
            };
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayTextAttribute : DescriptionAttribute
    {
        public DisplayTextAttribute(string description) : base(description)
        {
        }
    }
    
}

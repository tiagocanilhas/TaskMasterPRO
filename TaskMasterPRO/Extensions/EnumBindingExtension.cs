using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace TaskMasterPRO.Extensions
{
    public class EnumBindingExtension : MarkupExtension
    {
        public Type EnumType { get; private set; } 

        public EnumBindingExtension(Type enumType)
        {
            if (enumType is null || !enumType.IsEnum)
                throw new InvalidOperationException("The EnumType must be a valid enum type.");

            EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => Enum.GetValues(EnumType);
    }
}

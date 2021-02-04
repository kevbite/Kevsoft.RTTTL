using System;

namespace Kevsoft.RTTTL
{
    internal static class ScaleEnumHelper
    {
        internal static bool TryParseDefinedScale(ReadOnlySpan<char> value, out Scale scale)
        {
            return Enum.TryParse(new string(value), out scale) &&
                   Enum.IsDefined(typeof(Scale), scale);
        }

    }
}
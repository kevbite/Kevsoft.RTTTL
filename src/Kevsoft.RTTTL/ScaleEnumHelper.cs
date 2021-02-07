using System;

namespace Kevsoft.RTTTL
{
    internal static class ScaleEnumHelper
    {
        internal static bool TryParseDefinedScale(ReadOnlySpan<char> value, out Scale scale)
        {
#if NETSTANDARD2_1
            var s = new string(value);
#else
            var s = new string(value.ToArray());
#endif
            return Enum.TryParse(s, out scale) &&
                   Enum.IsDefined(typeof(Scale), scale);
        }

    }
}
using System;

namespace Kevsoft.RTTTL
{
    internal static class ReadOnlySpanCharExtensions
    {
        internal static ReadOnlySpan<char> ConsumeToAndEatDelimiter(this ReadOnlySpan<char> text, char delimiter,
            out ReadOnlySpan<char> value)
        {
            var indexOfDelimiter = text.IndexOf(delimiter);

            if (indexOfDelimiter is -1)
            {
                value = text[..text.Length];
                text = ReadOnlySpan<char>.Empty;
            }
            else
            {
                value = text[..indexOfDelimiter];
                text = text[(indexOfDelimiter + 1)..];
            }

            return text;
        }
    }
}
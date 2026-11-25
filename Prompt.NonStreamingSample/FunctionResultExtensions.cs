using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SemanticKernel;

namespace Prompt.NonStreamingSample
{
    internal static class FunctionResultExtensions
    {
        public static Stream AsStream(this FunctionResult result)
        {
            ArgumentNullException.ThrowIfNull(result);

            ReadOnlySpan<char> json = result.ToString().AsSpan();

            int start = json.IndexOf('[');
            ArgumentOutOfRangeException.ThrowIfGreaterThan((uint)start, (uint)json.Length);

            int end = json.LastIndexOf(']');
            ArgumentOutOfRangeException.ThrowIfGreaterThan((uint)end, (uint)json.Length);

            ReadOnlySpan<char> array = json.Slice(start, end - start + 1);
            int length = Encoding.UTF8.GetByteCount(array);
            byte[] buffer = new byte[length];
            int count = Encoding.UTF8.GetBytes(array, buffer);
            ArgumentOutOfRangeException.ThrowIfLessThan(count, array.Length);

            return new MemoryStream(buffer);
        }
    }
}

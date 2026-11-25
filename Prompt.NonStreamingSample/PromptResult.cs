using System;
using System.Collections.Generic;
using System.Text;

namespace Prompt.NonStreamingSample
{
    internal class PromptResult
    {
        public PromptResult() { }

        public List<string> Items { get; set; } = default!;
    }
}

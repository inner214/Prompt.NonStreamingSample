using System.Diagnostics;
using System.Text.Json;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using Prompt.NonStreamingSample;

Kernel kernel = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion(
        modelId: "gpt-5",
        apiKey: Environment.GetEnvironmentVariable("OpenAI__ApiKey")!,
        endpoint: new Uri(Environment.GetEnvironmentVariable("AzureOpenAI__Endpoint")!))
    .Build();

OpenAIPromptExecutionSettings executionSettings = new OpenAIPromptExecutionSettings
{
    ResponseFormat = typeof(PromptResult),
    ReasoningEffort = "MINIMAL"
};

KernelArguments kernelArguments = new KernelArguments(executionSettings);

int count = 75;
string prompt = $"Generate {count} funny company names.";

Stopwatch stopWatch = Stopwatch.StartNew();
FunctionResult result = await kernel.InvokePromptAsync(prompt, kernelArguments);
long elapsed = stopWatch.ElapsedMilliseconds;

IAsyncEnumerable<string?> items =
    JsonSerializer.DeserializeAsyncEnumerable<string>(result.AsStream(), false);

await foreach (string? item in items)
{
    Console.WriteLine(item);
}

Console.WriteLine(new string('-', 10));
Console.WriteLine($"Time to First Token (TTFT): {elapsed / 1_000}s");

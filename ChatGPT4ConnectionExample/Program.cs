
using System.Text;
using System.Text.Json;

internal class Program
{
    private static readonly string apiKey = "Your_API_Key";
    private static readonly string apiUri = "https://api.openai.com/v1/engines/davinci-codex/completions";
    private static HttpClient client = new HttpClient();

    private static void Main(string[] args)
    {
        Console.WriteLine("Enter your prompt:");
        var prompt = Console.ReadLine();

        var response = GetChatGPTResponse(prompt);

        Console.WriteLine($"response: {response}");
    }

    private static async Task<string> GetChatGPTResponse(string prompt)
    {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var requestBody = new
        {
            prompt = prompt,
            max_tokens = 150
        };

        var jsonRequestBody = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(apiUri, jsonRequestBody);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
            return responseObject.GetProperty("choices")[0].GetProperty("text").GetString();
        }
        else
        {
            return $"Error: {response.StatusCode}";
        }
    }
}
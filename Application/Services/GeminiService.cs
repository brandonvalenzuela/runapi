using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public GeminiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> GenerateTrainingPlan(string prompt)
    {
        var requestPayload = new { prompt = prompt };
        var json = JsonSerializer.Serialize(requestPayload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://api.gemini.com/generate-plan", content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        return responseContent;
    }
}
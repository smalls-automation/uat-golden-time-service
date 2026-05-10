using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using System.Globalization;

namespace UatGoldenTimeService.Tests;

public class TimeEndpointTests
{
    private static readonly HttpClient Client = CreateClient();

    private static HttpClient CreateClient()
    {
        var factory = new WebApplicationFactory<Program>();
        return factory.CreateClient();
    }

    [Fact]
    public async Task GetTime_ReturnsStatusCode200()
    {
        // Arrange & Act
        var response = await Client.GetAsync("/api/time");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetTime_ReturnsJsonContentType()
    {
        // Arrange & Act
        var response = await Client.GetAsync("/api/time");

        // Assert
        var contentType = response.Content.Headers.ContentType?.MediaType;
        Assert.Equal("application/json", contentType);
    }

    [Fact]
    public async Task GetTime_ReturnsTimeProperty()
    {
        // Arrange & Act
        var response = await Client.GetAsync("/api/time");
        var json = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        // Assert
        Assert.True(root.TryGetProperty("time", out _), "JSON response should contain a 'time' property");
    }

    [Fact]
    public async Task GetTime_ReturnsIso8601UtcTimeString()
    {
        // Arrange & Act
        var response = await Client.GetAsync("/api/time");
        var json = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        var timeString = root.GetProperty("time").GetString()
            ?? throw new InvalidOperationException("Time property was null in response");

        // Assert
        Assert.Contains("T", timeString);
        Assert.EndsWith("Z", timeString);
        // Verify it parses back to a valid DateTime
        var parsed = DateTime.Parse(timeString, null, DateTimeStyles.RoundtripKind);
        Assert.Equal(DateTimeKind.Utc, parsed.Kind);
    }

    [Fact]
    public async Task GetTime_ReturnsRecentUtcTime()
    {
        // Arrange & Act
        var before = DateTime.UtcNow;
        var response = await Client.GetAsync("/api/time");
        var json = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        var timeString = root.GetProperty("time").GetString()
            ?? throw new InvalidOperationException("Time property was null in response");
        var after = DateTime.UtcNow;
        var receivedTime = DateTime.Parse(timeString, null, DateTimeStyles.RoundtripKind);

        // Assert
        Assert.True(receivedTime >= before, "Time should not be before the request was made");
        Assert.True(receivedTime <= after.AddSeconds(5), "Time should not be significantly after the request was made");
    }

    [Fact]
    public async Task GetTime_ReturnsIso8601OrdinalFormat()
    {
        // Arrange & Act
        var response = await Client.GetAsync("/api/time");
        var json = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        var timeString = root.GetProperty("time").GetString()
            ?? throw new InvalidOperationException("Time property was null in response");

        // Assert — ISO-8601 ordinal format uses "yyyy-MM-ddTHH:mm:ss.fffffffZ"
        Assert.Matches(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$", timeString);
    }

    [Fact]
    public async Task GetTime_ResponseBodyIsValidJson()
    {
        // Arrange & Act
        var response = await Client.GetAsync("/api/time");
        var json = await response.Content.ReadAsStringAsync();

        // Assert — parsing should not throw
        var document = JsonDocument.Parse(json);
        Assert.NotNull(document);
    }

    [Fact]
    public async Task GetTime_ResponseContainsOnlyTimeProperty()
    {
        // Arrange & Act
        var response = await Client.GetAsync("/api/time");
        var json = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        // Assert — the response should be a single-key object with only "time"
        Assert.Single(root.EnumerateObject());
    }
}

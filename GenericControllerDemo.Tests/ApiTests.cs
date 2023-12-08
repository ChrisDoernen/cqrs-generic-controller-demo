using System.Text;
using System.Text.Json;
using FluentAssertions;
using GenericControllerDemo.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Ping = GenericControllerDemo.Api.Ping;

namespace GenericControllerDemo.Tests;

public class ApiTests
{
    private readonly HttpClient _client;

    public ApiTests()
    {
        var factory = new WebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GettingResponseForPingRequestWorks()
    {
        var request = new Ping(new Message("ping"));
        var requestJson = JsonSerializer.Serialize(request);

        var httpResponse = await CallApi(request, requestJson);

        httpResponse.IsSuccessStatusCode.Should().BeTrue();

        var bodyJson = await new StreamReader(
            await httpResponse.Content.ReadAsStreamAsync(),
            Encoding.UTF8
        ).ReadToEndAsync();

        var result = JsonSerializer.Deserialize<Message>(bodyJson);

        result.Should().BeOfType<Message>();
        result!.Foo.Should().Be("pong");
    }

    [Fact]
    public async Task GettingResponseForBarRequestWorks()
    {
        var request = new Bar();
        var requestJson = JsonSerializer.Serialize(request);

        var httpResponse = await CallApi(request, requestJson);

        httpResponse.IsSuccessStatusCode.Should().BeTrue();
    }

    private async Task<HttpResponseMessage> CallApi(object request, string requestJson)
    {
        var httpResponse = await _client.PostAsync(
            $"requests/{request.GetType().Name}",
            new StringContent(requestJson, Encoding.UTF8, "application/json")
        );
        return httpResponse;
    }
}

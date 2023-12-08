using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

app.MapPost(
    "requests/{request}",
    async (HttpRequest httpRequest, ISender sender, [FromRoute] string request, CancellationToken cancellationToken) =>
    {
        var type = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .FirstOrDefault(t => t.Name == request);
        var bodyJson = await new StreamReader(httpRequest.Body, Encoding.UTF8).ReadToEndAsync();
        var requestObject = JsonSerializer.Deserialize(bodyJson, type!);

        var result = await sender.Send(requestObject!, cancellationToken);

        var resultJson = JsonSerializer.Serialize<object>(result);

        return Results.Text(resultJson, contentType: "application/json");
    }
);

app.Run();

// Used for integration test reference
public partial class Program;
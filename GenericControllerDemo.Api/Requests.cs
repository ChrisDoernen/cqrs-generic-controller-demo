using MediatR;

namespace GenericControllerDemo.Api;

public record Message(string Foo);

public record Ping(Message Message) : IRequest<Message>;

public class PingHandler(ILogger<PingHandler> logger) : IRequestHandler<Ping, Message>
{
    public Task<Message> Handle(Ping request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling ping, {request.Message.Foo}");

        return Task.FromResult(new Message("pong"));
    }
}

public record Bar : IRequest;

public class BarHandler(ILogger<BarHandler> logger) : IRequestHandler<Bar>
{
    public Task Handle(Bar _, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling bar");

        return Task.CompletedTask;
    }
}
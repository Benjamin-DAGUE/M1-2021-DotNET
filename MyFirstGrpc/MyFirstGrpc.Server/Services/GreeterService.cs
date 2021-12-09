using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace MyFirstGrpc.Server.Services;
public class GreeterService : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }

    public override async Task SendRandomData(Empty request, IServerStreamWriter<SendRandomDataReply> responseStream, ServerCallContext context)
    {
        Random random = new Random();

        while (!context.CancellationToken.IsCancellationRequested)
        {
            int data = random.Next(1, 5);

            await responseStream.WriteAsync(new SendRandomDataReply()
            {
               Data = data,
            });

            await Task.Delay(TimeSpan.FromSeconds(data), context.CancellationToken);
        }
    }
}

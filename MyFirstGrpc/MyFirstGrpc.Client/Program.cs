using Grpc.Core;
using Grpc.Net.Client;
using MyFirstGrpc.Client;



using var channel = GrpcChannel.ForAddress("https://localhost:7002");


var client = new Greeter.GreeterClient(channel);


var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });


Console.WriteLine("Greeting: " + reply.Message);


var call = client.SendRandomData(new Google.Protobuf.WellKnownTypes.Empty());

await foreach (SendRandomDataReply item in call.ResponseStream.ReadAllAsync())
{
    Console.WriteLine(item.Data);
}


Console.WriteLine("Press any key to exit...");
Console.ReadKey();
// See https://aka.ms/new-console-template for more information
using BiDirectionalStreaming.GrpcServer.Protos;
using Grpc.Net.Client;
using System.Threading;

Console.WriteLine("Hello, World!");

var channel = GrpcChannel.ForAddress("http://localhost:5092");

var client = new BiDirectionalStream.BiDirectionalStreamClient(channel);

var stream = client.Send();

var cancellationTokenSource = new CancellationTokenSource();

var names = new List<string>()
{
    "Emir",
    "Zeynep",
    "Alperen",
    "Seliha",
    "Dilara",
    "Erol",
    "Azra",
    "Harun",
    "Tolga"
};


var task = Task.Run(async () =>
{
    foreach (var name in names)
    {
        await Task.Delay(1000);
        await stream.RequestStream.WriteAsync(new BiDirectionalStreamRequest
        {
            Message = name

        }, cancellationTokenSource.Token);
    }
});

while (await stream.ResponseStream.MoveNext(cancellationTokenSource.Token))
{
    Console.WriteLine("From Server " + stream.ResponseStream.Current.Message);
}

await task;
await stream.RequestStream.CompleteAsync();

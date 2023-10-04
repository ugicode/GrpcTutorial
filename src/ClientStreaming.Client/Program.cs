// See https://aka.ms/new-console-template for more information
using ClientStreaming.GrpcServer.Protos;
using Grpc.Net.Client;

Console.WriteLine("Hello, World!");

var channel = GrpcChannel.ForAddress("http://localhost:5242");

var client = new ClientStream.ClientStreamClient(channel);

var request = client.Send();

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
foreach (var name in names) 
{ 
    await request.RequestStream.WriteAsync(new ClientStreamRequest
    {
        Name = name
    }, cancellationTokenSource.Token);
}

await request.RequestStream.CompleteAsync();

var result = await request.ResponseAsync;

Console.WriteLine("From Server : " + result.Message);
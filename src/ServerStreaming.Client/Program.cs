// See https://aka.ms/new-console-template for more information
using Grpc.Net.Client;
using ServerStreaming.GrpcServer.Protos;

Console.WriteLine("Hello, World!");


var channel = GrpcChannel.ForAddress("http://localhost:5206");

var client = new ServerStream.ServerStreamClient(channel);


var response = client.Send(new ServerStreamRequest { Name = "Hi"});

var cancellationTokenSource = new CancellationTokenSource();

while (await response.ResponseStream.MoveNext(cancellationTokenSource.Token))
{   
    Console.WriteLine(response.ResponseStream.Current.Message);
}
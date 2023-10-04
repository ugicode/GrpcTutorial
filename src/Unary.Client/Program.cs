// See https://aka.ms/new-console-template for more information
using Grpc.Core;
using Grpc.Net.Client;
using Unary.GrpcServer;

Console.WriteLine("Hello, World! From Grpc");



var channel = GrpcChannel.ForAddress("http://127.0.0.1:5133");

var client = new Greeter.GreeterClient(channel);


var response = await client.SayHelloAsync(new HelloRequest
{
    Name = "Grpc !!! From Client",
});

Console.WriteLine("Grpc Response Waiting ...");

Console.WriteLine($"Grpc Response is {response.Message}");

Console.ReadLine();


// Client uygulamarı için gerekli kütüphanelerdir.

// Google.Protobuf : Protobuf Serilization ve Deserilization işmlemlerini yapan kütüphanedir.
// Grpc.Net.Client : .Net Mimarisine uygun gRPC kütüphanesidir.
// Grpc.Tools : Proto dosyalarını derlemek için gerekli compiler ve diğer araçları içeren kütüphanedir.

/*
    <PackageReference Include="Google.Protobuf" />
    <PackageReference Include="Grpc.AspNetCore.Server.ClientFactory" />
    <PackageReference Include="Grpc.Tools" PrivateAssets="All" />
 
 */
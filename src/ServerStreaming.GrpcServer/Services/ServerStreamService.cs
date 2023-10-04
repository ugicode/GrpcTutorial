using Grpc.Core;
using ServerStreaming.GrpcServer.Protos;

namespace ServerStreaming.GrpcServer.Services;

public class ServerStreamService : ServerStream.ServerStreamBase
{
    public override async Task Send(ServerStreamRequest request, IServerStreamWriter<ServerStreamResponse> responseStream, ServerCallContext context)
    {
        Console.WriteLine("Get From Client " + request.Name);

        for (int i = 0; i < 10; i++)
        {
            await Task.Delay(200);
            
            await responseStream.WriteAsync(new ServerStreamResponse { Message = $"{request.Name} Emir {i}" });

        }
    }

}

using ClientStreaming.GrpcServer.Protos;
using Grpc.Core;
using System.Text;

namespace ClientStreaming.GrpcServer.Services;

public class ClientStreamService : ClientStream.ClientStreamBase
{
    public override async Task<ClientStreamResponse> Send(IAsyncStreamReader<ClientStreamRequest> requestStream, ServerCallContext context)
    {
        var builder = new StringBuilder();

        while (await requestStream.MoveNext(context.CancellationToken))
        {
            builder.Append($"{requestStream.Current.Name}, ");
        }

        return new ClientStreamResponse
        {
            Message = builder.ToString(),
        };
    }
}

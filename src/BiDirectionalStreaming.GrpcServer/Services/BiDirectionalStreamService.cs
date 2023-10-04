

using BiDirectionalStreaming.GrpcServer.Protos;
using Grpc.Core;

namespace BiDirectionalStreaming.GrpcServer.Services;

public class BiDirectionalStreamService : BiDirectionalStream.BiDirectionalStreamBase
{
    public override async Task Send(IAsyncStreamReader<BiDirectionalStreamRequest> requestStream, IServerStreamWriter<BiDirectionalStreamResponse> responseStream, ServerCallContext context)
    {
        var task = Task.Run(async () =>
        {
            while (await requestStream.MoveNext(context.CancellationToken))
            {
                Console.WriteLine("From Client " + requestStream.Current.Message);
            };
        });


        for (int i = 0; i < 10; i++)
        {
            await Task.Delay(1000);
            await responseStream.WriteAsync(new BiDirectionalStreamResponse
            {
                Message = requestStream.Current.Message + i,
            });
        }

        await task;
    }

}

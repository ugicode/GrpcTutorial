// See https://aka.ms/new-console-template for more information
using FileStreaming.GrpcServer.Protos;
using Google.Protobuf;
using Grpc.Net.Client;

Console.WriteLine("Hello, World!");

Console.WriteLine("1. Upload");
Console.WriteLine("2. Download");



var key = Console.ReadLine();


if (key == "1")
{
    await Upload();

}
else if (key == "2")
{
    await Download();
}
else
{
    await Upload();
}

Console.ReadLine();


static async Task Download()
{
    var channel = GrpcChannel.ForAddress("http://127.0.0.1:5298");

    var client = new FileService.FileServiceClient(channel);

    var path = @"C:\Users\emirh\source\repos\GrpcTutorial\src\FileStreaming.Client\Downloads\";

    var fileInfo = new FileStreaming.GrpcServer.Protos.FileInfo
    {
        FileName = @"Atatürk - Yeni bir güneş gibi doğacaktır! _ Little Dark Age",
        FileExtension = ".mp4"
    };

    FileStream fileStream = null;

    var download = client.FileDownload(fileInfo);

    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    int count = 0;
    decimal chunkSize = 0;

    while (await download.ResponseStream.MoveNext(cancellationTokenSource.Token))
    {
        if (count++ == 0)
        {
            fileStream = new FileStream(
                $@"{path}\{download.ResponseStream.Current.Info.FileName}{download.ResponseStream.Current.Info.FileExtension}",
                FileMode.CreateNew, FileAccess.ReadWrite);

            fileStream.SetLength(download.ResponseStream.Current.FileSize);
        }


        var buffer = download.ResponseStream.Current.Buffer.ToByteArray();
        await fileStream.WriteAsync(buffer, 0, download.ResponseStream.Current.ReadedByte);
        Console.WriteLine($"{Math.Round(((chunkSize += download.ResponseStream.Current.ReadedByte) * 100) / download.ResponseStream.Current.FileSize)}%");
    }

    await Console.Out.WriteLineAsync("Yüklendi ...");
    await fileStream.DisposeAsync();
    fileStream.Close();

}


static async Task Upload()
{
    var channel = GrpcChannel.ForAddress("http://127.0.0.1:5298");

    var client = new FileService.FileServiceClient(channel);

    var upload = client.FileUpload();


    var file = @"C:\Users\emirh\Downloads\ForOther\Atatürk - Yeni bir güneş gibi doğacaktır! _ Little Dark Age.mp4";


    FileStream fileStream = new FileStream(file, FileMode.Open);



    var content = new BytesContent
    {
        FileSize = fileStream.Length,
        ReadedByte = 0,
        Info = new FileStreaming.GrpcServer.Protos.FileInfo
        {
            FileName = Path.GetFileNameWithoutExtension(fileStream.Name),
            FileExtension = Path.GetExtension(fileStream.Name)
        }
    };

    byte[] buffer = new byte[2048];

    while ((content.ReadedByte = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
    {
        content.Buffer = ByteString.CopyFrom(buffer);
        await upload.RequestStream.WriteAsync(content);
    }

    await upload.RequestStream.CompleteAsync();
    fileStream.Close();
}


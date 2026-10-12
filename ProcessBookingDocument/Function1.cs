using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ProcessBookingDocument;

public class Function1(ILogger<Function1> logger)
{
    //123 - id
    // bookings/123
    
    [Function(nameof(Function1))]
    public async Task Run([BlobTrigger("bookings/{id}", Connection = "")] Stream stream, string id)
    {
        using var blobStreamReader = new StreamReader(stream);
        var content = await blobStreamReader.ReadToEndAsync();
        logger.LogInformation("C# Blob trigger function processed blob\n Name: {name} \n Data: {content}", id,
            content);
        
    }
}
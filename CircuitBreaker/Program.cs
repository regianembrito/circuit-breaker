using Amazon;
using Amazon.SQS;
using CircuitBreaker.Service;

class Program
{
    static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IAmazonSQS>(sp => new AmazonSQSClient(RegionEndpoint.USEast1))
            .AddSingleton<ProcessingService>()
            .AddSingleton<SqsConsumerService>()
            .BuildServiceProvider();

        var consumerService = serviceProvider.GetRequiredService<SqsConsumerService>();

        await consumerService.StartListening();
    }
}

using Amazon.SQS;
using Amazon.SQS.Model;
using CircuitBreaker.Infrastructure;
using Polly.CircuitBreaker;
using System.Diagnostics;

namespace CircuitBreaker.Service
{
    public class SqsConsumerService
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly ProcessingService _processingService;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        private readonly string _queueUrl = "https://sqs.us-east-1.amazonaws.com/123456789012/MyQueue";

        public SqsConsumerService(IAmazonSQS sqsClient, ProcessingService processingService)
        {
            _sqsClient = sqsClient;
            _processingService = processingService;
            _circuitBreakerPolicy = CircuitBreakerPolicyConfig.CreatePolicy();
        }

        public async Task StartListening()
        {
            
            while (true)
            {
                var receiveMessageRequest = new ReceiveMessageRequest
                {
                    QueueUrl = _queueUrl,
                    MaxNumberOfMessages = 1,
                    WaitTimeSeconds = 10
                };
                var response = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest);
                foreach (var message in response.Messages)
                {
                    await ProcessMessage(message);
                }
            }
        }

        private async Task ProcessMessage(Message message)
        {
            Console.WriteLine($"Received message: {message.Body}");
            try
            {
                await _circuitBreakerPolicy.ExecuteAsync(() => _processingService.ProcessMessageAsync(message.Body));
                await _sqsClient.DeleteMessageAsync(_queueUrl, message.ReceiptHandle);
            }
            catch (BrokenCircuitException)
            {
                
                Console.WriteLine($"Circuit Breaker OPEN! Storing message for retry: {message.Body}");
                // Aqui você pode salvar a mensagem em um banco de dados ou DLQ
            }
        }
    }
}

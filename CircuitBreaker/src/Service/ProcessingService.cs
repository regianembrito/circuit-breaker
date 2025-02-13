namespace CircuitBreaker.Service
{
    public class ProcessingService
    {
        private readonly Random _random = new Random();

        public async Task ProcessMessageAsync(string message)
        {
            // Simula uma falha em 40% das vezes
            if (_random.Next(0, 10) < 4)
            {
                throw new Exception("Falha ao processar a mensagem.");
            }

            Console.WriteLine($"Mensagem processada com sucesso: {message}");
            await Task.CompletedTask;
        }
    }
}

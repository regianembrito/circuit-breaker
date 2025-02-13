using Polly;
using Polly.CircuitBreaker;

namespace CircuitBreaker.Infrastructure
{
    public static class CircuitBreakerPolicyConfig
    {
        public static AsyncCircuitBreakerPolicy CreatePolicy()
        {
            return Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 3, // Fechar o circuito após 3 falhas
                    durationOfBreak: TimeSpan.FromSeconds(10), // Período de espera antes de tentar novamente
                    onBreak: (ex, breakDelay) =>
                    {
                        Console.WriteLine($"Circuit Breaker OPEN: {ex.Message}. Retentando em {breakDelay.Seconds} segundos.");
                    },
                    onReset: () =>
                    {
                        Console.WriteLine("Circuit Breaker RESET: Retomando operações.");
                    },
                    onHalfOpen: () =>
                    {
                        Console.WriteLine("Circuit Breaker HALF-OPEN: Tentando novamente.");
                    }
                );
        }
    }
}

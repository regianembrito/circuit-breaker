Implementação em .NET Core usando Polly para o Circuit Breaker e AWS SDK para .NET para consumir mensagens da fila SQS.

## 📌 Cenário
* Um consumidor escuta mensagens de uma fila AWS SQS.
* Cada mensagem é processada chamando um serviço externo.
* Se houver falhas consecutivas, o Circuit Breaker entra no estado OPEN, evitando novas chamadas até que o serviço esteja disponível novamente.
* Se o Circuit Breaker estiver OPEN, a mensagem pode ser armazenada para uma tentativa posterior (exemplo: Dead Letter Queue ou banco de dados).

## 🛠 Explicação
1. Circuit Breaker (Polly)

    * Fecha após 3 falhas consecutivas.
    * Aguarda 10 segundos antes de reabrir parcialmente (HALF-OPEN).
    * Reabre completamente se chamadas de teste forem bem-sucedidas.

2. Consumidor de SQS

    *Escuta mensagens da fila AWS SQS.
    * Usa Circuit Breaker ao chamar ProcessingService.ProcessMessage(message).
    * Se o Circuit Breaker estiver OPEN, a mensagem pode ser armazenada para tentativa posterior.

3. Serviço de Processamento

    * Simula falhas 40% das vezes para testar o Circuit Breaker.

## 🎯 Conclusão
Essa abordagem protege a aplicação de sobrecarga caso o serviço de processamento esteja instável, garantindo maior resiliência e estabilidade.

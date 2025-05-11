using System.Text;
using RabbitMQ.Client;

// Criando fábrica para gerar a conexão e o canal
var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

//Criando a declaração para iniciar a fila
await channel.QueueDeclareAsync(
    queue: "mensagem",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null);

for (int i = 0; i < 10; i++)
{
    var mensagem = $"{DateTime.UtcNow} - {Guid.NewGuid()} - Nova Mensagem";
    var body = Encoding.UTF8.GetBytes(mensagem);

    //Realizando uma publicação basica da mensagem no RabbitMQ
    await channel.BasicPublishAsync(
        exchange: string.Empty,
        routingKey: "mensagem",
        mandatory: true,
        basicProperties: new BasicProperties 
        { 
            Persistent = true, 
            AppId = "14123", 
            UserId = "guest" },
        body: body);

    Console.WriteLine($"Enviando: {mensagem}");

    await Task.Delay(2000);
}
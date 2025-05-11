using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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

Console.WriteLine("Esperando por mensagens...");

var consumidor = new AsyncEventingBasicConsumer(channel);
//Criando um evento async para receber as mensagens
consumidor.ReceivedAsync += async (sender, eventArgs) =>
{
    byte[] body = eventArgs.Body.ToArray();
    string mensagem = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Recebido: {mensagem}");

    await ((AsyncEventingBasicConsumer)sender).Channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
};
uint qtd = await channel.MessageCountAsync("mensagem");

Console.WriteLine($"Quantidade de Mensagens total no servidor: {qtd}");
if (qtd > 0)
    await channel.BasicConsumeAsync("mensagem", false, consumidor);

Console.ReadLine();
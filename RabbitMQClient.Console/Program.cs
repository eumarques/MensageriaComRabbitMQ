using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class Program
{
    private static void Main(string[] args)
    {
        const string EXCHANGE = "curso-rabbitmq";
        const string ROUTINGKEY = "hr.person-created";

        var person = new Person("Tiago Marques", "123.456.789-10", new DateTime(1995, 06, 26));

        var connectionFactory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        var connection = connectionFactory.CreateConnection(EXCHANGE);
        var publishChannel = connection.CreateModel();

        var json = JsonSerializer.Serialize(person);
        var byteArray = Encoding.UTF8.GetBytes(json);

        publishChannel.BasicPublish(EXCHANGE, ROUTINGKEY, null, byteArray);

        Console.WriteLine($"Menssagem published: {json}");

        // criando um canal de consumo das mensagem
        var consumerChannel = connection.CreateModel();
        var consumer = new EventingBasicConsumer(consumerChannel);

        consumer.Received += (sender, eventArgs) =>
        {
            var contentArray = eventArgs.Body.ToArray();
            var contentString = Encoding.UTF8.GetString(contentArray);

            var message = JsonSerializer.Deserialize<Person>(contentString);

            Console.WriteLine($"Message received: {contentString}");

            consumerChannel.BasicAck(eventArgs.DeliveryTag, false);
        };

        consumerChannel.BasicConsume("person-created", false, consumer);
        Console.ReadLine();
    }

}

public class Person
{
    public Person(string fullName, string document, DateTime birthDate)
    {
        fullName = fullName;
        Document = document;
        birthDate = birthDate;
    }

    public string fullName { get; set; }
    public string Document { get; set; }
    public DateTime birthDate { get; set; }
}
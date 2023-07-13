using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Http.Headers;

namespace showcase.gatway.Extensions
{
    public class ChannelOption
    {
        public string QueueNane { get; set; } = string.Empty;
        public bool Durable { get; set; } = true;
        public bool Exclusive { get; set; } = false;
        public bool AutoDelete { get; set; } = false;
        public string Url { get; set; } = string.Empty;
        public Dictionary<string, object>? Arguments { get; set; } = null;
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "Limar";
        public string Password { get; set; } = "itsgallus";
        public string VirtualHost { get; set; } = "showcase-messages";
        public string ClienteName { get; set; } = "showcase-gatway";

        public static ChannelOption Factory(string name, string url)
        {
            return new ChannelOption { QueueNane = name, Url = url };
        }
    }

    public class PublishOption
    {
        public string Exchange { get; set; } = string.Empty;
        public string RoutingKey { get; set; } = string.Empty;
        public ReadOnlyMemory<byte> Body { get; set; }
        public IModel Channel { get; }
        public IDictionary<string, object> Headers { get; set; } = new Dictionary<string, object>();

        public PublishOption(ChannelOption option)
        {
            this.Channel = option.QueueDeclare();
            this.RoutingKey = option.QueueNane;
        }
    }

    public class ReadChannel
    {
        public IModel Channel { get; }
        public string QueueName { get; }

        public ReadChannel(ChannelOption option)
        {
            this.Channel = option.QueueDeclare();
            this.QueueName = option.QueueNane;
        }
    }

    public static class RabbitMQExtensions
    {
        private static ConnectionFactory? _factory;
        private static IConnection? _connection;

        public delegate void ReceivedMessage(byte[] mensagem, IDictionary<string, object> headers);

        public static void ConsumeMessages(this ReadChannel readChannel, ReceivedMessage receivedMessage)
        {
            var consumer = new EventingBasicConsumer(readChannel.Channel);

            consumer.Received += (model, ea) =>
            {
                var headers = ea.BasicProperties.Headers;
                var body = ea.Body.ToArray();

                receivedMessage(body, headers);
            };

            readChannel.Channel.BasicConsume(queue: readChannel.QueueName, autoAck: true, consumer: consumer);
        }

        public static IModel QueueDeclare(this ChannelOption modelOption)
        {
            _factory ??= new ConnectionFactory
            {
                HostName = modelOption.Url,
                Port = modelOption.Port,
                UserName = modelOption.UserName,
                Password = modelOption.Password,
                VirtualHost = modelOption.VirtualHost,
            };
            _connection ??= _factory.CreateConnection(modelOption.ClienteName);
            var channel = _connection.CreateModel();

            channel.QueueDeclare(modelOption);

            return channel;
        }

        public static void BasicPublish(this PublishOption publishOption)
        {
            var properties = publishOption.Channel.CreateBasicProperties();
            properties.Persistent = true;
            
            properties.Headers = publishOption.Headers;

            publishOption.Channel.BasicPublish(exchange: publishOption.Exchange,
                                 routingKey: publishOption.RoutingKey,
                                 basicProperties: properties,
                                 body: publishOption.Body);
        }

        private static void QueueDeclare(this IModel model, ChannelOption modelOption)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            model.QueueDeclare(modelOption.QueueNane, modelOption.Durable, modelOption.Exclusive, modelOption.AutoDelete, modelOption.Arguments);
        }

        public static IDictionary<string, object> ToDictionary(this HttpRequestHeaders headers)
        {
            var dictionary = new Dictionary<string, object>();

            foreach (var header in headers)
            {
                var headerValues = new List<string>();

                foreach (var value in header.Value)
                {
                    headerValues.Add(value);
                }

                dictionary.Add(header.Key, headerValues);
            }

            return dictionary;
        }
    }
}

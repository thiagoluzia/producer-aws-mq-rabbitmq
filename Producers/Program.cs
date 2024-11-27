#region TODO
// TODO: Criar Conexão - Estabelecer uma conexão com o RabbitMQ usando ConnectionFactory e CreateConnectionAsync().
// A conexão é responsável por estabelecer o link com o servidor RabbitMQ.

// TODO: Criar Canal - Utilizar CreateChannelAsync() para criar um canal de comunicação a partir da conexão estabelecida.
// O canal é essencial para interagir com o RabbitMQ, possibilitando declarar filas e publicar mensagens.

// TODO: Declarar Fila - Declarar a fila "hello" com QueueDeclareAsync() para garantir que a fila exista antes do envio das mensagens.
// Isso assegura que as mensagens enviadas tenham uma fila configurada para recebê-las.

// TODO: Criar e Serializar Mensagem - Criar um objeto Aluno com nome e e-mail fornecidos pelo usuário.
// Serializar o objeto para JSON e convertê-lo em bytes (UTF-8), pois o RabbitMQ trabalha apenas com dados binários.

// TODO: Publicar Mensagem - Utilizar o canal para publicar a mensagem na fila com BasicPublishAsync().
// Utilizar a exchange padrão (string.Empty) e a routingKey "hello" para indicar a fila de destino da mensagem.
#endregion
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Producers
{
    #region Sem comentário

    public class Program
    {
        static async Task Main(string[] args)
        {
            //Local
            var factory = new ConnectionFactory { HostName = "localhost" };

            //AWS
            //var factory = new ConnectionFactory 
            //{ 
            //    Uri = new Uri("amqps://"),
            //    Port = 5671,
            //    VirtualHost = "/",
            //    UserName = "",
            //    Password = ""

            //};

           
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "hello",
                                   durable: false,
                                   exclusive: false,
                                   autoDelete: false,
                                   arguments: null);

            Console.Write("Digite uma mensagem e aperte ENTER: ");

            while (true)
            {
                var aluno = new Aluno();

                Console.Write("NOME: ");
                aluno.Nome = Console.ReadLine().ToString();
                Console.Write("E-MAIL: ");
                aluno.Email = Console.ReadLine().ToString();

                if (String.IsNullOrEmpty(aluno.Nome) && String.IsNullOrEmpty(aluno.Email))
                    break;

                var mensagem = JsonSerializer.Serialize(aluno);

                var corpo = Encoding.UTF8.GetBytes(mensagem);

                await channel.BasicPublishAsync(exchange: string.Empty,
                                                                routingKey: "hello",
                                                                body: corpo);

                Console.WriteLine($"[x] Enviado '{mensagem}'");
            }
        }
    }

    public class Aluno
    {
        public string Nome { get; set; }
        public string Email { get; set; }
    }
    #endregion

    #region Com comentários
    //public class Program
    //{

    //    static async Task Main(string[] args)
    //    {
    //        // TODO: PASSO 1: Criar Conexão - Estabelece uma conexão com o servidor RabbitMQ usando a factory.
    //        // A conexão é essencial para que a aplicação se comunique com o RabbitMQ.
    //        var factory = new ConnectionFactory { HostName = "localhost" };
    //        using var connection = await factory.CreateConnectionAsync();

    //        // TODO: PASSO 2: Criar Canal - Utiliza a conexão estabelecida para criar um canal de comunicação com o RabbitMQ.
    //        // O canal é necessário para interagir com o RabbitMQ e realizar operações como declarar filas e publicar mensagens.
    //        using var channel = await connection.CreateChannelAsync();

    //        // TODO: PASSO 3: Declarar Fila - Declara a fila "hello" para garantir que ela exista antes de enviar qualquer mensagem.
    //        // durable: false - A fila não será persistida no disco, sendo removida se o servidor reiniciar.
    //        // exclusive: false - A fila não é exclusiva para esta conexão.
    //        // autoDelete: false - A fila não será removida automaticamente quando não houver consumidores.
    //        await channel.QueueDeclareAsync(queue: "hello",
    //                                        durable: false,
    //                                        exclusive: false,
    //                                        autoDelete: false,
    //                                        arguments: null);

    //        Console.Write("Digite uma mensagem e aperte ENTER: ");

    //        while (true)
    //        {
    //            // TODO: PASSO 4: Criar e Popular Objeto Aluno - Cria um novo objeto 'Aluno' e solicita que o usuário forneça o nome e o e-mail.
    //            var aluno = new Aluno();

    //            // Solicita ao usuário que insira o nome do aluno.
    //            Console.Write("NOME: ");
    //            aluno.Nome = Console.ReadLine().ToString();

    //            // Solicita ao usuário que insira o e-mail do aluno.
    //            Console.Write("E-MAIL: ");
    //            aluno.Email = Console.ReadLine().ToString();

    //            // TODO: PASSO 5: Verificar Finalização - Se ambos os campos (Nome e Email) estiverem vazios, sai do loop.
    //            if (string.IsNullOrEmpty(aluno.Nome) && string.IsNullOrEmpty(aluno.Email))
    //                break;

    //            // TODO: PASSO 6: Serializar Mensagem - Serializa o objeto 'Aluno' para o formato JSON.
    //            // Em seguida, converte a mensagem serializada em um array de bytes usando UTF-8, já que o RabbitMQ trabalha com dados binários.
    //            var mensagem = JsonSerializer.Serialize(aluno);
    //            var corpo = Encoding.UTF8.GetBytes(mensagem);

    //            // TODO: PASSO 7: Publicar Mensagem - Utiliza o canal para publicar a mensagem na fila "hello".
    //            // exchange: string.Empty - Não utiliza uma exchange específica, usa a exchange padrão do RabbitMQ.
    //            // routingKey: "hello" - Define a fila de roteamento para "hello".
    //            await channel.BasicPublishAsync(exchange: string.Empty,
    //                                            routingKey: "hello",
    //                                            body: corpo);

    //            // Informa ao usuário que a mensagem foi enviada com sucesso.
    //            Console.WriteLine($"[x] Enviado '{mensagem}'");
    //        }
    //    }
    //}

    //// Classe Aluno - Define as propriedades Nome e Email, usadas para serializar e enviar as mensagens.
    //public class Aluno
    //{
    //    public string Nome { get; set; }
    //    public string Email { get; set; }
    //}
    #endregion
}

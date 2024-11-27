
# RabbitMQ Producer Example

Este repositório demonstra como criar um produtor simples usando RabbitMQ em .NET. O exemplo implementa um processo para conectar-se ao RabbitMQ, declarar filas e publicar mensagens.

## Tecnologias Utilizadas

- .NET 6+
- RabbitMQ.Client
- System.Text.Json

---

## Funcionalidades

1. **Conexão com RabbitMQ**: Estabelece uma conexão com o servidor RabbitMQ.
2. **Criação de Canal**: Cria um canal de comunicação para interagir com o RabbitMQ.
3. **Declaração de Fila**: Garante que a fila exista antes de enviar mensagens.
4. **Criação e Serialização de Mensagens**: Converte objetos em JSON e os envia como mensagens binárias.
5. **Publicação de Mensagens**: Envia mensagens para a fila especificada.

---

## Documentação Completa

Para mais detalhes sobre RabbitMQ com C#, confira o PDF:
[📄 RabbitMQ com C# - Documentação Completa](RabbitMQ_com_CSharp.pdf)

---

## Estrutura do Projeto

- `Program`: Classe principal que implementa o produtor.
- `Aluno`: Classe modelo para representar os dados enviados.

---

## Configuração Local

Certifique-se de que o RabbitMQ esteja em execução localmente. Use as configurações padrão:

```csharp
var factory = new ConnectionFactory { HostName = "localhost" };
```

---

## Configuração AWS

Caso utilize um ambiente em nuvem, atualize as credenciais:

```csharp
var factory = new ConnectionFactory
{
    Uri = new Uri("amqps://"),
    Port = 5671,
    VirtualHost = "/",
    UserName = "<seu_usuario>",
    Password = "<sua_senha>"
};
```

---

## Como Executar

1. Clone o repositório:
   ```bash
   git clone https://github.com/seu-usuario/rabbitmq-producer.git
   cd rabbitmq-producer
   ```

2. Instale as dependências do RabbitMQ no seu ambiente local ou utilize uma instância na nuvem.

3. Compile e execute o projeto:
   ```bash
   dotnet build
   dotnet run
   ```

4. Insira os dados solicitados no console (nome e e-mail) para enviar mensagens à fila `hello`.

---

## Código de Exemplo

### Produtor

```csharp
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public class Program
{
    static async Task Main(string[] args)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
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
            aluno.Nome = Console.ReadLine();
            Console.Write("E-MAIL: ");
            aluno.Email = Console.ReadLine();

            if (string.IsNullOrEmpty(aluno.Nome) && string.IsNullOrEmpty(aluno.Email))
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
```

---

## Licença

Este projeto está sob a licença MIT. Consulte o arquivo `LICENSE` para mais detalhes.

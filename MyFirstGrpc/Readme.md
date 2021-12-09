# Introduction
gRPC est une technologie RPC (Remote Procedure Call) d�velopp�e par Google.
Elle n�cessite l'�tablissement d'un **contrat** (appel� proto ou prototype) qui d�crit les int�ractions possibles avec le service.

# Le proto

Le proto est un fichier qui utilise le langage `Protocol Buffers` pour d�crire la structure d'un service.

``` proto
syntax = "proto3";
import "google/protobuf/empty.proto";												//Permet l'utilisation du type de message "vide".

option csharp_namespace = "MyFirstGrpc.Server";										//Espace de nom � utiliser pour le code g�n�r�.

package greet;


service Greeter {																	// D�finition d'un service nomm� Greeter.

  rpc SayHello (HelloRequest) returns (HelloReply);									//Cr�ation d'une proc�dure qui prend en param�tre un message de type HelloRequest et retourne un message de type HelloReply

  rpc SendRandomData (google.protobuf.Empty) returns (stream SendRandomDataReply);	//Cr�ation d'une proc�dure qui prend en param�tre un message de type Empty (vide) et retourne un flux (stream) de messages de type SendRandomDataReply
}


message HelloRequest {																//D�finition de la structure du message HelloRequest
  string name = 1;																	//Le message HelloRequest contient une propri�t� "name" de type "string"
}

message HelloReply {																//D�finition de la structure du message HelloRequest
  string message = 1;
}

message SendRandomDataReply {														//D�finition de la structure du message SendRandomDataReply
  int32 data = 1;
}
```

Le fichier proto devra �tre connu par le serveur et le client. Il est utilis� par gRPC pour g�n�rer automatiquement l'ensemble du code n�cessaire � la structure du service.

# Le serveur

En .NET, un service gRPC est expos� par un serveur web ASP.NET Core


``` csharp
// ./Program.cs

using MyFirstGrpc.Server.Services;

var builder = WebApplication.CreateBuilder(args);	//Pr�paration d'un serveur Web ASP.NET Core (initialisation du builder)
builder.Services.AddGrpc();							//Ajout de la prise en charge de gRPC.

var app = builder.Build();							//Cr�ation de de l'application ASP.NET Core � partir du builder.

app.MapGrpcService<GreeterService>();				//Ajout du service Greeter comme service gRPC.

													//Cr�ation d'un message � la racine du site sur GET pour informer le visiteur de la n�cessiter de passer par gRPC.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();											//D�marrage du serveur Web ASP.NET Core.


```

``` csharp
// ./Services/GreeterService.cs

//Impl�mentation du service Greeter, h�rite de Greeter.GreeterBase qui a �t� g�n�r� automatiquement � partir du proto.
public class GreeterService : Greeter.GreeterBase
{
    //Impl�mentation de la proc�dure SayHello
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        //request contient la requ�te de l'utilisateur.
        //context contient des informations sur le client et la connexion.

        //La m�thode retourne un message HelloReply.
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }

    //Impl�mentation de la proc�dure SendRandomData
    public override async Task SendRandomData(Empty request, IServerStreamWriter<SendRandomDataReply> responseStream, ServerCallContext context)
    {
        Random random = new Random();

        //Tant que le client n'a pas demand� l'arr�t de la diffusion ou ne s'est pas d�connect�
        while (!context.CancellationToken.IsCancellationRequested)
        {
            //On g�n�re une valeure comprise entre 1 et 5
            int data = random.Next(1, 5);

            //On �crit dans le flux la valeur en asynchrone, elle va �tre envoy�e au client
            await responseStream.WriteAsync(new SendRandomDataReply()
            {
               Data = data,
            });

            //On attent avant d'envoyer une nouvelle valeure.
            await Task.Delay(TimeSpan.FromSeconds(data), context.CancellationToken);
        }
    }
}

```

# Le client

EN .NET, l'impl�mentation gRPC n�cessite l'import de trois packages NuGet :

``` ps
Install-Package Grpc.Net.Client
Install-Package Google.Protobuf
Install-Package Grpc.Tools
```

Il est �galement n�cessaire d'ajouter le fichier proto et de modifier le fichier projet :

``` xml
  <ItemGroup>
    <Protobuf Include="Protos\greet.proto" GrpcServices="Client" />
  </ItemGroup>
```

Ensuite, le client peut appeler le serveur comme ceci :

``` csharp
using Grpc.Core;
using Grpc.Net.Client;
using MyFirstGrpc.Client;


using var channel = GrpcChannel.ForAddress("https://localhost:7002");                   //Cr�ation d'une connection au serveur (peut �tre utilis�e par plusieurs clients, prend en charge le multiplexage et le multithreading)
var client = new Greeter.GreeterClient(channel);                                        //Cr�ation d'un client
var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });    //Appel asynchrone d'une proc�dure unaire
Console.WriteLine("Greeting: " + reply.Message);                                        //Affichage du r�sultat


var call = client.SendRandomData(new Google.Protobuf.WellKnownTypes.Empty());           //Cr�ation d'un abonnement � une proc�dure RPC qui retourne un flux.

await foreach (SendRandomDataReply item in call.ResponseStream.ReadAllAsync())          //Foreach asynchrone qui r�alise la lecture du flux (on entre dans la boucle � chaque r�ception d'un message).
{
    Console.WriteLine(item.Data);                                                       //On affiche les donn�es r�ceptionn�s.
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
```

# Documentation
Pour l'impl�mentation gRPC en .NET, vous pouvez vous r�f�rer � la (documentation officielle)[https://docs.microsoft.com/fr-fr/aspnet/core/grpc/?view=aspnetcore-6.0].
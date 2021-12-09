# Introduction
gRPC est une technologie RPC (Remote Procedure Call) développée par Google.
Elle nécessite l'établissement d'un **contrat** (appelé proto ou prototype) qui décrit les intéractions possibles avec le service.

# Le proto

Le proto est un fichier qui utilise le langage `Protocol Buffers` pour décrire la structure d'un service.

``` proto
syntax = "proto3";
import "google/protobuf/empty.proto";												//Permet l'utilisation du type de message "vide".

option csharp_namespace = "MyFirstGrpc.Server";										//Espace de nom à utiliser pour le code généré.

package greet;


service Greeter {																	// Définition d'un service nommé Greeter.

  rpc SayHello (HelloRequest) returns (HelloReply);									//Création d'une procédure qui prend en paramètre un message de type HelloRequest et retourne un message de type HelloReply

  rpc SendRandomData (google.protobuf.Empty) returns (stream SendRandomDataReply);	//Création d'une procédure qui prend en paramètre un message de type Empty (vide) et retourne un flux (stream) de messages de type SendRandomDataReply
}


message HelloRequest {																//Définition de la structure du message HelloRequest
  string name = 1;																	//Le message HelloRequest contient une propriété "name" de type "string"
}

message HelloReply {																//Définition de la structure du message HelloRequest
  string message = 1;
}

message SendRandomDataReply {														//Définition de la structure du message SendRandomDataReply
  int32 data = 1;
}
```

Le fichier proto devra être connu par le serveur et le client. Il est utilisé par gRPC pour générer automatiquement l'ensemble du code nécessaire à la structure du service.

# Le serveur

En .NET, un service gRPC est exposé par un serveur web ASP.NET Core


``` csharp
// ./Program.cs

using MyFirstGrpc.Server.Services;

var builder = WebApplication.CreateBuilder(args);	//Préparation d'un serveur Web ASP.NET Core (initialisation du builder)
builder.Services.AddGrpc();							//Ajout de la prise en charge de gRPC.

var app = builder.Build();							//Création de de l'application ASP.NET Core à partir du builder.

app.MapGrpcService<GreeterService>();				//Ajout du service Greeter comme service gRPC.

													//Création d'un message à la racine du site sur GET pour informer le visiteur de la nécessiter de passer par gRPC.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();											//Démarrage du serveur Web ASP.NET Core.


```

``` csharp
// ./Services/GreeterService.cs

//Implémentation du service Greeter, hérite de Greeter.GreeterBase qui a été généré automatiquement à partir du proto.
public class GreeterService : Greeter.GreeterBase
{
    //Implémentation de la procédure SayHello
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        //request contient la requête de l'utilisateur.
        //context contient des informations sur le client et la connexion.

        //La méthode retourne un message HelloReply.
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }

    //Implémentation de la procédure SendRandomData
    public override async Task SendRandomData(Empty request, IServerStreamWriter<SendRandomDataReply> responseStream, ServerCallContext context)
    {
        Random random = new Random();

        //Tant que le client n'a pas demandé l'arrêt de la diffusion ou ne s'est pas déconnecté
        while (!context.CancellationToken.IsCancellationRequested)
        {
            //On génère une valeure comprise entre 1 et 5
            int data = random.Next(1, 5);

            //On écrit dans le flux la valeur en asynchrone, elle va être envoyée au client
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

EN .NET, l'implémentation gRPC nécessite l'import de trois packages NuGet :

``` ps
Install-Package Grpc.Net.Client
Install-Package Google.Protobuf
Install-Package Grpc.Tools
```

Il est également nécessaire d'ajouter le fichier proto et de modifier le fichier projet :

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


using var channel = GrpcChannel.ForAddress("https://localhost:7002");                   //Création d'une connection au serveur (peut être utilisée par plusieurs clients, prend en charge le multiplexage et le multithreading)
var client = new Greeter.GreeterClient(channel);                                        //Création d'un client
var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });    //Appel asynchrone d'une procédure unaire
Console.WriteLine("Greeting: " + reply.Message);                                        //Affichage du résultat


var call = client.SendRandomData(new Google.Protobuf.WellKnownTypes.Empty());           //Création d'un abonnement à une procédure RPC qui retourne un flux.

await foreach (SendRandomDataReply item in call.ResponseStream.ReadAllAsync())          //Foreach asynchrone qui réalise la lecture du flux (on entre dans la boucle à chaque réception d'un message).
{
    Console.WriteLine(item.Data);                                                       //On affiche les données réceptionnés.
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
```

# Documentation
Pour l'implémentation gRPC en .NET, vous pouvez vous référer à la (documentation officielle)[https://docs.microsoft.com/fr-fr/aspnet/core/grpc/?view=aspnetcore-6.0].
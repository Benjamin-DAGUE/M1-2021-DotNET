using System.Web;
using System.Text.Json;
using imdbtester;

HttpClient client = new HttpClient();
client.BaseAddress = new Uri("http://www.omdbapi.com/");

Console.Write("Rechercher un film : ");
string search = Console.ReadLine() ?? throw new Exception("La recherche est obligatoire");
var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"?s={HttpUtility.UrlEncode(search)}&apikey=YOURAPIKEYHERE");

var response = client.Send(requestMessage);

if (response.IsSuccessStatusCode)
{
    SearchResult? result = await JsonSerializer.DeserializeAsync<SearchResult>(response.Content.ReadAsStream());

    if (result?.Search?.Count > 0)
    {
        foreach (Film film in result.Search)
        {
            Console.WriteLine(film.Title);
        }
    }
    else
    {
        Console.WriteLine("Aucun résultat");
    }

    Console.WriteLine(result);
}
else
{
    Console.WriteLine("Erreur lors de l'exécution de la requête.");
}


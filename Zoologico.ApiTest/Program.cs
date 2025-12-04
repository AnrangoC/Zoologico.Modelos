namespace Zoologico.ApiTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri("https://localhost:7011/");
            var response = httpClient.GetAsync("api/Especies").Result;
            var json = response.Content.ReadAsStringAsync().Result;

            var especies = Newtonsoft.Json.JsonConvert.DeserializeObject<Modelos.ApiResult<List<Modelos.Especie>>>(json);

            Console.WriteLine(json);
            Console.ReadLine();
        }
    }
}

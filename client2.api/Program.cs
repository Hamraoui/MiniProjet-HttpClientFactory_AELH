


using client2.api;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

class Program
{
    static async Task Main(string[] args)
    {
        var usersURL = "https://localhost:7013/api/User";

        /* regular httpclient
         * bad for mid/big size projects */


        /* using(var httpClient = new HttpClient())
         {
             ...
         }*/

        // creating ihttpClientFactory instance, thru dependency injection

        var serviceCollection = new ServiceCollection();

        ConfigureServices(serviceCollection);

        var services = serviceCollection.BuildServiceProvider();
       

        var httpClientFactory = services.GetRequiredService<IHttpClientFactory>();

        // creating httpclient instance ( basic use )

        var httpClient = httpClientFactory.CreateClient();

        // specifiying response format
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        var response = await httpClient.GetAsync(usersURL);

        response.EnsureSuccessStatusCode();

        if (response.IsSuccessStatusCode)
        {
            // deserialize the content of the response ( JSON ) into IEnumerable of users details
            var users = await response.Content.ReadFromJsonAsync<IEnumerable<UserDto2>>();

            foreach (var user in users)
            {
                Console.WriteLine(user.username);
            }
        }
        else { Console.WriteLine("No Result"); }

        Console.ReadLine();
    }

    public static void ConfigureServices (ServiceCollection services)
    {
        services.AddHttpClient();
    }
}


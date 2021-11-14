using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace clienttest
{
    class MainClass
    {
        static readonly HttpClient client = new HttpClient();
        static async Task Main()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                DataSerializer ds = new DataSerializer("json");
                HttpResponseMessage response = await client.GetAsync("http://127.0.0.1:4000/Ping");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Console.WriteLine($"Ответ на /ping: {responseBody}");
                response = await client.GetAsync("http://127.0.0.1:4000/GetInputData");
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Ответ на /getinputdata: {responseBody}");
                Input json = ds.Deserialized(responseBody);
                Output output = ds.GetAnswer(json);
                string serOutput = ds.Serialized(output);
                Console.WriteLine($"Вычисленный ответ на задание: {serOutput}");

                response = await client.GetAsync($"http://127.0.0.1:4000/WriteAnswer?answer=\"{serOutput}\"");
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Оценка ответа: {responseBody}");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }
}

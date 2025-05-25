using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Text;

public static class MyHttpReq
{
    public static HttpClient client;

    public static async Task SendHttpGET()
    {
        HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(" https://api.thecatapi.com/v1/images/search");
            string content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
    }

    public static async Task SendHttpGETWithHeaders()
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "CSharpApp");
        HttpResponseMessage response = await client.GetAsync(" https://api.thecatapi.com/v1/images/search");
        string content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);
    }

    public static async Task SendHttpPOSTithHeaders()
    {
        HttpClient client = new HttpClient();
        var data = new StringContent("{\"key\":\"value\"}", Encoding.UTF8, "application/json");
        var postResponse = await client.PostAsync("https://httpbin.org/post", data);
        string content = await postResponse.Content.ReadAsStringAsync();
        Console.WriteLine(content);
    }
}

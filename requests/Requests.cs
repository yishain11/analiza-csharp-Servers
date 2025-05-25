using System.Net.Http;
using System;
using System.Threading.Tasks;

public static class MyHttpReq
{
    public static async Task SendHttpGET()
    {
        HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(" https://api.thecatapi.com/v1/images/search");
            string content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
    }
}

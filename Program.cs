using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Servers
{
    internal class Program
    {
        public static async Task runServer()
        {
            string url = "http://localhost:8080/";
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();

            Console.WriteLine($"Listening at {url}...");

            while (true)
            {
                // Wait for an incoming request
                HttpListenerContext context = await listener.GetContextAsync();

                // Get request and response objects
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                Console.WriteLine($"[{DateTime.Now}] {request.HttpMethod} {request.Url}");

                // Build response
                string responseString = "<html><body><h1>Hello from C# HTTP Server!</h1></body></html>";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);

                response.ContentLength64 = buffer.Length;
                response.ContentType = "text/html";

                // Write the response
                using (var output = response.OutputStream)
                {
                    await output.WriteAsync(buffer, 0, buffer.Length);
                }
            }

        }



        public static async Task Main(string[] args)
        {
            //await runServer();
            Task t = MyHttpReq.SendHttpGET();
            await t;
        }
    }
}

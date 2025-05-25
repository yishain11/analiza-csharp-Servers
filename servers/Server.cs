using System.IO;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

public class ServerExample {

    public async Task genServer() { 
        HttpListener server = new HttpListener();
        server.Prefixes.Add("http://localhost:5000/");
        server.Start();
        Console.WriteLine("Server listening on http://localhost:5000/");
        while (true)
        {
            var context = await server.GetContextAsync();
            var request = context.Request;
            var response = context.Response;

            string path = request.Url.AbsolutePath;
            string method = request.HttpMethod;
            string output = "";
            int status = 200;

            switch (path)
            {
                case "/hello":
                    if (method == "GET")
                        output = "Hello from section 1!";
                    else
                        status = 405;
                    break;

                case "/echo":
                    if (method == "POST")
                    {
                        var reader = new StreamReader(request.InputStream);
                        string body = await reader.ReadToEndAsync();
                        try
                        {
                            var json = JsonDocument.Parse(body);
                            string msg = json.RootElement.GetProperty("message").GetString();
                            output = $"Echo: {msg}";
                        }
                        catch
                        {
                            status = 400;
                            output = "Invalid JSON. Expected: { \"message\": \"...\" }";
                        }
                    }
                    else status = 405;
                    break;

                case "/headers":
                    if (method == "GET" && request.Headers["X-Secret"] == "123")
                        output = "Header check passed!";
                    else
                    {
                        status = 403;
                        output = "Missing or wrong header.";
                    }
                    break;

                case "/auth":
                    if (method == "POST" && request.Headers["Authorization"] != null)
                    {
                        string auth = request.Headers["Authorization"];
                        if (auth.StartsWith("Basic "))
                        {
                            string encoded = auth.Substring("Basic ".Length);
                            string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
                            if (decoded == "user:pass")
                                output = "Auth success!";
                            else
                                output = "Auth failed.";
                        }
                        else output = "Missing Basic prefix.";
                    }
                    else
                    {
                        status = 401;
                        response.Headers.Add("WWW-Authenticate", "Basic");
                        output = "Unauthorized";
                    }
                    break;

                case "/submit":
                    if (method == "PUT")
                    {
                        var reader = new StreamReader(request.InputStream);
                        string body = await reader.ReadToEndAsync();
                        try
                        {
                            var json = JsonDocument.Parse(body);
                            string name = json.RootElement.GetProperty("name").GetString();
                            string email = json.RootElement.GetProperty("email").GetString();
                            output = $"Received: {name}, {email}";
                        }
                        catch
                        {
                            status = 400;
                            output = "Expected JSON: { \"name\": \"\", \"email\": \"\" }";
                        }
                    }
                    else status = 405;
                    break;

                default:
                    status = 404;
                    output = "Unknown endpoint.";
                    break;
            }

            byte[] buffer = Encoding.UTF8.GetBytes(output);
            response.StatusCode = status;
            response.ContentType = "text/plain";
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer);
            response.Close();
        }
    }



}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using RosMolExtension;

namespace RosMolServer
{
    public delegate Response? RequestAction(NameValueCollection headers, string content);

    internal static class Listener
    {
        public static event RequestAction? OnListened;

        public static bool AllowDebug =
#if DEBUG
            true;
#else
            false;
#endif

        public static Thread StartRecivingThreard(string prefix, CancellationToken token)
        {
            Thread thread = new(new ParameterizedThreadStart((a) => StartListening(prefix, token)));
            thread.Start(prefix);
            return thread;
        }

        private static async void StartListening(string prefix, CancellationToken token)
        {
            if (!HttpListener.IsSupported)
            {
                throw new Exception("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
            }

            if (prefix == null || prefix.Length == 0)
                throw new Exception("Prefixes is null!");

            var httpListener = new HttpListener();

            try
            {
                httpListener.Prefixes.Add(prefix);

                httpListener.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                Console.WriteLine("Enter prefix:");

                string? cmd = Console.ReadLine();

                if (cmd == null || cmd.Length == 0)
                {
                    Console.WriteLine("Listener aborted!");
                    return;
                }
                else
                {
                    StartListening(cmd, token);

                    return;
                }
            }

            Console.WriteLine($"Http listener has started on Uri: {prefix}");

            while (!token.IsCancellationRequested)
            {
                var context = await httpListener.GetContextAsync();

                HttpListenerRequest request = context.Request;

                if (AllowDebug)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    Console.WriteLine($"Client addr: {request.RemoteEndPoint}");
                    Console.WriteLine($"Request url: {request.Url}");
                    Console.WriteLine("Headers:");

                    for (int i = 0; i < request.Headers.Count; i++)
                    {
                        Console.Write($"{request.Headers.Keys[i]}: {request.Headers[i]}; ");
                    }

                    Console.WriteLine();

                    Console.ResetColor();
                }

                var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                string requestBody = await reader.ReadToEndAsync();

                request.InputStream.Close();
                reader.Close();

                string? responseString;

                try
                {
                    responseString = OnListened?.Invoke(request.Headers, requestBody)?.ToString();
                }catch(Exception e)
                {
                    responseString = new ErrorResponse(Response.Status.Error).ToString();
                }

                if (responseString != null)
                {
                    if (AllowDebug)
                    {
                        Console.WriteLine($"Send to {request.RemoteEndPoint}: {responseString?.Substring(0, Math.Min(responseString.Length, 1024)) ?? "null"}");
                    }

                    HttpListenerResponse response = context.Response;
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;

                    using Stream output = response.OutputStream;
                    await output.WriteAsync(buffer);
                    await output.FlushAsync();
                }
            }
            httpListener.Stop();
        }

        public static async Task<string> GetResponse(string url, string code, params (string key, string value)[] args)
        {
            HttpRequestMessage requestMessage = new(HttpMethod.Get, url);

            requestMessage.Headers.Add("code", code);

            foreach (var (key, value) in args)
            {
                requestMessage.Headers.Add(key, value);
            }

            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(requestMessage);

            return await response.Content.ReadAsStringAsync();
        }
    }
}

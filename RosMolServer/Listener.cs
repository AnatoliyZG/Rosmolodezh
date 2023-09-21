using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace RosMolServer
{
    public delegate string RequestAction(NameValueCollection args);

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
            Thread thread = new Thread(new ParameterizedThreadStart((a) => StartListening(prefix, token)));
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

            HttpListener httpListener = new HttpListener();

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
                HttpListenerContext context = await Task.Run(httpListener.GetContextAsync, token);
                HttpListenerRequest request = context.Request;

                if (AllowDebug)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    Console.WriteLine($"Client addr: {request.RemoteEndPoint}");
                    Console.WriteLine($"Request url: {request.Url}");
                    Console.WriteLine("Headers:");

                    for (int i = 0; i < request.Headers.Count; i++)
                    {
                        Console.Write($" {request.Headers.Keys[i]}: {request.Headers[i]}");
                    }

                    Console.WriteLine("\nArgs:");

                    for (int i = 0; i < request.QueryString.Count; i++)
                    {
                        Console.WriteLine($"{request.QueryString.AllKeys[i]}: {request.QueryString[i]}");
                    }

                    Console.ResetColor();
                }

                string? responseString = OnListened?.Invoke(request.Headers);

                if (responseString != null)
                {
                    HttpListenerResponse response = context.Response;
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;

                    using Stream output = response.OutputStream;
                    await output.WriteAsync(buffer, 0, buffer.Length);
                    await output.FlushAsync();
                }
            }
            httpListener.Stop();
        }

        public static async Task<string> GetResponse(string url, string code, params (string key, string value)[] args)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            requestMessage.Headers.Add("code", code);

            foreach (var arg in args)
            {
                requestMessage.Headers.Add(arg.key, arg.value);
            }

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(requestMessage);

            return await response.Content.ReadAsStringAsync();
        }
    }
}

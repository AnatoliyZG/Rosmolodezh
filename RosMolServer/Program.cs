using System.Collections.Specialized;
using RosMolServer;

string defaultPrefix = "http://*:4447/connection/";

CancellationTokenSource tokenSource = new CancellationTokenSource();

Listener.StartRecivingThreard(defaultPrefix, tokenSource.Token);

InputRequests inputRequests = new InputRequests();

Listener.OnListened += inputRequests.Input;

while (true)
{
    try
    {
        string[] cmd = Console.ReadLine().Split();
        switch (cmd[0])
        {
            case "response":
                if (cmd.Length > 1)
                {
                    await Listener.GetResponse(cmd[1], "TEST-REQUEST", ("key", "value"));
                }
                else
                {
                    await Listener.GetResponse("http://localhost:4447/connection/", "TEST-REQUEST", ("key", "value"));
                }
                break;
            case "end":
                tokenSource.Cancel();
                return;
        }
    }catch(Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
}

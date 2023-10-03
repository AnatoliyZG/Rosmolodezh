using RosMolExtension;
using RosMolServer;


string defaultPrefix = "http://*:4447/connection/";

CancellationTokenSource tokenSource = new();

Listener.StartRecivingThreard(defaultPrefix, tokenSource.Token);

DataBase dataBase = await DataBase.CreateAsync(database: "C:\\USERS\\ZREGA\\DOCUMENTS\\ROSDB.MDF");

dataBase.GetCachedContent<AnnounceData>("Announces");

InputRequests inputRequests = new(dataBase);

Listener.OnListened += inputRequests.Input;

while (true)
{
    try
    {
        string[] cmd = Console.ReadLine()!.Split();

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
                dataBase.Dispose();
                tokenSource.Cancel();
                return;
        }
    }catch(Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
}

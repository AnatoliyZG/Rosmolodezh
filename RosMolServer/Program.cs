using RosMolExtension;
using RosMolServer;


string server = "5.42.220.135"; // "(localdb)\\MSSQLLocalDB"
string port = "3306";
string database = "default_db"; // "C:\\USERS\\ZREGA\\DOCUMENTS\\ROSDB.MDF";
string user = "gen_user";
string password = "Mt>!C7w$]@N,Y~";

string defaultPrefix = "http://*:4447/connection/";

CancellationTokenSource tokenSource = new();

Listener.StartRecivingThreard(defaultPrefix, tokenSource.Token);

DataBase dataBase = await DataBase.CreateAsync(server, port, database, user, password);


dataBase.CacheValue<AnnounceData>("Announces")
        .CacheValue<AnnounceData>("Options")
        .CacheValue<AnnounceData>("Wishes")
        .CacheValue<AnnounceData>("News")
        .CacheValue<AnnounceData>("Events");

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
            case "debug":
                Console.WriteLine("Debug: " + (Listener.AllowDebug = !Listener.AllowDebug));
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

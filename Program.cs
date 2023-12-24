using System.Net;
using System.Text;
using Microsoft.Win32;

namespace WinLockServer;

internal static class Program
{
    private static bool _lockState;
    
    private static void Main()
    {
        if (!OperatingSystem.IsWindows())
        {
            Console.Error.WriteLine("This program is only supported on Windows.");
            return;
        }
        
        SystemEvents.SessionSwitch += (_, args) =>
        {
            if (!OperatingSystem.IsWindows()) return; // Warning suppression
            
            _lockState = args.Reason switch
            {
                SessionSwitchReason.SessionLock => true,
                SessionSwitchReason.SessionUnlock => false,
                _ => _lockState
            };
        };

        HttpListener listener = new();
        listener.Prefixes.Add("http://+:26969/");
        listener.Start();
        listener.BeginGetContext(ReceiveGet, listener);
        
        Console.WriteLine("Server started");

        SystemEvents.SessionEnding += (_, _) => listener.Stop();
        
        while (listener.IsListening) Thread.Sleep(100);
    }

    private static void ReceiveGet(IAsyncResult res)
    {
        Console.WriteLine("Got request");
        
        HttpListener listener = (HttpListener) res.AsyncState!;
        HttpListenerContext ctx = listener.EndGetContext(res);

        HttpListenerResponse resp = ctx.Response;
        resp.StatusCode = 200;
        resp.ContentType = "text/plain";
        resp.ContentEncoding = Encoding.UTF8;
        resp.OutputStream.Write(Encoding.UTF8.GetBytes(_lockState ? "1" : "0"));
        resp.Close();
        
        listener.BeginGetContext(ReceiveGet, listener);
    }
}

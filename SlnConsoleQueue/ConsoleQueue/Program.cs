// See https://aka.ms/new-console-template for more information


using ConsoleQueue;

internal class Program
{
    private static void Main(string[] args)
    {        
      var queueManager = new QueueManager();

      //queueManager.SendMessagesAsync().GetAwaiter().GetResult();
      //Console.WriteLine("messages were sent");
      //Console.ReadLine();

      if (args.Length <= 0 || args[0] == "sender")
      {
          queueManager.SendMessagesAsync().GetAwaiter().GetResult();
          Console.WriteLine("messages were sent");
      }
      else if (args[0] == "receiver")
      {
          queueManager.ReceiveMessagesAsync().GetAwaiter().GetResult();
          Console.WriteLine("messages were received");
      }
      else
          Console.WriteLine("nothing to do");

      Console.ReadLine();        
    }
}
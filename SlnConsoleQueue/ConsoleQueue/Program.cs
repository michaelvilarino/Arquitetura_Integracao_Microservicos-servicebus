// See https://aka.ms/new-console-template for more information


using ConsoleQueue;
using Microsoft.Azure.ServiceBus;

internal class Program
{
    private static void Main(string[] args)
    {        
      var queueManager = new QueueManager();

      if (args.Contains("send"))
      {
          var messages = args.Where(f => f != "send").Select(s => s).ToList();

            messages.ForEach(message =>
            {
                queueManager.SendMessagesAsync(message).GetAwaiter().GetResult();
                Console.WriteLine($"message sended: {message} ");
            });
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
using Microsoft.Azure.ServiceBus;
using System.Text;

namespace ConsoleQueue
{
    public class QueueManager
    {
        static IQueueClient _queueClient;

        const string QueueConnectionString = "Endpoint=sb://michaelvilarino.servicebus.windows.net/;SharedAccessKeyName=ProductPolicy;SharedAccessKey=x1qq3Go3CDI9RCyDrqCNldoabVqDpoRGq+ASbH1FsfY=";
        const string QueuePath = "ProductChanged";

        public async Task SendMessagesAsync()
        {
            _queueClient = new QueueClient(QueueConnectionString, QueuePath);
            var messages = "Hi,Hello,Hey,How are you,Be Welcome".Split(',')
                .Select(msg => {
                    Console.WriteLine($"Will send message: {msg}");
                    return new Message(Encoding.UTF8.GetBytes(msg));
                }).ToList();

            var sendTask = _queueClient.SendAsync(messages);
            await sendTask;
            CheckCommunicationExceptions(sendTask);
            var closeTask = _queueClient.CloseAsync();
            await closeTask;
            CheckCommunicationExceptions(closeTask);

        }

        public async Task ReceiveMessagesAsync()
        {
            _queueClient = new QueueClient(QueueConnectionString, QueuePath);
            _queueClient.RegisterMessageHandler(MessageHandler,
                new MessageHandlerOptions(ExceptionHandler) { AutoComplete = false });
            Console.ReadLine();
            await _queueClient.CloseAsync();
        }
        public Task ExceptionHandler(ExceptionReceivedEventArgs exceptionArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionArgs.Exception}.");
            var context = exceptionArgs.ExceptionReceivedContext;
            Console.WriteLine($"Endpoint:{context.Endpoint}, Path:{context.EntityPath}, Action:{context.Action}");
            return Task.CompletedTask;
        }

        public async Task MessageHandler(Message message, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Received message:{Encoding.UTF8.GetString(message.Body)}");
            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        public bool CheckCommunicationExceptions(Task task)
        {
            if (task.Exception == null || task.Exception.InnerExceptions.Count == 0) return true;

            task.Exception.InnerExceptions.ToList()
                .ForEach(innerException =>
                {
                    Console.WriteLine($"Error in SendAsync task:{ innerException.Message}.Details: { innerException.StackTrace}");        
                    if (innerException is ServiceBusCommunicationException)
                         Console.WriteLine("Connection Problem with Host");
                });

            return false;
        }

    }
}

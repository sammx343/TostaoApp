using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;

namespace TostaoApp.Conexion
{
    public class ServiceBus
    {
        const string ServiceBusConnectionString = "Endpoint=sb://evaapp.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=KkEqOUkJD4XQoq4W04D7Lyg5P/Sq1FQg2B1KUYMf45w=";
        const string QueueName = "myqueue";
        //static IQueueClient queueClient;

        public async Task MainAsync(string message)
        {
            //queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after sending all the messages.");
            Console.WriteLine("======================================================");

            // Send messages.
            await SendMessagesAsync(message);

            //await queueClient.CloseAsync();
        }

        private async Task SendMessagesAsync(string text)
        {
            try
            {
                string messageBody = $"{text}";

               // var message = new Microsoft.Azure.ServiceBus.Message(Encoding.UTF8.GetBytes(messageBody));

                // Write the body of the message to the console.
                Console.WriteLine($"Sending message: {messageBody}");

                // Send the message to the queue.
               // await queueClient.SendAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}
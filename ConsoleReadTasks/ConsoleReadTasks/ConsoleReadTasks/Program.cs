using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadTasks
{
    class Program
    {
        static void Main(string[] args)
        {

            // Set the machine to read queues and processes
            string machineToRead = System.Environment.MachineName;
            /*****
             * Chacking MSMQ status
             * *****/
            StringBuilder sendMessage = new StringBuilder();
            sendMessage.AppendLine(" Message from Daily Status Process on " + System.Environment.MachineName);
            sendMessage.AppendLine(System.Environment.MachineName + " Searching on machine  " + machineToRead);
            Console.WriteLine("<--------Checking MSMQ status --------------->");
            sendMessage.AppendLine("<--------Checking MSMQ status --------------->");
            sendMessage.AppendLine(" Reading MSMQ Status");

            // read all the queues
            var queues = MessageQueue.GetPrivateQueuesByMachine(machineToRead);
            foreach (MessageQueue queue in queues)
            {

                MessageQueue new_queue = new MessageQueue(queue.Path);
                queue.MessageReadPropertyFilter.SentTime = true;
                queue.MessageReadPropertyFilter.Body = true;
                new_queue.MessageReadPropertyFilter.SentTime = true;
                Message[] msgs = new_queue.GetAllMessages();
                // We will keep track of the error queue
                if (queue.QueueName == "private$\\error")
                {
                    sendMessage.AppendLine(" Error Queue :" + msgs.Length);
                    Console.WriteLine(" Error Queue :" + msgs.Length);
                }
            }
            sendMessage.AppendLine("-------------End of EMail--------------------------");
            MailMessage nMail = new MailMessage();
            nMail.To.Add("test@google.com");
            nMail.From = new MailAddress("test@google.com");
            nMail.Subject = ("Testing A message from " + System.Environment.MachineName);
            nMail.Body = sendMessage.ToString();
            SmtpClient sc = new SmtpClient("localhost");
            sc.Send(nMail);
        }
    }
}

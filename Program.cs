
using Amqp;
using Amqp.Framing;
using Amqp.Types;
using System;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {

            string    url = (args.Length > 0) ? args[0] :
                "amqp://guest:guest@127.0.0.1:5672";
            string target = (args.Length > 1) ? args[1] : "exampleFilter";
            int     count = (args.Length > 2) ? Convert.ToInt32(args[2]) : 10;

            Address peerAddr = new Address(url);
            Connection connection = new Connection(peerAddr);
            Session session = new Session(connection);

            Message msg = new Message("I can match a filter");
            msg.ApplicationProperties = new ApplicationProperties();
            msg.ApplicationProperties["sn"] = 100;


            SenderLink sender = new SenderLink(session, "send-1", target);
            sender.Send(msg);
             
            sender.Close();
            session.Close();
            connection.Close();

        }
    }
}


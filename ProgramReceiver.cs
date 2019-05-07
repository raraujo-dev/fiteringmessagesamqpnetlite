
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
            string source = (args.Length > 1) ? args[1] : "exampleFilter";
            int     count = (args.Length > 2) ? Convert.ToInt32(args[2]) : 10;

            Address peerAddr = new Address(url);
            Connection connection = new Connection(peerAddr);
            Session session = new Session(connection);

	        Map filters = new Map();
            filters.Add(new Symbol("f1"), new DescribedValue(new Symbol("apache.org:selector-filter:string"), "sn = 100"));
            
            ReceiverLink receiver = new ReceiverLink(session, "receiver-", new Source() { Address = source, FilterSet = filters }, null);
            Message msg = receiver.Receive();
            Console.WriteLine(msg.ApplicationProperties.ToString());
            receiver.Accept(msg);
               
	        receiver.Close();
            session.Close();
            connection.Close();

        }
    }
}

# fiteringmessagesamqpnetlite
How to filter messages on AMQ .NET client?


This example is a producer that shows how to use application properties to filter messages on AMQ .NET client:

```
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
```

As well a simple receiver consuming only messages that match with the sn=100 string using application property:

```
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
```
Note: Ou need to declare on csproj file the AMQP.dll example below:

```
<ItemGroup>
  <Reference Include="AMQP">
          <HintPath>/home/raraujo/ferramentas/amq-dotnetcore/bin/netstandard2.0/AMQP.dll</HintPath>
  </Reference>
</ItemGroup>


```

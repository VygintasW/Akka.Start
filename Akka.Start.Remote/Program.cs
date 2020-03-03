using Akka.Actor;
using Akka.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Akka.Start.Remote
{
    class Program
    {
        private static ActorSystem MovieStreamingActorSystem { get; set; }

        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddXmlFile("hocon.xml");

            var configuration = builder.Build();

            var hoconConfig = configuration.GetSection("hocon");

            var config = ConfigurationFactory.ParseString(hoconConfig.Value);

            Console.WriteLine("Creating MovieStreamingActorSystem in remote process");

            MovieStreamingActorSystem = ActorSystem.Create(nameof(MovieStreamingActorSystem), config);

            await MovieStreamingActorSystem.WhenTerminated;
        }
    }
}

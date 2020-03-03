using Akka.Actor;
using System;
using System.Threading.Tasks;

namespace Akka.Start.Remote
{
    class Program
    {
        private static ActorSystem MovieStreamingActorSystem { get; set; }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Creating MovieStreamingActorSystem in remote process");

            MovieStreamingActorSystem = ActorSystem.Create(nameof(MovieStreamingActorSystem));

            await MovieStreamingActorSystem.WhenTerminated;
        }
    }
}
